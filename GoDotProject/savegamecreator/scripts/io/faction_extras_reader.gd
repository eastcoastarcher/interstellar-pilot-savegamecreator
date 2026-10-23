class_name FactionExtrasReader
extends RefCounted

# Reads the five sections that follow [Factions], in file order, starting at byte 1948:
# Avatars, PatrolPaths, FactionRelations, FactionDamage, FactionOpinions.
static func read_all(path: String) -> Dictionary:
	var f := FileAccess.open(path, FileAccess.READ)
	if f == null:
		return {}
	f.seek(1948)
	var out := {}

	# [Avatars]
	BinaryHelpers.read_string(f)
	var avatar_blocks := []
	var avatar_count := BinaryHelpers.read_i32(f)
	for i in range(avatar_count):
		var fid := BinaryHelpers.read_i32(f)
		var n := f.get_8()
		var ids := []
		for j in range(n):
			ids.append(f.get_8())
		avatar_blocks.append({"faction_id": fid, "avatar_profile_ids": ids})
	out["avatars"] = avatar_blocks

	# [PatrolPaths]
	BinaryHelpers.read_string(f)
	var path_count := BinaryHelpers.read_i32(f)
	var paths := []
	for i in range(path_count):
		var p := {}
		p["id"] = BinaryHelpers.read_i32(f)
		p["sector_id"] = BinaryHelpers.read_i32(f)
		p["is_loop"] = BinaryHelpers.read_bool(f)
		var node_count := BinaryHelpers.read_i32(f)
		var nodes := []
		for j in range(node_count):
			var node := {}
			node["sector_position"] = BinaryHelpers.read_vec3(f)
			node["order"] = BinaryHelpers.read_i32(f)
			nodes.append(node)
		p["nodes"] = nodes
		paths.append(p)
	out["patrol_paths"] = paths

	# [FactionRelations]
	BinaryHelpers.read_string(f)
	var rel_blocks := []
	var rel_count := BinaryHelpers.read_i32(f)
	for i in range(rel_count):
		var fid := BinaryHelpers.read_i32(f)
		var item_count := BinaryHelpers.read_i32(f)
		var items := []
		for j in range(item_count):
			var it := {}
			it["other_faction_id"] = BinaryHelpers.read_i32(f)
			it["permanent_peace"] = BinaryHelpers.read_bool(f)
			it["restrict_hostility_timeout"] = BinaryHelpers.read_bool(f)
			it["neutrality"] = BinaryHelpers.read_i32(f)
			it["hostility_end_time"] = f.get_double()
			items.append(it)
		rel_blocks.append({"faction_id": fid, "relations": items})
	out["faction_relations"] = rel_blocks

	# [FactionDamage]
	BinaryHelpers.read_string(f)
	var dmg_blocks := []
	var dmg_count := BinaryHelpers.read_i32(f)
	for i in range(dmg_count):
		var fid := BinaryHelpers.read_i32(f)
		var n := BinaryHelpers.read_i32(f)
		var items := []
		for j in range(n):
			var other := BinaryHelpers.read_i32(f)
			var dmg := f.get_float()
			items.append({"other_faction_id": other, "damage": dmg})
		dmg_blocks.append({"faction_id": fid, "damage": items})
	out["faction_damage"] = dmg_blocks

	# [FactionOpinions]
	BinaryHelpers.read_string(f)
	var op_count := BinaryHelpers.read_i32(f)
	var ops := []
	for i in range(op_count):
		var o := {}
		o["faction_id"] = BinaryHelpers.read_i32(f)
		o["other_faction_id"] = BinaryHelpers.read_i32(f)
		o["opinion"] = f.get_float()
		o["created_time"] = BinaryHelpers.read_i32(f)
		ops.append(o)
	out["faction_opinions"] = ops

	out["end_offset"] = f.get_position()
	f.close()
	return out
