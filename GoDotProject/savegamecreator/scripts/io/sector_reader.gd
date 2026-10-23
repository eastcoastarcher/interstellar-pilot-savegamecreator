class_name SectorReader
extends RefCounted

# Reads the [Sectors] section, which starts at byte 168 (verified).
static func read_sectors(path: String) -> Dictionary:
	var f := FileAccess.open(path, FileAccess.READ)
	if f == null:
		return {}
	f.seek(168)
	var heading := BinaryHelpers.read_string(f)
	var count := BinaryHelpers.read_i32(f)
	var sectors := []
	for i in range(count):
		var s := {}
		s["id"] = BinaryHelpers.read_i32(f)
		s["name"] = BinaryHelpers.read_string(f)
		s["map_position"] = BinaryHelpers.read_vec3(f)
		s["description"] = BinaryHelpers.read_string(f)
		s["gate_distance_multiplier"] = f.get_float()
		s["random_seed"] = BinaryHelpers.read_i32(f)
		s["background_rotation"] = BinaryHelpers.read_vec3(f)
		s["ambient_light_color"] = BinaryHelpers.read_vec3(f)
		s["direction_light_color"] = BinaryHelpers.read_vec3(f)
		s["direction_light_rotation"] = BinaryHelpers.read_vec3(f)
		s["last_time_changed_control"] = f.get_double()
		s["light_direction_fudge"] = f.get_float()
		sectors.append(s)
	var result := {"heading": heading, "count": count, "sectors": sectors, "end_offset": f.get_position()}
	f.close()
	return result
