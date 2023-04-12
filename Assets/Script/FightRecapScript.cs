using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightRecapScript : MonoBehaviour
{
    [SerializeField] Color healCol;
    [SerializeField] Color damageCol;
    [SerializeField] GameObject text;
    [SerializeField] Transform content;
    Formule formule = new Formule();
    public void InitValue(List<FightManager.Action> _action)
    {
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        int totalTxt = 0;
        for (int i = 0; i < _action.Count; i++)
        {
            for (int j = 0; j < _action[i].targets.Count; j++)
            {
                GameObject go = Instantiate(text, content);
                go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _action[i].user.GetMonster().monsterName + " Utilise " + _action[i].aptitude.spellname;
                if (_action[i].aptitude.spellType == AptitudeData.SpellType.Soutien || _action[i].aptitude.spellType == AptitudeData.SpellType.Soin)
                {
                    go.GetComponent<RawImage>().color = healCol;
                    int heal = SetHeal(_action[i], j);
                    go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _action[i].targets[j].GetMonster().monsterName + " Recupere " + heal.ToString() + " PV";
                }
                else
                {
                    go.GetComponent<RawImage>().color = damageCol;
                    int dmg = SetDamage(_action[i], j);
                    go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                        _action[i].targets[j].GetMonster().monsterName + " Prend " + dmg.ToString() + " degat";
                }
                totalTxt++;
            }
        }
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().sizeDelta.x, 115 * totalTxt -15);
    }


    int SetDamage(FightManager.Action _act, int _target)
    {
        int dmg = 0;
        dmg = formule.AptitudeDamage(_act, _target);
        _act.targets[_target].AppliDamage(dmg);
        return dmg;
    }
    int SetHeal(FightManager.Action _act, int _target)
    {
        int heal = 0;
        heal = formule.AptitudeDamage(_act, _target);
        _act.targets[_target].AppliHeal(heal);
        return heal;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
