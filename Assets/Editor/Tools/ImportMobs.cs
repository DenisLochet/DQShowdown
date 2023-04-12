using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System.Collections.Generic;


[System.Serializable]
public class MonsterValue
{
    public string Name;
    public int Type;
    public int Size;
    public string Rank;
    public int PV;
    public int PM;
    public int ATK;
    public int DEF;
    public int AGI;
    public int WIS;
}

[System.Serializable]
public class AptitudeValue
{
    public string spellName;
    public string Type;
    public string Element;
    public string Size;
    public bool Multicible;
    public int Cout;
    public string Description;
    public string StatBonus;
    public int BonusValue;
}

[System.Serializable]
public class SetAptitudeValue
{
    public string SetName;
    public List<string> aptitudes;
}

public class monstersValue
{
    public List<MonsterValue> monsters;
}
public class Aptitudes
{
    public List<AptitudeValue> aptitudes;
}
public class SetAptitudes
{
    public List<SetAptitudeValue> setApt;
}
public class ImportMobs : EditorWindow
{
    public string path;
    public string JsonString;
    int selected = 0;
    public monstersValue monstersList = new monstersValue();


    [MenuItem("Assets/Import Monsters")]
    public static void ShowWindow()
    {
        GetWindow<ImportMobs>("Import Monsters");
    }
    void ImportMonters()
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("choose Json Files", GUILayout.MaxWidth(125));
        if (GUILayout.Button("Monsters List"))
        {
            path = EditorUtility.OpenFilePanel("files in .Json", "", "Json");
            Debug.Log(path);
        }
        GUILayout.EndHorizontal();
        GUILayout.Label("Path monsters Json", EditorStyles.label);
        GUILayout.TextArea(path);

        if (GUILayout.Button("Add Monsters"))
        {
            string json2 = File.ReadAllText(path);
            monstersValue values = JsonUtility.FromJson<monstersValue>(json2);

            MonstersData asset = CreateInstance<MonstersData>();

            Object[] sprites = AssetDatabase.LoadAllAssetsAtPath("Assets/Editor Default Resources/MonsterRessources.png");

            var allAptitudes = Resources.LoadAll("AptitudeResources", typeof(AptitudeData)).Cast<AptitudeData>().ToArray();
            AptitudeData aptDef = new AptitudeData();
            AptitudeData aptAtk = new AptitudeData();
            foreach (var item in allAptitudes)
            {
                if (item.spellname == "Defence")
                {
                    aptDef = item;
                }
                if (item.spellname == "Attaque")
                {
                    aptAtk = item;
                }
            }


            foreach (var monster in values.monsters)
            {
                asset = ScriptableObject.CreateInstance<MonstersData>();
                asset.monsterName = monster.Name;
                foreach (var sprite in sprites)
                {
                    if (sprite.name == asset.monsterName)
                    {
                        asset.sprite = (Sprite)sprite;
                        break;
                    }
                }
                if (asset.sprite == null)
                    Debug.LogError($"Sprite null pour {asset.monsterName}");
                asset.type = (MonstersData.Type)monster.Type;
                asset.size = (MonstersData.Size)monster.Size;
                asset.rank = monster.Rank;
                asset.stats.HP = monster.PV;
                asset.stats.MP = monster.PM;
                asset.stats.ATK = monster.ATK;
                asset.stats.DEF = monster.DEF;
                asset.stats.AGI = monster.AGI;
                asset.stats.WIS = monster.WIS;
                asset.aptAttaque = aptAtk;
                asset.aptDefence = aptDef;
                AssetDatabase.CreateAsset(asset, "Assets/Resources/MonsterResources/" + monster.Name + ".asset");
                AssetDatabase.SaveAssets();

            }
            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
    void ImportAptitude()
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("choose Json Files", GUILayout.MaxWidth(125));
        if (GUILayout.Button("Aptitude List"))
        {
            path = EditorUtility.OpenFilePanel("files in .Json", "", "Json");
            Debug.Log(path);
        }
        GUILayout.EndHorizontal();
        GUILayout.Label("Path Aptitude Json", EditorStyles.label);
        GUILayout.TextArea(path);

        if (GUILayout.Button("Add Aptitude"))
        {
            string json2 = File.ReadAllText(path);
            Aptitudes values = JsonUtility.FromJson<Aptitudes>(json2);

            AptitudeData asset = new AptitudeData();

            foreach (var apt in values.aptitudes)
            {
                asset = ScriptableObject.CreateInstance<AptitudeData>();
                asset.spellname = apt.spellName;
                switch (apt.Type)
                {
                    case ("Magique"):
                        asset.spellType = AptitudeData.SpellType.Magique;
                    break;
                    case ("Physique"):
                        asset.spellType = AptitudeData.SpellType.Physique;
                        break;
                    case ("Souffle"):
                        asset.spellType = AptitudeData.SpellType.Souffle;
                        break;
                    case ("Soutien"):
                        asset.spellType = AptitudeData.SpellType.Soutien;
                        break;
                    case ("Soin"):
                        asset.spellType = AptitudeData.SpellType.Soin;
                        break;
                    case ("Stat"):
                        asset.spellType = AptitudeData.SpellType.Stat;
                        break;
                    default:
                        break;
                }
                switch (apt.Element)
                {
                    case ("Neutre"):
                        asset.spellElem = AptitudeData.SpellElem.None;
                        break;
                    case ("Feu"):
                        asset.spellElem = AptitudeData.SpellElem.Feu;
                        break;
                    case ("Bang"):
                        asset.spellElem = AptitudeData.SpellElem.Bang;
                        break;
                    case ("Vent"):
                        asset.spellElem = AptitudeData.SpellElem.Vent;
                        break;
                    case ("Lumiere"):
                        asset.spellElem = AptitudeData.SpellElem.Lumiere;
                        break;
                    case ("Tenebre"):
                        asset.spellElem = AptitudeData.SpellElem.Tenebre;
                        break;
                    case ("Crame"):
                        asset.spellElem = AptitudeData.SpellElem.Crame;
                        break;
                    case ("Glace"):
                        asset.spellElem = AptitudeData.SpellElem.Glace;
                        break;
                    case ("Souffle feu"):
                        asset.spellElem = AptitudeData.SpellElem.SouffleFeu;
                        break;
                    case ("Souffle glace"):
                        asset.spellElem = AptitudeData.SpellElem.SouffleGlace;
                        break;
                    default:
                        break;
                }
                switch (apt.Size)
                {
                    case ("None"):
                        asset.sizeSpell = AptitudeData.SizeSpell.None;
                        break;
                    case ("Normal"):
                        asset.sizeSpell = AptitudeData.SizeSpell.Normal;
                        break;
                    case ("Super"):
                        asset.sizeSpell = AptitudeData.SizeSpell.Super;
                        break;
                    case ("Mega"):
                        asset.sizeSpell = AptitudeData.SizeSpell.Mega;
                        break;
                    case ("Giga"):
                        asset.sizeSpell = AptitudeData.SizeSpell.Giga;
                        break;
                    default:
                        break;
                }
                asset.isMulticible = apt.Multicible; 
                asset.cost = apt.Cout;
                asset.effectText = apt.Description;
                switch (apt.StatBonus)
                {
                    case ("None"):
                        asset.statBonus = AptitudeData.StatBonus.None;
                        break;
                    case ("HP"):
                        asset.statBonus = AptitudeData.StatBonus.HP;
                        break;
                    case ("MP"):
                        asset.statBonus = AptitudeData.StatBonus.MP;
                        break;
                    case ("ATK"):
                        asset.statBonus = AptitudeData.StatBonus.ATK;
                        break;
                    case ("DEF"):
                        asset.statBonus = AptitudeData.StatBonus.DEF;
                        break;
                    case ("AGI"):
                        asset.statBonus = AptitudeData.StatBonus.AGI;
                        break;
                    case ("WIS"):
                        asset.statBonus = AptitudeData.StatBonus.WIS;
                        break;
                    default:
                        break;
                }
                asset.bonusValue = apt.BonusValue;
                AssetDatabase.CreateAsset(asset, "Assets/Resources/AptitudeResources/" + apt.spellName + ".asset");
                AssetDatabase.SaveAssets();

            }
            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
    void ImportSetAptitude()
    {
        var allAptitudes = Resources.LoadAll("AptitudeResources", typeof(AptitudeData)).Cast<AptitudeData>().ToArray();
        if (allAptitudes.Length == 0)
        {
            GUILayout.Label("Load Aptitude before", EditorStyles.label);
        }
        else
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("choose Json Files", GUILayout.MaxWidth(125));
            if (GUILayout.Button("Set Aptitude List"))
            {
                path = EditorUtility.OpenFilePanel("files in .Json", "", "Json");
                Debug.Log(path);
            }
            GUILayout.EndHorizontal();
            GUILayout.Label("Path Set Aptitude Json", EditorStyles.label);
            GUILayout.TextArea(path);

            if (GUILayout.Button("Add Set Aptitude"))
            {
                string json2 = File.ReadAllText(path);
                SetAptitudes values = JsonUtility.FromJson<SetAptitudes>(json2);

                SetAptitudeData asset = new SetAptitudeData();

                foreach (var apt in values.setApt)
                {
                    asset = ScriptableObject.CreateInstance<SetAptitudeData>();
                    asset.aptitudeDatas = new List<AptitudeData>();
                    asset.setName = apt.SetName;
                    for (int i = 0; i < apt.aptitudes.Count; i++)
                    {
                        for (int j = 0; j < allAptitudes.Length; j++)
                        {
                            if (allAptitudes[j].name == apt.aptitudes[i])
                                asset.aptitudeDatas.Add(allAptitudes[j]);
                        }
                    }
                    AssetDatabase.CreateAsset(asset, "Assets/Resources/SetResources/" + apt.SetName + ".asset");
                    AssetDatabase.SaveAssets();

                }
                EditorUtility.FocusProjectWindow();

                Selection.activeObject = asset;
            }
        }
        
    }
    private void OnGUI()
    {
        GUILayout.BeginHorizontal(); 
        string[] options = new string[]
        {
         "Import Monster", "Import Aptitude", "Import Set Aptitude",
        };
        selected = EditorGUILayout.Popup("Import type", selected, options);
        GUILayout.EndHorizontal();
        if (selected == 0)
            ImportMonters();
        else if (selected == 1)
            ImportAptitude();
        else
            ImportSetAptitude();
    }
}
