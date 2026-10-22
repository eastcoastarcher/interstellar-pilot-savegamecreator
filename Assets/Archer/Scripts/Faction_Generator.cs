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

using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using Pixelfactor.IP.Common.Factions;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GenerateFaction : EditorWindow
{
    private GameObject selectedGameObject;
    private GameObject sectorFolder;
    private int GenerateFactionAmount = 1;
    private GUIContent selectedGameObjectTooltip = new GUIContent("Faction Root Folder", "Select a GameObject in the scene that contains factions objects to generate into. By default, it was Factions.");
    private GUIContent GenerateFactionAmountTooltip = new GUIContent("Generate Amount", "Specify the number of factions to generate.");
    private GUIContent sectorFolderTooltip = new GUIContent("Sector Root Folder.", "Select a GameObject in the scene that contains sector objects, this will be use to determining homesector(not correctly implemented yet). By default, it was Sectors.");
    private string customText = "Made by Commander Soul. My discord is Soul Harvester#5994";

    [MenuItem("Window/Generate Faction")]
    public static void ShowWindow()
    {
        GetWindow<GenerateFaction>("Generate Faction");
    }


    private void OnGUI()
    {
        EditorGUILayout.HelpBox("Select a game object to generate factions and randomize their components.", MessageType.Info);
        selectedGameObject = (GameObject)EditorGUILayout.ObjectField(selectedGameObjectTooltip, selectedGameObject, typeof(GameObject), true);
        sectorFolder = (GameObject)EditorGUILayout.ObjectField(sectorFolderTooltip, sectorFolder, typeof(GameObject), true);
        GenerateFactionAmount = EditorGUILayout.IntField(GenerateFactionAmountTooltip, GenerateFactionAmount);

        EditorGUI.BeginChangeCheck(); // Start tracking GUI changes

        // Generate Faction button with tooltip
        GUIContent generateFactionButtonContent = new GUIContent("Generate Faction", "Generate factions");
        Rect generateFactionButtonRect = GUILayoutUtility.GetRect(generateFactionButtonContent, GUI.skin.button);
        bool generateFactionButtonClicked = GUI.Button(generateFactionButtonRect, generateFactionButtonContent);
        if (generateFactionButtonRect.Contains(Event.current.mousePosition))
        {
            GUI.tooltip = "Generate factions and modify components";
        }

        // Delete All Generated Factions button with tooltip
        GUIContent deleteFactionButtonContent = new GUIContent("Delete All Factions", "Delete all generated factions, this will basically clearing inside of faction root folder");
        Rect deleteFactionButtonRect = GUILayoutUtility.GetRect(deleteFactionButtonContent, GUI.skin.button);
        bool deleteFactionButtonClicked = GUI.Button(deleteFactionButtonRect, deleteFactionButtonContent);
        if (deleteFactionButtonRect.Contains(Event.current.mousePosition))
        {
            GUI.tooltip = "Delete all generated factions";
        }

        if (EditorGUI.EndChangeCheck()) // Check if any GUI changes occurred
        {
            if (generateFactionButtonClicked)
            {
                AddFactionsAndModifyComponents();
            }
            else if (deleteFactionButtonClicked)
            {
                DeleteGeneratedFactions();
            }
        }

        GUILayout.Label(customText);
    }

    private void AddFactionsAndModifyComponents()
    {
        if (selectedGameObject == null || sectorFolder == null)
        {
            Debug.LogError("No game object or sector folder selected.");
            return;
        }

        Transform[] potentialHomeSectors = sectorFolder.transform.GetComponentsInChildren<Transform>(false);

        // Filter the potential home sectors to only include those with the EditorSector script
        potentialHomeSectors = potentialHomeSectors
            .Where(sector => sector != sectorFolder.transform && sector.GetComponent<EditorSector>() != null)
            .ToArray();
        // Create a list of excluded FactionTypes, None and Player is obvious
        // StationBuilder is kinda ambigous without Pixelfactor Explanation
        // I assume its a minor faction or freelancer, both of which doesn't have a suitable name yet in NameLibrary.cs
        // Update, Security now causing file to stuck on load so I removing it
        List<FactionType> excludedTypes = new List<FactionType>
        {
        FactionType.Player,
        FactionType.Security,
        FactionType.StationBuilder,
        FactionType.None
        };

        
        System.Random rng = new System.Random();
        

        // Loop for generator
        for (int i = 0; i < GenerateFactionAmount; i++)
        {
            // Create a new Faction game object
            GameObject factionObject = new GameObject("Faction");
            factionObject.transform.parent = selectedGameObject.transform;

            // Add the EditorFaction component to the Faction game object
            EditorFaction editorFaction = factionObject.AddComponent<EditorFaction>();
            editorFaction.Id = Random.Range(0, 1000000);

            // Generate custom name using front and rear names from the NameLibrary class
            string frontName = NameLibrary.frontNames[Random.Range(0, NameLibrary.frontNames.Length)];
            string rearName = NameLibrary.rearNames[Random.Range(0, NameLibrary.rearNames.Length)];
            editorFaction.CustomName = frontName + " " + rearName;

            // Get virtue value from front name and rear name
            int frontVirtue = NameLibrary.GetFactionVirtue(frontName);
            int rearVirtue = NameLibrary.GetFactionVirtue(rearName);

            // Calculate the total virtue
            float totalVirtue = (frontVirtue + rearVirtue);

            // Get aggression value from front name and rear name
            int frontAggression = NameLibrary.GetFactionAggressiveness(frontName);
            int rearAggression = NameLibrary.GetFactionAggressiveness(rearName);
            int totalAggression = frontAggression + rearAggression;

            // Convert the value from their range respective dictionary to Range(0, 1) and declare it to respective variable
            float virtue = Mathf.InverseLerp(-5f, 5f, totalVirtue);

            // Check if the rear name is one of the specific names where virtue should be 0 since this names obviously for evil faction
            if (rearName == "Bandit" || rearName == "Rebellion" || rearName == "Cult" || rearName == "Pirate" || rearName == "Raider" || rearName == "Marauder")
            {
                virtue = 0f;
                editorFaction.FactionType = FactionType.Bandit;
            }
            float aggression = Mathf.InverseLerp(-4f, 5f, totalAggression);

            // Generate custom short name using the same front name because this is how in-game naming system
            editorFaction.CustomShortName = frontName;

            // Choose a random home sector from the potentialHomeSectors list
            int randomIndex = Random.Range(0, potentialHomeSectors.Length);
            Transform randomHomeSector = potentialHomeSectors[randomIndex];

            // Assign the selected home sector to the editorFaction
            editorFaction.HomeSectorTransform = randomHomeSector;

            editorFaction.Description = "A Faction"; // Maybe implement ChatGPT to create random description based on faction stats?

            // Get virtue and aggression variable
            editorFaction.Aggression = aggression;
            editorFaction.Virtue = virtue;

            // Check the virtue value and set the faction type accordingly
            if (virtue < 0.05f) // Virtue that lower or equal to 0.05 will have chances of faction type as bandit, outlaw, or other faction
            {
                float randomValue = Random.value;
                if (randomValue < 0.2f) // 20% chances for other faction type beside bandit and outlaw
                {
                    // Choose a random valid FactionType excluding Bandit and Outlaw
                    List<FactionType> excludedTypesModified = new List<FactionType>(excludedTypes);
                    excludedTypesModified.Remove(FactionType.Bandit);
                    excludedTypesModified.Remove(FactionType.Outlaw);
                    // Loop until a valid faction type (not Bandit or Outlaw) is chosen
                    do
                    {
                        (FactionType factionType, int credits, bool isCivilian) = GetRandomFactionType(excludedTypesModified);
                        // Auto Setup
                        editorFaction.FactionType = factionType; // Faction types
                        editorFaction.Credits = credits; // Amount of credits
                        editorFaction.IsCivilian = isCivilian;
                    } while (editorFaction.FactionType == FactionType.Bandit || editorFaction.FactionType == FactionType.Outlaw);
                }
                else if (randomValue < 0.3f) // 10% (range is 0.2 to 0.3 or 0.3 - 0.2 to be exact) for outlaw faction type choosen
                {
                    editorFaction.FactionType = FactionType.Outlaw;
                    editorFaction.Credits = Random.Range(0, 200000);
                }
                else // the rest which is 1 - (0.2 + 0.1) is 0.7 or 70%
                {
                    editorFaction.FactionType = FactionType.Bandit;
                    editorFaction.Credits = Random.Range(50000, 1000000);
                }
            }
            else if (virtue <= 0.4f) // Virtue that lower or equal to 0.4 while higher than 0.2 will have chances of faction type as outlaw or other faction
            {
                float randomValue = Random.value;
                if (randomValue < 0.3f) // 30% chances for other faction type beside bandit and outlaw
                {
                    // Choose a random valid FactionType excluding Bandit and Outlaw
                    List<FactionType> excludedTypesModified = new List<FactionType>(excludedTypes);
                    excludedTypesModified.Remove(FactionType.Bandit);
                    excludedTypesModified.Remove(FactionType.Outlaw);
                    // Loop until a valid faction type (not Bandit or Outlaw) is chosen
                    do
                    {
                        (FactionType factionType, int credits, bool isCivilian) = GetRandomFactionType(excludedTypesModified);
                        // Auto Setup
                        editorFaction.FactionType = factionType; // Faction types
                        editorFaction.Credits = credits; // Amount of credits
                        editorFaction.IsCivilian = isCivilian;
                    } while (editorFaction.FactionType == FactionType.Bandit || editorFaction.FactionType == FactionType.Outlaw);
                }
                else // The rest which is 1 - 0.3 is 0.7 or 70%
                {
                    editorFaction.FactionType = FactionType.Outlaw;
                    editorFaction.Credits = Random.Range(0, 200000);
                }
            }
            else // Virtue that above 0.4 will generate other faction type beside bandit and outlaw
            {
                // Choose a random valid FactionType excluding Bandit and Outlaw
                List<FactionType> excludedTypesModified = new List<FactionType>(excludedTypes);
                excludedTypesModified.Remove(FactionType.Bandit);
                excludedTypesModified.Remove(FactionType.Outlaw);
                // Loop until a valid faction type (not Bandit or Outlaw) is chosen
                do
                {
                    (FactionType factionType, int credits, bool isCivilian) = GetRandomFactionType(excludedTypesModified);
                    // Auto Setup
                    editorFaction.FactionType = factionType; // Faction types
                    editorFaction.Credits = credits; // Amount of credits
                    editorFaction.IsCivilian = isCivilian;
                } while (editorFaction.FactionType == FactionType.Bandit || editorFaction.FactionType == FactionType.Outlaw);
            }

            // Weirdly enough, all default non player faction always have full trade Efficiency
            editorFaction.TradeEfficiency = 1;

            editorFaction.Greed = Random.Range(0f, 1f);
            editorFaction.Cooperation = Random.Range(0f, 1f);
            editorFaction.DynamicRelations = true;
            editorFaction.ShowJobBoards = true;
            editorFaction.CreateJobs = true;
            editorFaction.MinNpcCombatEfficiency = Random.Range(0f, 1f);
            editorFaction.MaxNpcCombatEfficiency = Random.Range(editorFaction.MinNpcCombatEfficiency, 1f); // Just to make sure MaxNpcCombatEfficiency always higher
            editorFaction.AdditionalRpProvision = Random.Range(0, 10000);
            if (virtue <= 0.45f)
            {
                editorFaction.TradeIllegalGoods = true;
            }
            else
            {
                editorFaction.TradeIllegalGoods = false;
            }

            // This value are fixed because there is no explanation, no noticeable effect and in samplescenes this is the default value
            editorFaction.GeneratedNameId = -1;
            editorFaction.GeneratedSuffixId = -1;
            editorFaction.SpawnTime = 0.0;
            editorFaction.HighestEverNetWorth = 0L;
            editorFaction.PreferredFormationId = -1;
            editorFaction.PilotRankingSystemId = -1;
            editorFaction.DestroyWhenNoUnits = true;
            editorFaction.RequisitionPointMultiplier = 1;


            editorFaction.EditorColor = Color.grey;

            // Create a Faction Setting game object as a child of the Faction game object
            GameObject factionSettingObject = new GameObject("Faction Setting");
            factionSettingObject.transform.parent = factionObject.transform;

            // Add the EditorFactionCustomSetting component to the Faction Setting game object
            EditorFactionCustomSettings factionSetting = factionSettingObject.AddComponent<EditorFactionCustomSettings>();

            // Modify the Faction Setting component properties
            factionSetting.PreferSingleShip = false; //Because NameLibrary.cs doesn't contain any name for freelance/Individual name

            // Default faction Setting value because... AI
            factionSetting.RepairShips = true;
            factionSetting.UpgradeShips = true;
            factionSetting.MinFleetUnitCount = 1;
            factionSetting.MaxFleetUnitCount = 8;
            factionSetting.HostileWithAll = false;
            factionSetting.AllowOtherFactionToUseDocks = true;
            factionSetting.IgnoreStationCreditsReserve = false;
            factionSetting.DailyIncome = 0; // Artificial credit bonus doesn't actually used in non player faction

            factionSetting.RepairMinHullDamage = Random.Range(0f, 1f);
            factionSetting.RepairMinCreditsBeforeRepair = Random.Range(0, 10000);
            factionSetting.PreferenceToPlaceBounty = Random.Range(0f, 1f);
            factionSetting.LargeShipPreference = Random.Range(0f, 1f);
            factionSetting.OffensiveStance = Random.Range(0f, 1f);
            factionSetting.PreferenceToBuildTurrets = Random.Range(0f, 1f);
            factionSetting.PreferenceToBuildStations = Random.Range(0f, 1f);

            Debug.Log("Faction added and components modified.");
        }
    }
    // Method to choose a random valid FactionType excluding the specified types and generate corresponding credits
    private (FactionType, int, bool) GetRandomFactionType(List<FactionType> excludedTypes)
    {
        System.Array factionTypes = System.Enum.GetValues(typeof(FactionType));

        // Create a list of valid FactionTypes by excluding the specified types
        List<FactionType> validTypes = new List<FactionType>();
        foreach (FactionType factionType in factionTypes)
        {
            if (!excludedTypes.Contains(factionType))
            {
                validTypes.Add(factionType);
            }
        }

        // Get a random index within the range of valid FactionTypes
        int randomIndex = UnityEngine.Random.Range(0, validTypes.Count);

        // Get the randomly chosen valid FactionType
        FactionType randomFactionType = validTypes[randomIndex];

        // Set the credits and other properties based on the chosen FactionType
        int credits;
        bool isCivilian; // There is an explanation that isCivilian is not used if the faction type is not generic. I just know it after i finish this code and Im too lazy to remove it
        switch (randomFactionType)
        {
            case FactionType.Generic:
                credits = Random.Range(200000, 1000000);
                isCivilian = false;
                break;
            case FactionType.Trader:
                credits = Random.Range(100000, 500000);
                isCivilian = false;
                break;
            case FactionType.Scavenger:
                credits = Random.Range(0, 50000);
                isCivilian = false;

                break;
            case FactionType.Miner:
                credits = Random.Range(100000, 250000);
                isCivilian = false;
                break;
            case FactionType.BountyHunter:
                credits = Random.Range(500000, 700000);
                isCivilian = false;
                break;
            case FactionType.Empire:
                credits = Random.Range(5000000, 8000000);
                isCivilian = false;
                break;
            case FactionType.PassengerTransport:
                credits = Random.Range(50000, 200000);
                isCivilian = false;
                break;
            case FactionType.Explorer:
                credits = Random.Range(500000, 800000);
                isCivilian = false;
                break;
            case FactionType.EquipmentDealer:
                credits = Random.Range(200000, 500000);
                isCivilian = false;
                break;
            case FactionType.Mercenary:
                credits = Random.Range(500000, 1000000);
                isCivilian = false;
                break;
            case FactionType.Security:
                credits = Random.Range(500000, 1000000);
                isCivilian = false;
                break;
            case FactionType.Bar:
                credits = Random.Range(50000, 150000);
                isCivilian = false;
                break;
            default:
                credits = Random.Range(50000, 500000);
                isCivilian = false;
                break;
        }

        // Return the randomly chosen valid FactionType, credits, and isCivilian as a tuple
        return (randomFactionType, credits, isCivilian);
    }
    private void DeleteGeneratedFactions()
    {
        if (selectedGameObject == null)
        {
            Debug.LogError("No game object selected.");
            return;
        }

        // Get all generated faction objects under the selected game object
        EditorFaction[] generatedFactions = selectedGameObject.GetComponentsInChildren<EditorFaction>().Where(f => f.Id >= 0).ToArray();

        foreach (EditorFaction faction in generatedFactions)
        {
            // Destroy the faction object and its children
            DestroyImmediate(faction.gameObject);
        }

        Debug.Log("Deleted all generated factions.");
    }
}