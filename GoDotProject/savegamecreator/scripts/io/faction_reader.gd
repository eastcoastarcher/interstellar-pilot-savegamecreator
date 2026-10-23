class_name FactionReader
extends RefCounted

# Reads the [Factions] section, which starts at byte 1451 (verified).
static func read_factions(path: String) -> Dictionary:
	var f := FileAccess.open(path, FileAccess.READ)
	if f == null:
		return {}
	f.seek(1451)
	var heading := BinaryHelpers.read_string(f)
	var count := BinaryHelpers.read_i32(f)
	var factions := []
	for i in range(count):
		factions.append(_read_faction(f))
	var result := {"heading": heading, "count": count, "factions": factions, "end_offset": f.get_position()}
	f.close()
	return result

static func _read_faction(f: FileAccess) -> Dictionary:
	var fa := {}
	fa["id"] = BinaryHelpers.read_i32(f)
	fa["generated_name_id"] = BinaryHelpers.read_i32(f)
	fa["generated_suffix_id"] = BinaryHelpers.read_i32(f)
	fa["custom_name"] = BinaryHelpers.read_string(f)
	fa["custom_short_name"] = BinaryHelpers.read_string(f)
	fa["home_sector_id"] = BinaryHelpers.read_i32(f)
	var has_home_pos := BinaryHelpers.read_bool(f)
	if has_home_pos:
		fa["home_sector_position"] = BinaryHelpers.read_vec3(f)
	fa["credits"] = BinaryHelpers.read_i32(f)
	fa["description"] = BinaryHelpers.read_string(f)
	fa["is_civilian"] = BinaryHelpers.read_bool(f)
	fa["faction_type"] = BinaryHelpers.read_i32(f)
	fa["aggression"] = f.get_float()
	fa["virtue"] = f.get_float()
	fa["greed"] = f.get_float()
	fa["cooperation"] = f.get_float()
	fa["trade_efficiency"] = f.get_float()
	fa["dynamic_relations"] = BinaryHelpers.read_bool(f)
	fa["show_job_boards"] = BinaryHelpers.read_bool(f)
	fa["create_jobs"] = BinaryHelpers.read_bool(f)
	fa["requisition_point_multiplier"] = f.get_float()
	fa["destroy_when_no_units"] = BinaryHelpers.read_bool(f)
	fa["min_npc_combat_efficiency"] = f.get_float()
	fa["max_npc_combat_efficiency"] = f.get_float()
	fa["additional_rp_provision"] = BinaryHelpers.read_i32(f)
	fa["trade_illegal_goods"] = BinaryHelpers.read_bool(f)
	fa["spawn_time"] = f.get_double()
	fa["highest_ever_net_worth"] = f.get_64()
	fa["ranking_system_id"] = BinaryHelpers.read_i32(f)
	fa["preferred_formation_id"] = BinaryHelpers.read_i32(f)
	if BinaryHelpers.read_bool(f):
		fa["custom_settings"] = _read_ai_settings(f)
	if BinaryHelpers.read_bool(f):
		fa["stats"] = _read_stats(f)
	var excluded := []
	var excluded_count := BinaryHelpers.read_i32(f)
	for j in range(excluded_count):
		excluded.append(BinaryHelpers.read_i32(f))
	fa["autopilot_excluded_sector_ids"] = excluded
	return fa

static func _read_ai_settings(f: FileAccess) -> Dictionary:
	var s := {}
	s["build_ships"] = BinaryHelpers.read_bool(f)
	s["repair_ships"] = BinaryHelpers.read_bool(f)
	s["upgrade_ships"] = BinaryHelpers.read_bool(f)
	s["repair_min_hull_damage"] = f.get_float()
	s["repair_min_credits_before_repair"] = BinaryHelpers.read_i32(f)
	s["preference_to_place_bounty"] = f.get_float()
	s["large_ship_preference"] = f.get_float()
	s["cloak_ship_preference"] = f.get_float()
	s["daily_income"] = BinaryHelpers.read_i32(f)
	s["hostile_with_all"] = BinaryHelpers.read_bool(f)
	s["min_fleet_unit_count"] = BinaryHelpers.read_i32(f)
	s["max_fleet_unit_count"] = BinaryHelpers.read_i32(f)
	s["offensive_stance"] = f.get_float()
	s["allow_other_faction_to_use_docks"] = BinaryHelpers.read_bool(f)
	s["preference_to_build_turrets"] = f.get_float()
	s["preference_to_build_stations"] = f.get_float()
	s["preference_to_have_ammo"] = f.get_float()
	s["ignore_station_credits_reserve"] = BinaryHelpers.read_bool(f)
	s["max_jump_distance_from_home_sector"] = BinaryHelpers.read_i32(f)
	s["max_station_build_distance_from_home_sector"] = BinaryHelpers.read_i32(f)
	s["pilot_gender"] = BinaryHelpers.read_i32(f)
	s["fixed_ship_count"] = BinaryHelpers.read_i32(f)
	s["sector_control_likelihood"] = f.get_float()
	return s

static func _read_stats(f: FileAccess) -> Dictionary:
	var st := {}
	st["total_ships_claimed"] = BinaryHelpers.read_i32(f)
	st["units_destroyed_by_class"] = _read_int_map(f)
	st["units_lost_by_class"] = _read_int_map(f)
	st["scratchcards_scratched"] = BinaryHelpers.read_i32(f)
	st["highest_scratchcard_win"] = BinaryHelpers.read_i32(f)
	return st

static func _read_int_map(f: FileAccess) -> Dictionary:
	var m := {}
	var n := BinaryHelpers.read_i32(f)
	for i in range(n):
		var k := BinaryHelpers.read_i32(f)
		m[k] = BinaryHelpers.read_i32(f)
	return m
