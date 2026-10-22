/*
Creative Commons Attribution-NonCommercial 4.0 International License
https://creativecommons.org/licenses/by-nc/4.0/

This work is licensed under the Creative Commons Attribution-NonCommercial 4.0 International License. To view a copy of this license, visit the link above.

You are free to:
  - Share: copy and redistribute the material in any medium or format
  - Adapt: remix, transform, and build upon the material

Under the following terms:
  - Attribution: You must give appropriate credit, provide a link to the license, and indicate if changes were made. You may do so in any reasonable manner, but not in any way that suggests the licensor endorses you or your use.
  - NonCommercial: You may not use the material for commercial purposes.

Note: Usage of this script is strictly limited to lawful and ethical purposes. It is prohibited to use this script for any malicious, harmful, or illegal activities.
*/
// Made by Commander Soul. My discord is Soul Harvester#5994

using System.Collections.Generic;
using UnityEngine;

public static class NameLibrary
{

    // Each dictionary have their own max and min value in the end of each part. don't follow those and it might break everything because the generator code use that limit.
    public static Dictionary<string, int> factionVirtue = new Dictionary<string, int>() // Virtue value for front name
    {
        { "Nova", 4 },
        { "Terran", 3 },
        { "Martian", 2 },
        { "Celestial", 5 },
        { "Shadow", -4 },
        { "Divine", 5 },
        { "Galactic", 3 },
        { "Abyssal", -5 },
        { "Radiant", 5 },
        { "Lunar", 3 },
        { "Stellar", 4 },
        { "Void", -3 },
        { "Ethereal", 5 },
        { "Infernal", -5 },
        { "Seraphic", 5 },
        { "Arcane", 3 },
        { "Phoenix", 4 },
        { "Valkyrie", 3 },
        { "Vanguard", 3 },
        { "Dreadnought", -4 },
        { "Sylvan", 3 },
        { "Thunder", 3 },
        { "Frost", 2 },
        { "Nebula", 3 },
        { "Crimson", 2 },
        { "Whisper", 2 },
        { "Serenity", 4 },
        { "Onyx", 2 },
        { "Solaris", 4 },
        { "Eclipse", -3 },
        { "Golden", 3 },
        { "Silver", 3 },
        { "Twilight", -2 },
        { "Ironclad", 3 },
        { "Pandora", -5 },
        { "Azure", 3 },
        { "Ragnarok", -5 },
        { "Evergreen", 3 },
        { "Sable", -3 },
        { "Fury", -4 },
        { "Harmony", 5 },
        { "Shade", -4 },
        { "Oracle", 5 },
        { "Verdant", 3 },
        { "Ebon", -2 },
        { "Cinder", -2 },
        { "Celeste", 3 },
        { "Obsidian", -3 },
        { "Valiant", 4 },
        { "Bane", -4 },
        { "Aurora", 4 },
        { "Sanguine", -5 },
        { "Pulse", 3 },
        { "Glimmer", 2 },
        { "Apex", 4 },
        { "Cerulean", 3 },
        { "Chaos", -5 },
        { "Unity", 4 },
        { "Dusk", -1 },
        { "Empyrean", 5 },
        { "Mystic", 3 },
        { "Hallowed", 5 },
        { "Sunder", -4 },
        { "Elysium", 5 },
        { "Inferno", -5 },
        { "Amethyst", 2 },
        { "Vesper", 3 },
        { "Sapphire", 3 },
        { "Vortex", -4 },
        { "Jade", 2 },
        { "Platinum", 3 },
        { "Solstice", 4 },
        { "Rune", 3 },
        { "Ember", 1 },
        { "Zephyr", 3 },
        { "Astral", 4 },
        { "Penumbral", -3 },
        { "Epoch", 4 },
        { "Illusion", 2 },
        { "Eternal", 5 },
        { "Nether", -5 },
        { "Oblivion", -5 },
        { "Zeal", 4 },
        { "Venom", -4 },
        { "Whirlwind", 3 },
        { "Honor", 5 },
        { "Wrath", -5 },
        { "Nimbus", 3 },
        { "Warden", 4 },
        { "Majestic", 5 },
        { "Wisp", 3 },
        { "Frostbite", -3 },
        { "Gloom", -2 },
        { "Ignite", -3 },
        { "Halcyon", 4 },
        { "Umbral", -2 },
        { "Umber", -2 },
        { "Resolute", 5 },
        { "Covenant", 4 },
        { "Morose", -3 },
        { "Vex", -3 },
        { "Seraph", 4 },
        { "Opal", 2 },
        { "Harbinger", -5 },
        { "Tranquil", 3 },
        { "Gilded", 4 },
        { "Silent", 3 },
        { "Aurelia", 3 },
        { "Viper", -4 },
        { "Nyx", -2 }
        // Add more entries here...
        // Use format ("Front Name", Virtue) Virtue determine by how evil or good the faction that use this name as it Front name
        // Higher = More toward good, lower = More toward evil
        // the value right now is -5 min and 5 max
        // It will sum the value between front name and rear name, the value get remmaped into range of 0 as min and 1 as max which is virtue value
    };


    public static Dictionary<string, int> rearFactionVirtue = new Dictionary<string, int>() // Virtue value for rear name
    {
        { "Legion", -4 },
        { "Consortium", 3 },
        { "Guild", 2 },
        { "Conglomerate", 1 },
        { "Cult", -5 },
        { "Clan", 2 },
        { "Circle", 1 },
        { "Union", 3 },
        { "Syndicate", -3 },
        { "Alliance", 5 },
        { "Association", 3 },
        { "Covenant", 4 },
        { "Corporation", 1 },
        { "Assembly", 2 },
        { "Society", 5 },
        { "Confederacy", 3 },
        { "Federation", 4 },
        { "Congregation", 2 },
        { "Coalition", 3 },
        { "Collective", 4 },
        { "Confederation", 2 },
        { "Republic", 3 },
        { "Dominion", 0 },
        { "Order", 2 },
        { "Network", 1 },
        { "Hierarchy", 2 },
        { "Bandit", -5 },
        { "Cohort", -2 },
        { "Empire", 0 },
        { "Rebellion", -5 },
        { "Club", -1 },
        { "Army", -2 },
        {"Pirate", -5},
        {"Raider", -5},
        {"Marauder", -5}
        // Add more entries here...
        // Use format ("Rear Name", Virtue) Virtue determine by how evil or good the faction that use this name as it Rear name
        // Higher = More toward good, lower = More toward evil
        // the value right now is -5 min and 5 max  
        // It will sum the value between front name and rear name, the value get remmaped into range of 0 as min and 1 as max which is virtue value
    };


    public static Dictionary<string, int> factionAggressiveness = new Dictionary<string, int>() // Faction aggressiveness
    {
        { "Nova", 3 },
        { "Terran", 2 },
        { "Martian", 1 },
        { "Celestial", 2 },
        { "Shadow", 4 },
        { "Divine", -3 },
        { "Galactic", 1 },
        { "Abyssal", 5 },
        { "Radiant", -3 },
        { "Lunar", 2 },
        { "Stellar", 3 },
        { "Void", 4 },
        { "Ethereal", -2 },
        { "Infernal", 5 },
        { "Seraphic", -2 },
        { "Arcane", 1 },
        { "Phoenix", 4 },
        { "Valkyrie", 3 },
        { "Vanguard", 3 },
        { "Dreadnought", 4 },
        { "Sylvan", 2 },
        { "Thunder", 4 },
        { "Frost", 1 },
        { "Nebula", 2 },
        { "Crimson", 3 },
        { "Whisper", -2 },
        { "Serenity", -3 },
        { "Onyx", 2 },
        { "Solaris", -3 },
        { "Eclipse", 4 },
        { "Golden", 1 },
        { "Silver", 2 },
        { "Twilight", -4 },
        { "Ironclad", 3 },
        { "Pandora", -4 },
        { "Azure", 2 },
        { "Ragnarok", 5 },
        { "Evergreen", -1 },
        { "Sable", -3 },
        { "Fury", 5 },
        { "Harmony", -2 },
        { "Shade", 4 },
        { "Oracle", -3 },
        { "Verdant", -1 },
        { "Ebon", 4 },
        { "Cinder", 5 },
        { "Celeste", -2 },
        { "Obsidian", 4 },
        { "Valiant", 2 },
        { "Bane", 5 },
        { "Aurora", -2 },
        { "Sanguine", 4 },
        { "Pulse", 3 },
        { "Glimmer", 2 },
        { "Apex", 3 },
        { "Cerulean", 2 },
        { "Chaos", 5 },
        { "Unity", -2 },
        { "Dusk", 3 },
        { "Empyrean", -3 },
        { "Mystic", 2 },
        { "Hallowed", -4 },
        { "Sunder", 5 },
        { "Elysium", -3 },
        { "Inferno", 5 },
        { "Amethyst", 1 },
        { "Vesper", 2 },
        { "Sapphire", 2 },
        { "Vortex", 4 },
        { "Jade", 1 },
        { "Platinum", 2 },
        { "Solstice", 3 },
        { "Rune", 2 },
        { "Ember", 4 },
        { "Zephyr", 2 },
        { "Astral", 3 },
        { "Penumbral", 4 },
        { "Epoch", 2 },
        { "Illusion", 1 },
        { "Eternal", -3 },
        { "Nether", 5 },
        { "Oblivion", 5 },
        { "Zeal", 2 },
        { "Venom", 4 },
        { "Whirlwind", 3 },
        { "Honor", -3 },
        { "Wrath", 5 },
        { "Nimbus", 3 },
        { "Warden", 3 },
        { "Majestic", 2 },
        { "Wisp", 3 },
        { "Frostbite", 2 },
        { "Gloom", -2 },
        { "Ignite", 2 },
        { "Halcyon", -3 },
        { "Umbral", 2 },
        { "Umber", 2 },
        { "Resolute", 2 },
        { "Covenant", 3 },
        { "Morose", -2 },
        { "Vex", -2 },
        { "Seraph", 3 },
        { "Opal", 1 },
        { "Harbinger", 5 },
        { "Tranquil", -1 },
        { "Gilded", 3 },
        { "Silent", 2 },
        { "Aurelia", 2 },
        { "Viper", 4 },
        { "Nyx", 2 }
        // Add more entries here...
        // Use format ("Front Name", Aggression) Aggression determine by how aggressive the faction that use this name as it Front name
        // Higher = more aggressive
        // the value right now is -4 min and 5 max, Don't ask why
        // It will sum the value between front name and rear name the value get remmaped into range of 0 as min and 1 as max which is aggression value
    };


    public static Dictionary<string, int> rearFactionAggressiveness = new Dictionary<string, int>() // Rear name for aggressiveness
    {
        { "Legion", 2 },
        { "Consortium", 1 },
        { "Guild", 0 },
        { "Conglomerate", -1 },
        { "Cult", 1 },
        { "Clan", 0 },
        { "Circle", -1 },
        { "Union", 0 },
        { "Syndicate", 1 },
        { "Alliance", 0 },
        { "Association", -1 },
        { "Covenant", 1 },
        { "Corporation", -1 },
        { "Assembly", -2 },
        { "Society", -2 },
        { "Confederacy", 0 },
        { "Federation", -1 },
        { "Congregation", -2 },
        { "Coalition", -1 },
        { "Collective", -2 },
        { "Confederation", -1 },
        { "Republic", -2 },
        { "Dominion", -2 },
        { "Order", -1 },
        { "Network", -1 },
        { "Hierarchy", -2 },
        { "Bandit", 2 },
        { "Cohort", 0 },
        { "Empire", 2 },
        { "Rebellion", 2 },
        { "Club", -2 },
        { "Army", 2 },
        {"Pirate", 2},
        {"Raider", 2},
        {"Marauder", 2}
        // Add more entries here...
        // Use format ("Rear Name", Aggression) Aggression determine by how aggressive the faction that use this name as it rear name
        // Higher = more aggressive
        // the value right now is -2 min and 2 max
        // It will sum the value between front name and rear name the value get remmaped into range 0 as min and 1 as max which is aggression value
    };


    public static string[] frontNames = new string[] // Front name list
    {
            "Nova", "Terran", "Martian", "Celestial", "Shadow", "Divine", "Galactic", "Abyssal", "Radiant",
        "Lunar", "Stellar", "Void", "Ethereal", "Infernal", "Seraphic", "Arcane", "Phoenix", "Valkyrie",
        "Vanguard", "Dreadnought", "Sylvan", "Thunder", "Frost", "Nebula", "Crimson", "Whisper",
        "Serenity", "Onyx", "Solaris", "Eclipse", "Golden", "Silver", "Twilight", "Ironclad", "Pandora",
        "Azure", "Ragnarok", "Evergreen", "Sable", "Fury", "Harmony", "Shade", "Oracle", "Verdant",
        "Ebon", "Cinder", "Celeste", "Obsidian", "Valiant", "Bane", "Aurora", "Sanguine", "Pulse",
        "Glimmer", "Apex", "Cerulean", "Chaos", "Unity", "Dusk", "Empyrean", "Mystic", "Hallowed",
        "Sunder", "Elysium", "Inferno", "Amethyst", "Vesper", "Sapphire", "Vortex", "Jade", "Platinum",
        "Solstice", "Rune", "Ember", "Zephyr", "Astral", "Penumbral", "Epoch", "Illusion", "Eternal",
        "Nether", "Oblivion", "Zeal", "Venom", "Whirlwind", "Honor", "Wrath", "Nimbus", "Warden",
        "Majestic", "Wisp", "Frostbite", "Gloom", "Ignite", "Halcyon", "Umbral", "Umber", "Resolute",
        "Covenant", "Morose", "Vex", "Seraph", "Opal", "Harbinger", "Tranquil", "Gilded", "Silent",
        "Aurelia", "Viper", "Nyx"
            // Add more front names here...
    };


    public static string[] rearNames = new string[] // Rear name list
    {
        "Legion",
        "Consortium",
        "Guild",
        "Conglomerate",
        "Cult",
        "Clan",
        "Circle",
        "Union",
        "Syndicate",
        "Alliance",
        "Association",
        "Covenant",
        "Corporation",
        "Assembly",
        "Society",
        "Confederacy",
        "Federation",
        "Congregation",
        "Coalition",
        "Collective",
        "Confederation",
        "Republic",
        "Dominion",
        "Order",
        "Network",
        "Hierarchy",
        "Bandit",
        "Cohort",
        "Empire",
        "Rebellion",
        "Club",
        "Army",
        "Pirate",
        "Raider",
        "Marauder"
        // Add more rear names here...
    };

    public static int GetFactionVirtue(string factionName) // Creating Method to calculate Virtue
    {
        if (factionVirtue.ContainsKey(factionName))
        {
            return factionVirtue[factionName];
        }
        else if (rearFactionVirtue.ContainsKey(factionName))
        {
            return rearFactionVirtue[factionName];
        }
        else
        {
            return 0; // Default value if the faction name doesn't have value that assigned to it
        }
    }
    public static int GetFactionAggressiveness(string factionName) // Creating Method to calculate Aggressiveness
    {
        if (factionAggressiveness.ContainsKey(factionName))
        {
            return factionAggressiveness[factionName];
        }
        else if (rearFactionAggressiveness.ContainsKey(factionName))
        {
            return rearFactionAggressiveness[factionName];
        }
        else
        {
            return 0; // Default value if the faction name doesn't have value that assigned to it
        }
    }
}
