using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Weapons
{
    public bool Sword;
    public bool Spear;
    public bool Axe;
    public bool Hammer;
    public bool Whip;
    public bool Claw;
    public bool Staff;
}

[System.Serializable]
public struct Stats
{
    public int HP;
    public int MP;
    public int ATK;
    public int DEF;
    public int AGI;
    public int WIS;
}

[CreateAssetMenu(fileName = "MonstersData", menuName = "Monster Script", order = 1)]
 [System.Serializable]
public class MonstersData : ScriptableObject
{
    [System.Serializable]
    public enum Type
    {
        Gluant,
        Dragon,
        Naturel,
        Bete,
        Matiere,
        Demon,
        Zombie,
        Boss
    }
    [System.Serializable]
    public enum Size
    {
        Small = 1,
        Normal,
        Giant,
    }

    public string monsterName;
    public Sprite sprite;
    public string rank;
    public Type type;
    public Size size;
    public List<MonstersPassif.Passif> passif;
    public Stats stats;
    public Weapons weapons;
    public AptitudeData aptDefence;
    public AptitudeData aptAttaque;
    public MonstersData(MonstersData _monsterData)
    {
        this.monsterName = _monsterData.monsterName;
        this.sprite = _monsterData.sprite;
        this.rank = _monsterData.rank;
        this.type = _monsterData.type;
        this.size = _monsterData.size;
        this.passif = _monsterData.passif;
        this.stats = _monsterData.stats;
        this.weapons = _monsterData.weapons;
        this.aptDefence = _monsterData.aptDefence;
        this.aptAttaque = _monsterData.aptAttaque;
    }
    
    public MonstersData()
    {
        this.monsterName = "";
        this.sprite = null;
        this.rank = "";
        this.type = Type.Gluant;
        this.size = Size.Small;
        this.passif = new List<MonstersPassif.Passif>();
        this.stats = new Stats();
        this.weapons = new Weapons();
        this.aptDefence = null;
        this.aptAttaque = null;
    }
}
