using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "AptitudeData", menuName = "Aptitude Data", order = 1)]
public class AptitudeData : ScriptableObject
{
    [System.Serializable]
    public enum SpellType
    {
        Magique,
        Physique,
        Souffle,
        Soutien,
        Soin,
        Stat
    }
    [System.Serializable]
    public enum SpellElem
    {
        None,
        Feu,
        Bang,
        Vent,
        Lumiere,
        Tenebre,
        Crame,
        Glace,
        SouffleFeu,
        SouffleGlace
    }

    public enum SizeSpell
    {
        None,
        Normal,
        Super,
        Mega,
        Giga
    }

    public enum StatBonus
    {
        None,
        HP,
        MP,
        ATK,
        DEF,
        AGI,
        WIS
    }

    public string spellname;
    public SpellType spellType;
    public SizeSpell sizeSpell;
    public bool isMulticible;
    public SpellElem spellElem;
    public int cost;
    public string effectText;
    public StatBonus statBonus;
    public int bonusValue;

    public AptitudeData()
    {
        spellname = "";
        spellType = new SpellType();
        sizeSpell = new SizeSpell();
        isMulticible = false;
        spellElem = new SpellElem();
        cost = 0;
        effectText = "";
        statBonus = new StatBonus();
        bonusValue = 0;
    }
}
