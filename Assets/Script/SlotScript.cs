using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    Image imageComp;
    RectTransform rectTransform;
    [SerializeField] TextMeshProUGUI HPText;
    [SerializeField] TextMeshProUGUI MPText;

    [SerializeField] MonstersData monsterData;
    [SerializeField] List<SetAptitudeData> setAptitudeDatas;

    MonstersData monster;
    MonstersData monsterBase;
    int team;

    private void Awake()
    {
        imageComp = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        if (monsterData == null)
            return;
        monster = new MonstersData(monsterData);
        monsterBase = new MonstersData(monster);
        team = -1;
    }

    void RemoveSetAptitudeUpperValue(int _i)
    {
        if (setAptitudeDatas.Count > _i)
        {
            for (int i = _i; i < setAptitudeDatas.Count; i++)
            {
                setAptitudeDatas.RemoveAt(i);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (monster == null)
            return;
        switch (monster.size)
        {
            case MonstersData.Size.Small:
                RemoveSetAptitudeUpperValue(3);
                break;
            case MonstersData.Size.Normal:
                RemoveSetAptitudeUpperValue(4);
                break;
            case MonstersData.Size.Giant:
                RemoveSetAptitudeUpperValue(5);
                break;
            default:
                break;
        }

        for (int i = 0; i < setAptitudeDatas.Count; i++)
        {
            for (int j = 0; j < setAptitudeDatas[i].aptitudeDatas.Count; j++)
            {
                if (setAptitudeDatas[i].aptitudeDatas[j].statBonus != AptitudeData.StatBonus.None)
                {
                    switch (setAptitudeDatas[i].aptitudeDatas[j].statBonus)
                    {
                        case AptitudeData.StatBonus.HP:
                            monster.stats.HP += setAptitudeDatas[i].aptitudeDatas[j].bonusValue;
                            break;
                        case AptitudeData.StatBonus.MP:
                            monster.stats.MP += setAptitudeDatas[i].aptitudeDatas[j].bonusValue;
                            break;
                        case AptitudeData.StatBonus.ATK:
                            monster.stats.ATK += setAptitudeDatas[i].aptitudeDatas[j].bonusValue;
                            break;
                        case AptitudeData.StatBonus.DEF:
                            monster.stats.DEF += setAptitudeDatas[i].aptitudeDatas[j].bonusValue;
                            break;
                        case AptitudeData.StatBonus.AGI:
                            monster.stats.AGI += setAptitudeDatas[i].aptitudeDatas[j].bonusValue;
                            break;
                        case AptitudeData.StatBonus.WIS:
                            monster.stats.WIS += setAptitudeDatas[i].aptitudeDatas[j].bonusValue;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetImage(Sprite _sprite)
    {
        imageComp.sprite = _sprite;
    }
    public Sprite GetImage()
    {
       return imageComp.sprite;
    }
    public void SetImgSize(Vector2 _transform)
    {
        rectTransform.sizeDelta = new Vector2(_transform.x, _transform.y);
    }
    public Vector2 GetImgSize()
    {
        return rectTransform.sizeDelta;
    }
    public void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    public MonstersData GetMonster()
    {
        return monster;
    }

    public void ChangeHPText(string _value)
    {
        HPText.text = "HP: "+ _value;
    }
    public void ChangeMPText(string _value)
    {
        MPText.text = "MP: " + _value;
    }

    public int GetNumberOfSetAtp()
    {
        return setAptitudeDatas.Count;
    }

    public int GetTeam()
    {
        return team;
    }

    public void SetTeam(int _team)
    {
        team = _team;
    }

    public void AppliDamage(int damage)
    {
        monster.stats.HP -= damage;
        if(monster.stats.HP < 0)
            monster.stats.HP = 0;
        ChangeHPText(monster.stats.HP.ToString());
    }

    public void AppliHeal(int _heal)
    {
        monster.stats.HP += _heal;
        if(monster.stats.HP > monsterBase.stats.HP)
            monster.stats.HP = monsterBase.stats.HP;
        ChangeHPText(monster.stats.HP.ToString());
    }

    public void ResetValue()
    {
        monster = monsterBase;
        ChangeHPText(monster.stats.HP.ToString());
    }
    public SetAptitudeData GetSetAptitudeData(int _id)
    {
        if (_id >= 0 && _id < setAptitudeDatas.Count)
            return setAptitudeDatas[_id];
        return null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (FightManager.instance.isSelectingTarget && team != FightManager.instance.GetActualTargetableTeam())
        {
            FightManager.instance.SetTarget(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (FightManager.instance.isSelectingTarget && team != FightManager.instance.GetActualTargetableTeam())
        {
            FightManager.instance.SetUISelectedMonster(monster.monsterName);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (FightManager.instance.isSelectingTarget && team != FightManager.instance.GetActualTargetableTeam())
        {
            FightManager.instance.SetUISelectedMonster(" ");
        }
    }
}
