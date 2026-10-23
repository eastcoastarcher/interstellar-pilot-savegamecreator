class_name UnitReader
extends RefCounted

# Reads the [Units] base records, which start at byte 2477 (verified: 158 units, ends at 10705).
static func read_units(path: String) -> Dictionary:
	var f := FileAccess.open(path, FileAccess.READ)
	if f == null:
		return {}
	f.seek(2477)
	var heading := BinaryHelpers.read_string(f)
	var count := BinaryHelpers.read_i32(f)
	var units := []
	for i in range(count):
		units.append(_read_unit(f))
	var result := {"heading": heading, "count": count, "units": units, "end_offset": f.get_position()}
	f.close()
	return result

static func _read_unit(f: FileAccess) -> Dictionary:
	var u := {}
	u["id"] = BinaryHelpers.read_i32(f)
	u["seed"] = BinaryHelpers.read_i32(f)
	u["class_id"] = BinaryHelpers.read_i32(f)
	u["sector_id"] = BinaryHelpers.read_i32(f)
	u["position"] = BinaryHelpers.read_vec3(f)
	u["rotation"] = BinaryHelpers.read_vec3(f)
	u["faction_id"] = BinaryHelpers.read_i32(f)
	u["rp_provision"] = BinaryHelpers.read_i32(f)
	if BinaryHelpers.read_bool(f):
		var cargo := {}
		cargo["class_id"] = BinaryHelpers.read_i32(f)
		cargo["quantity"] = BinaryHelpers.read_i32(f)
		cargo["expires"] = BinaryHelpers.read_bool(f)
		cargo["spawn_time"] = f.get_double()
		u["cargo"] = cargo
	if BinaryHelpers.read_bool(f):
		var debris := {}
		debris["scrap_quantity"] = BinaryHelpers.read_i32(f)
		debris["expires"] = BinaryHelpers.read_bool(f)
		debris["expiry_time"] = f.get_double()
		debris["related_class_id"] = BinaryHelpers.read_i32(f)
		u["debris"] = debris
	if BinaryHelpers.read_bool(f):
		u["asteroid_remaining_yield"] = BinaryHelpers.read_i32(f)
	if BinaryHelpers.read_bool(f):
		var n := BinaryHelpers.read_i32(f)
		var items := []
		for j in range(n):
			var item := {}
			item["sell_multiplier"] = f.get_float()
			item["unit_class_id"] = BinaryHelpers.read_i32(f)
			items.append(item)
		u["ship_trader_items"] = items
	return u
