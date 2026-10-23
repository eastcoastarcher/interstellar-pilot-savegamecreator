extends Control

const SAVE_PATH := "res://testdata/BountyUniverse.dat"

@onready var header_label: Label = $HeaderLabel

func _ready() -> void:
	var h := HeaderReader.read_header(SAVE_PATH)
	if h.is_empty():
		header_label.text = "Could not open save:\n" + SAVE_PATH
		return
	var heads := SectionIndex.find_headings(SAVE_PATH)
	var secs := SectorReader.read_sectors(SAVE_PATH)
	var facs := FactionReader.read_factions(SAVE_PATH)
	var extras := FactionExtrasReader.read_all(SAVE_PATH)
	var units := UnitReader.read_units(SAVE_PATH)
	var comp := UnitComponentsReader.read_all(SAVE_PATH)
	header_label.text = (
		"Version: " + ".".join(h["version"].map(func(n): return str(n))) + "\n"
		+ "Created: " + ".".join(h["created_version"].map(func(n): return str(n))) + "\n"
		+ "Autosave: " + ("Yes" if h["is_autosave"] else "No") + "\n"
		+ "Timestamp: " + h["timestamp"] + "\n"
		+ "Header ends at byte: " + str(h["header_end_offset"]) + "\n"
		+ "Headings found: " + str(heads.size()) + "\n"
		+ "Sectors: " + str(secs["count"]) + "  (ends at byte " + str(secs["end_offset"]) + ")\n"
		+ "Factions: " + str(facs["count"]) + "  (ends at byte " + str(facs["end_offset"]) + ")\n"
		+ "Faction extras end at byte: " + str(extras["end_offset"]) + "\n"
		+ "Units: " + str(units["count"]) + "  (ends at byte " + str(units["end_offset"]) + ")\n"
		+ "Unit components end at byte: " + str(comp["end_offset"])
	)
	print("HEADINGS COUNT: ", heads.size())
	print("SECTORS COUNT: ", secs["count"], " END: ", secs["end_offset"])
	print("FACTIONS COUNT: ", facs["count"], " END: ", facs["end_offset"])
	print("OPINIONS: ", extras["faction_opinions"].size(), " EXTRAS END: ", extras["end_offset"])
	print("UNITS COUNT: ", units["count"], " END: ", units["end_offset"])
	print("COMPONENTS END: ", comp["end_offset"])
	print("COMPONENT COUNTS: radius=", comp["radius"].size(), " names=", comp["names"].size(), " components=", comp["components"].size(), " constructing=", comp["constructing"].size(), " damage=", comp["damage_counter"].size(), " modded=", comp["modded"].size(), " capacitor=", comp["capacitor"].size(), " cloak=", comp["cloak"].size(), " powered_down=", comp["powered_down"].size(), " throttle=", comp["throttle"].size(), " cargo=", comp["cargo"].size(), " shields=", comp["shields"].size(), " component_health=", comp["component_health"].size(), " active=", comp["active"].size(), " unit_health=", comp["unit_health"].size())
