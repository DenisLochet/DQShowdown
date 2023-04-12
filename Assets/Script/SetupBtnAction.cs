using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupBtnAction : MonoBehaviour 
{
    public void InitBtnSpell(RectTransform _content,GameObject _spellBtn, int _monsterId,MonsterSlots _team)
    {
        for (int i = 0; i < _content.childCount; i++)
        {
            Destroy(_content.GetChild(i).gameObject);
        }
        SlotScript tempMonster = _team.GetSlotsById(_monsterId);

        int totalbtn = 0;
        for (int i = 0; i < tempMonster.GetNumberOfSetAtp(); i++)
        {
            for (int j = 0; j < tempMonster.GetSetAptitudeData(i).aptitudeDatas.Count; j++)
            {
                if (tempMonster.GetSetAptitudeData(i).aptitudeDatas[j].spellType == AptitudeData.SpellType.Stat)
                {
                    continue;
                }
                GameObject go = Instantiate(_spellBtn, _content);
                go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tempMonster.GetSetAptitudeData(i).aptitudeDatas[j].spellname;
                go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "MP " + tempMonster.GetSetAptitudeData(i).aptitudeDatas[j].cost.ToString();

                AptitudeData temp = tempMonster.GetSetAptitudeData(i).aptitudeDatas[j];
                go.GetComponent<Button>().onClick.AddListener( () => FightManager.instance.EffectBtnSpell(temp));

                totalbtn++;
            }
        }
        _content.sizeDelta = new Vector2(_content.sizeDelta.x, 65 * totalbtn - 15);
    }
}
