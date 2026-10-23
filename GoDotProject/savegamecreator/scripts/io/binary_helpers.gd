class_name BinaryHelpers
extends RefCounted

# Reads a version as three 32-bit ints: major, minor, build.
static func read_version(f: FileAccess) -> Array:
	return [f.get_32(), f.get_32(), f.get_32()]

static func read_bool(f: FileAccess) -> bool:
	return f.get_8() != 0

# .NET style 7-bit encoded length (used by BinaryReader.ReadString).
static func read_7bit_int(f: FileAccess) -> int:
	var result := 0
	var shift := 0
	while true:
		var b := f.get_8()
		result |= (b & 0x7f) << shift
		shift += 7
		if (b & 0x80) == 0:
			break
	return result

# Length-prefixed UTF-8 string, same as C# BinaryReader.ReadString.
static func read_string(f: FileAccess) -> String:
	var length := read_7bit_int(f)
	return f.get_buffer(length).get_string_from_utf8()
	
static func read_vec3(f: FileAccess) -> Vector3:
	return Vector3(f.get_float(), f.get_float(), f.get_float())

static func read_i32(f: FileAccess) -> int:
	var v := f.get_32()
	if v >= 2147483648:
		v -= 4294967296
	return v
