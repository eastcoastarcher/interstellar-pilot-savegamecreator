class_name HeaderReader
extends RefCounted

# Reads the save header in the same field order as the Unity HeaderReader.cs.
# Returns a Dictionary, or an empty Dictionary if the file cannot be opened.
static func read_header(path: String) -> Dictionary:
	var f := FileAccess.open(path, FileAccess.READ)
	if f == null:
		push_error("Cannot open save: " + path)
		return {}

	var h := {}
	h["version"] = BinaryHelpers.read_version(f)
	h["created_version"] = BinaryHelpers.read_version(f)
	h["is_autosave"] = BinaryHelpers.read_bool(f)
	h["timestamp"] = BinaryHelpers.read_string(f)
	h["scenario_info_id"] = f.get_32()
	h["global_save_number"] = f.get_32()
	h["save_number"] = f.get_32()
	h["have_player"] = BinaryHelpers.read_bool(f)
	h["player_sector_name"] = BinaryHelpers.read_string(f)
	h["player_name"] = BinaryHelpers.read_string(f)
	h["credits"] = f.get_64()
	h["faction_name"] = BinaryHelpers.read_string(f)
	h["net_worth"] = f.get_64()
	h["permadeath"] = BinaryHelpers.read_bool(f)
	h["seconds_elapsed"] = f.get_double()
	h["game_start_date"] = BinaryHelpers.read_string(f)
	h["scenario_title"] = BinaryHelpers.read_string(f)
	h["scenario_author"] = BinaryHelpers.read_string(f)
	h["scenario_authoring_tool"] = BinaryHelpers.read_string(f)
	h["scenario_description"] = BinaryHelpers.read_string(f)
	h["header_end_offset"] = f.get_position()

	f.close()
	return h
