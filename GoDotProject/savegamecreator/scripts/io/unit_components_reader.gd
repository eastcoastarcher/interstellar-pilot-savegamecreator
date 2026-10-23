class_name UnitComponentsReader
extends RefCounted

# Reads the 15 unit component sections, from [UnitRadius] at byte 10705 to [FactionIntel] at byte 11425 (verified).
static func read_all(path: String) -> Dictionary:
	var f := FileAccess.open(path, FileAccess.READ)
	if f == null:
		return {}
	f.seek(10705)
	var out := {}
	BinaryHelpers.read_string(f)
	out["radius"] = _read_radius(f)
	BinaryHelpers.read_string(f)
	out["names"] = _read_names(f)
	BinaryHelpers.read_string(f)
	out["components"] = _read_components(f)
	BinaryHelpers.read_string(f)
	out["constructing"] = _read_constructing(f)
	BinaryHelpers.read_string(f)
	out["damage_counter"] = _read_damage_counter(f)
	BinaryHelpers.read_string(f)
	out["modded"] = _read_modded(f)
	BinaryHelpers.read_string(f)
	out["capacitor"] = _read_capacitor(f)
	BinaryHelpers.read_string(f)
	out["cloak"] = _read_cloak(f)
	BinaryHelpers.read_string(f)
	out["powered_down"] = _read_powered_down(f)
	BinaryHelpers.read_string(f)
	out["throttle"] = _read_throttle(f)
	BinaryHelpers.read_string(f)
	out["cargo"] = _read_cargo(f)
	BinaryHelpers.read_string(f)
	out["shields"] = _read_shields(f)
	BinaryHelpers.read_string(f)
	out["component_health"] = _read_component_health(f)
	BinaryHelpers.read_string(f)
	out["active"] = _read_active(f)
	BinaryHelpers.read_string(f)
	out["unit_health"] = _read_unit_health(f)
	out["end_offset"] = f.get_position()
	f.close()
	return out

static func _read_radius(f: FileAccess) -> Array:
	var list := []
	var n := BinaryHelpers.read_i32(f)
	for i in range(n):
		var e := {}
		e["unit_id"] = BinaryHelpers.read_i32(f)
		e["radius"] = f.get_float()
		list.append(e)
	return list

static func _read_names(f: FileAccess) -> Array:
	var list := []
	var n := BinaryHelpers.read_i32(f)
	for i in range(n):
		var e := {}
		e["unit_id"] = BinaryHelpers.read_i32(f)
		e["name"] = BinaryHelpers.read_string(f)
		e["short_name"] = BinaryHelpers.read_string(f)
		list.append(e)
	return list

static func _read_components(f: FileAccess) -> Array:
	var list := []
	var n := BinaryHelpers.read_i32(f)
	for i in range(n):
		var e := {}
		e["unit_id"] = BinaryHelpers.read_i32(f)
		var name_index := BinaryHelpers.read_i32(f)
		e["ship_name_index"] = name_index
		if name_index == -1:
			e["custom_ship_name"] = BinaryHelpers.read_string(f)
		if BinaryHelpers.read_bool(f):
			var items := []
			var fn := BinaryHelpers.read_i32(f)
			for j in range(fn):
				var item := {}
				item["state"] = BinaryHelpers.read_i32(f)
				item["production_elapsed"] = f.get_float()
				items.append(item)
			e["factory_items"] = items
		list.append(e)
	return list

static func _read_constructing(f: FileAccess) -> Array:
	var list := []
	var n := BinaryHelpers.read_i32(f)
	for i in range(n):
		var e := {}
		e["unit_id"] = BinaryHelpers.read_i32(f)
		e["state"] = f.get_8()
		e["progress"] = f.get_float()
		list.append(e)
	return list

static func _read_damage_counter(f: FileAccess) -> Array:
	var list := []
	var n := BinaryHelpers.read_i32(f)
	for i in range(n):
		var e := {}
		e["unit"] = UnitReader._read_unit(f)
		e["damage_received"] = f.get_float()
		list.append(e)
	return list

static func _read_modded(f: FileAccess) -> Array:
	var list := []
	var n := BinaryHelpers.read_i32(f)
	for i in range(n):
		var e := {}
		e["unit_id"] = BinaryHelpers.read_i32(f)
		e["bay_id"] = BinaryHelpers.read_i32(f)
		e["component_class_id"] = BinaryHelpers.read_i32(f)
		list.append(e)
	return list

static func _read_capacitor(f: FileAccess) -> Array:
	var list := []
	var n := BinaryHelpers.read_i32(f)
	for i in range(n):
		var e := {}
		e["unit_id"] = BinaryHelpers.read_i32(f)
		e["charge"] = f.get_float()
		list.append(e)
	return list

static func _read_cloak(f: FileAccess) -> Array:
	var list := []
	var n := BinaryHelpers.read_i32(f)
	for i in range(n):
		list.append(BinaryHelpers.read_i32(f))
	return list

static func _read_powered_down(f: FileAccess) -> Array:
	var list := []
	var n := BinaryHelpers.read_i32(f)
	for i in range(n):
		var e := {}
		e["unit_id"] = BinaryHelpers.read_i32(f)
		var bay_count := BinaryHelpers.read_i32(f)
		var bays := []
		for j in range(bay_count):
			bays.append(BinaryHelpers.read_i32(f))
		e["powered_down_bay_ids"] = bays
		list.append(e)
	return list

static func _read_throttle(f: FileAccess) -> Array:
	var list := []
	var n := BinaryHelpers.read_i32(f)
	for i in range(n):
		var e := {}
		e["unit_id"] = BinaryHelpers.read_i32(f)
		e["engine_throttle"] = f.get_float()
		list.append(e)
	return list

static func _read_cargo(f: FileAccess) -> Array:
	var list := []
	var n := BinaryHelpers.read_i32(f)
	for i in range(n):
		var e := {}
		e["unit_id"] = BinaryHelpers.read_i32(f)
		var item_count := BinaryHelpers.read_i32(f)
		var items := []
		for j in range(item_count):
			var item := {}
			item["cargo_class_id"] = BinaryHelpers.read_i32(f)
			item["quantity"] = BinaryHelpers.read_i32(f)
			items.append(item)
		e["items"] = items
		list.append(e)
	return list

static func _read_shields(f: FileAccess) -> Array:
	var list := []
	var n := BinaryHelpers.read_i32(f)
	for i in range(n):
		var e := {}
		e["unit_id"] = BinaryHelpers.read_i32(f)
		var point_count := f.get_8()
		var points := []
		for j in range(point_count):
			var p := {}
			p["point_index"] = f.get_8()
			p["health"] = f.get_float()
			points.append(p)
		e["damaged_shields"] = points
		list.append(e)
	return list

static func _read_component_health(f: FileAccess) -> Array:
	var list := []
	var n := BinaryHelpers.read_i32(f)
	for i in range(n):
		var e := {}
		e["unit_id"] = BinaryHelpers.read_i32(f)
		var comp_count := BinaryHelpers.read_i32(f)
		var comps := []
		for j in range(comp_count):
			var c := {}
			c["bay_id"] = BinaryHelpers.read_i32(f)
			c["health"] = f.get_float()
			comps.append(c)
		e["damaged_components"] = comps
		list.append(e)
	return list

static func _read_active(f: FileAccess) -> Array:
	var list := []
	var n := BinaryHelpers.read_i32(f)
	for i in range(n):
		var e := {}
		e["unit_id"] = BinaryHelpers.read_i32(f)
		e["velocity"] = BinaryHelpers.read_vec3(f)
		e["current_turn"] = f.get_float()
		list.append(e)
	return list

static func _read_unit_health(f: FileAccess) -> Array:
	var list := []
	var n := BinaryHelpers.read_i32(f)
	for i in range(n):
		var e := {}
		e["unit_id"] = BinaryHelpers.read_i32(f)
		e["is_destroyed"] = BinaryHelpers.read_bool(f)
		e["health"] = f.get_float()
		list.append(e)
	return list
