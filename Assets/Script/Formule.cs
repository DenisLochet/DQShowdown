using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formule
{
    List<float> coMono = new List<float> { 20,70,150,300};
    float rMono = .2f;
    List<float> coMulti = new List<float> { 13.3f,46.7f,100,200};
    float rMulti = .1f;


    int MagiqueDamage(float co, float r, float WIS)
    {
        float temp = co + r * WIS;
        float tempRng = .05f * temp;
        return (int)Random.Range(temp - tempRng, temp + tempRng);
    }

    float CheckRatio(FightManager.Action act)
    {
        return .0f;
    }
    int PhysiqueDamage(float ATK, float DEF)
    {
        float temp = ATK / 2 - DEF / 4;
        float tempRng = .05f * temp;
        return (int)Random.Range(temp - tempRng, temp + tempRng);
    }

    int SouffleDamage(string apt)
    {
        int temp = 0;
        if (apt == "Souffle frais" || apt =="Souffle de feu")
            temp = 18;
        else if (apt == "Cracheflamme" || apt == "Souffle froid")
            temp = 37;
        else if (apt == "Inferno" || apt == "Souffle de givre")
            temp = 82;
        else if (apt == "Brulure" || apt == "Souffle gla-glacial")
            temp = 196;
        return (int)Random.Range(temp - temp * .05f, temp + temp * .05f);
    }
    public int AptitudeDamage(FightManager.Action act,int targetID)
    {
        switch (act.aptitude.spellType)
        {
            case AptitudeData.SpellType.Magique:
                {
                    float ratio = CheckRatio(act); 
                    if (!act.aptitude.isMulticible)
                    {
                        return MagiqueDamage(coMono[(int)act.aptitude.sizeSpell - 1], rMono+ ratio, act.user.GetMonster().stats.WIS);
                    }
                    else
                    {
                        return MagiqueDamage(coMulti[(int)act.aptitude.sizeSpell - 1], rMulti+ ratio, act.user.GetMonster().stats.WIS);
                    }
                }
                
                break;
            case AptitudeData.SpellType.Physique:
                return PhysiqueDamage(act.user.GetMonster().stats.ATK, act.user.GetMonster().stats.DEF);
                break;
            case AptitudeData.SpellType.Souffle:
                return SouffleDamage(act.aptitude.spellname);
                break;
            default:
                Debug.LogError("Ce n'est pas une Aptitude Valide");
                break;
        }        
        return 0;
    }
}
