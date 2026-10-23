# verify_save.py - independent Python check of BountyUniverse.dat.
# Usage: python3 verify_save.py BountyUniverse.dat
# Re-checks every verified section offset. Prints PASS or FAIL per check.
import re, struct, sys

path = sys.argv[1] if len(sys.argv) > 1 else "BountyUniverse.dat"
d = open(path, "rb").read()
pos = 0
results = []

def i32():
    global pos; v = struct.unpack_from("<i", d, pos)[0]; pos += 4; return v
def i64():
    global pos; v = struct.unpack_from("<q", d, pos)[0]; pos += 8; return v
def f32():
    global pos; v = struct.unpack_from("<f", d, pos)[0]; pos += 4; return v
def f64():
    global pos; v = struct.unpack_from("<d", d, pos)[0]; pos += 8; return v
def u8():
    global pos; v = d[pos]; pos += 1; return v
def b8():
    return u8() != 0
def vec3():
    return (f32(), f32(), f32())
def s():
    global pos
    n = 0; sh = 0
    while True:
        c = d[pos]; pos += 1
        n |= (c & 0x7F) << sh; sh += 7
        if not c & 0x80:
            break
    t = d[pos:pos + n].decode("utf-8", "replace"); pos += n
    return t
def skip_heading(p):
    return p + 1 + d[p]

def check(name, got, expect):
    ok = got == expect
    results.append(ok)
    print(f"{'PASS' if ok else 'FAIL'}  {name}: {got} (expect {expect})")

def header():
    for _ in range(6): i32()          # version + created version
    b8(); s()                         # autosave, timestamp
    i32(); i32(); i32()               # scenario id, global save no, save no
    b8(); s(); s()                    # have player, sector, player name
    i64(); s(); i64()                 # credits, faction, net worth
    b8(); f64()                       # permadeath, seconds elapsed
    for _ in range(5): s()            # start date, title, author, tool, description

def sector():
    i32(); s(); vec3(); s(); f32(); i32(); vec3(); vec3(); vec3(); vec3(); f64(); f32()

def faction():
    i32(); i32(); i32(); s(); s(); i32()
    if b8(): vec3()
    i32(); s()
    b8(); i32()
    for _ in range(5): f32()
    for _ in range(3): b8()
    f32(); b8(); f32(); f32(); i32(); b8(); f64(); i64(); i32(); i32()
    if b8():
        for _ in range(3): b8()
        f32(); i32(); f32(); f32(); f32(); i32(); b8(); i32(); i32(); f32()
        b8(); f32(); f32(); f32(); b8(); i32(); i32(); i32(); i32(); f32()
    if b8():
        i32()
        for _ in range(2):
            n = i32()
            for _ in range(n):
                i32(); i32()
        i32(); i32()
    n = i32()
    for _ in range(n): i32()

def unit_base():
    for _ in range(4): i32()
    vec3(); vec3(); i32(); i32()
    if b8(): i32(); i32(); b8(); f64()
    if b8(): i32(); b8(); f64(); i32()
    if b8(): i32()
    if b8():
        n = i32()
        for _ in range(n):
            f32(); i32()

def radius():
    for _ in range(i32()): i32(); f32()
def named():
    for _ in range(i32()): i32(); s(); s()
def components():
    for _ in range(i32()):
        i32()
        if i32() == -1: s()
        if b8():
            for _ in range(i32()): i32(); f32()
def constructing():
    for _ in range(i32()): i32(); u8(); f32()
def damage_counter():
    for _ in range(i32()): unit_base(); f32()
def modded():
    for _ in range(i32()): i32(); i32(); i32()
def capacitor():
    for _ in range(i32()): i32(); f32()
def cloak():
    for _ in range(i32()): i32()
def powered_down():
    for _ in range(i32()):
        i32()
        for _ in range(i32()): i32()
def throttle():
    for _ in range(i32()): i32(); f32()
def cargo():
    for _ in range(i32()):
        i32()
        for _ in range(i32()): i32(); i32()
def shields():
    for _ in range(i32()):
        i32()
        for _ in range(u8()): u8(); f32()
def comp_health():
    for _ in range(i32()):
        i32()
        for _ in range(i32()): i32(); f32()
def active():
    for _ in range(i32()): i32(); vec3(); f32()
def unit_health():
    for _ in range(i32()): i32(); b8(); f32()

# 1. Header
pos = 0
header()
check("header ends", pos, 168)

# 2. Sectors
pos = skip_heading(168)
n = i32()
for _ in range(n): sector()
check("sectors count", n, 13)
check("sectors end", pos, 1451)

# 3. Factions
pos = skip_heading(1451)
n = i32()
for _ in range(n): faction()
check("factions count", n, 3)
check("factions end", pos, 1948)

# 4. Small faction sections
pos = skip_heading(1948)
for _ in range(i32()):
    i32(); n = u8()
    for _ in range(n): u8()
pos = skip_heading(pos)
for _ in range(i32()):
    i32(); n = i32()
    for _ in range(n): vec3(); i32()
pos = skip_heading(pos)
for _ in range(i32()):
    i32(); n = i32()
    for _ in range(n): i32(); b8(); b8(); i32(); f64()
pos = skip_heading(pos)
for _ in range(i32()):
    i32(); n = i32()
    for _ in range(n): i32(); f32()
pos = skip_heading(pos)
for _ in range(i32()):
    i32(); i32(); f32(); i32()
check("faction extras end", pos, 2477)

# 5. Units base records
pos = skip_heading(2477)
n = i32()
for _ in range(n): unit_base()
check("units count", n, 158)
check("units end", pos, 10705)

# 6. Unit component group
pos = skip_heading(10705)
for fn in [radius, named, components, constructing, damage_counter, modded, capacitor,
           cloak, powered_down, throttle, cargo, shields, comp_health, active, unit_health]:
    pos = skip_heading(pos) if fn is not radius else pos
    fn()
check("unit components end", pos, 11425)

# 7. Heading map (informational)
headings = []
for m in re.finditer(rb"\[[A-Za-z0-9 _.\-]+\]", d):
    st = m.start()
    if st >= 1 and d[st - 1] == len(m.group()):
        headings.append((st - 1, m.group().decode()))
check("heading count", len(headings), 65)

print()
print("ALL PASS" if all(results) else "SOME CHECKS FAILED")
