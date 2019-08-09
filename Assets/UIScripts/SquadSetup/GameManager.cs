using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml.Serialization;

public class GameManager
{
    private static Dictionary<string, List<string>> Rewards;
    public static Dictionary<string, List<string>> rewards { get => Rewards; set => Rewards = value; }
    private static Dictionary<string, SquadManager.UnitStats> allChars;
    public static Dictionary<string, SquadManager.UnitStats> AllChars { get => allChars; private set => allChars = value; }

    public static List<string> BinomList;
    public static List<int> BinomLevelList;
    public static int currentBinom;
    public static List<SquadManager.UnitStats> enemySquad;
    public static List<SquadManager.UnitStats> userSquad;
    public static List<SquadManager.UnitStats> availableUnits;
    public static int gold;

    static GameManager()
    {
        BinomList = new List<string>() { "Desert", "Forest", "Underwater", "SkyPick" };
        BinomLevelList = new List<int>();
        AllChars = new Dictionary<string, SquadManager.UnitStats>();
            List<SquadManager.UnitStats> allCharsList = ParseSquadFile(Resources.Load<TextAsset>("characters").text);
            foreach (var unit in allCharsList)
            {
                AllChars.Add(unit.fileName, unit);
            }
        Rewards = new Dictionary<string, List<string>>();
        userSquad = new List<SquadManager.UnitStats>();
        enemySquad = new List<SquadManager.UnitStats>();
        availableUnits = new List<SquadManager.UnitStats>();
        Rewards["Desert"] = new List<string>() { "Horde", "Radulac" };
        Rewards["Underwater"] = new List<string>() { "Overmind", "Skoll" };
        Rewards["Forest"] = new List<string>() { "MechaSnake", "Demeres" };
        Rewards["SkyPick"] = new List<string>() { "Remment", "Kitsune" };
        currentBinom = 0;

        if (string.IsNullOrEmpty(PlayerPrefs.GetString("gameData")))
        {
            Save firstItems = new Save
            {
                gold = 1000,
                levels = new int[4] { 1, 1, 1, 1 },
                squad = new string[] { },
                availableUnits = new string[] { "ColossalCrow", "ChampionGoblin", "BladeMaster", "Grunt", "MknightGoldnharl", "BloodMage" }
            };
            Serialize(firstItems, "gameData");
        }
        Save actualSave = Deserialize<Save>("gameData");
        gold = actualSave.gold;
        foreach (var unit in actualSave.squad)
        {
            SquadManager.UnitStats x = allChars[unit];
            x.ChangeTeam(1);
            userSquad.Add(x);
        }
        foreach (var unit in actualSave.availableUnits)
        {
            availableUnits.Add(allChars[unit]);
        }
        foreach (var binomLevel in actualSave.levels)
        {
            BinomLevelList.Add(binomLevel);
        }
        if (Resources.Load<TextAsset>("Squads/" + BinomList[currentBinom] + "/Lvl" + BinomLevelList[currentBinom]) != null)
        {
            enemySquad = ParseSquadFile(Resources.Load<TextAsset>("Squads/" + BinomList[currentBinom] + "/Lvl" + BinomLevelList[currentBinom]).text);
        }
        else
        {
            BinomLevelList[currentBinom]--;
            enemySquad = ParseSquadFile(Resources.Load<TextAsset>("Squads/" + BinomList[currentBinom] + "/Lvl" + BinomLevelList[currentBinom]).text);
        }
    }

    public static List<SquadManager.UnitStats> ParseSquadFile(string s)
    {
        List<SquadManager.UnitStats> squad = new List<SquadManager.UnitStats>();
        foreach (var row in s.Split('\n'))
        {
            if (!string.IsNullOrEmpty(row))
            {
                SquadManager.UnitStats newUnit = new SquadManager.UnitStats();
                int paramId = 0;
                foreach (var param in row.Split('\t'))
                {
                    switch (paramId)
                    {
                        case 0:
                            newUnit.fileName = param;
                            break;
                        case 1:
                            newUnit.lvl = int.Parse(param);
                            newUnit.cost = int.Parse(param)*200;
                            break;
                        case 2:
                            newUnit.amountofdice = int.Parse(param);
                            break;
                        case 3:
                            newUnit.dicetype = int.Parse(param);
                            break;
                        case 4:
                            newUnit.movetype = int.Parse(param);
                            break;
                        case 5:
                            newUnit.attacktype = int.Parse(param);
                            break;
                        case 7:
                            newUnit.hp = int.Parse(param);
                            break;
                        case 8:
                            Enum.TryParse<SkillPatterns.skill>(param, out newUnit.skilltype);
                            Enum.TryParse<AttackPatterns.attackEffect>(param, out newUnit.attackEffect);
                            break;
                        case 9:
                            if (!string.IsNullOrEmpty(param))
                            {
                                newUnit.position = int.Parse(param);
                            }
                            break;
                        default:
                            break;
                    }
                    paramId++;
                }
                newUnit.team = -1;
                squad.Add(newUnit);
            }
        }
        return squad;
    }

    public static void ChangeBinom(int value)
    {
        if (currentBinom+value>=0&&currentBinom+value<BinomList.Count)
        {
            currentBinom += value;
        }
        else if (currentBinom + value < 0)
        {
            currentBinom = BinomList.Count - 1;
        }
        else if (currentBinom + value >= BinomList.Count)
        {
            currentBinom = 0;
        }
        GameObject.Find("BinomImage").GetComponent<RawImage>().texture = Resources.Load<Texture>("Binoms/"+BinomList[currentBinom]);
        GameObject.Find("BinomName").GetComponent<TMPro.TextMeshProUGUI>().text = BinomList[currentBinom];     
        GameObject.Find("Level").GetComponent<TMPro.TextMeshProUGUI>().text = BinomLevelList[currentBinom].ToString();
        enemySquad = ParseSquadFile(Resources.Load<TextAsset>("Squads/" + BinomList[currentBinom] + "/Lvl" + BinomLevelList[currentBinom]).text);
    }

    public class Save
    {
        public int gold;
        public int[] levels;
        public string[] squad;
        public string[] availableUnits;
    }

    public static void Serialize<T>(T toSerialize, string name)
    {
        PlayerPrefs.SetString(name, JsonUtility.ToJson(toSerialize));        
    }

    public static T Deserialize<T>(string name)
    {
        string jsonData = PlayerPrefs.GetString(name);
        return JsonUtility.FromJson<T>(jsonData);
    }
}