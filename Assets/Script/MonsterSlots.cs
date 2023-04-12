using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MonsterSlots : MonoBehaviour
{
    [SerializeField] int team;
    [SerializeField] List<SlotScript> slots = new List<SlotScript>();
    List<MonstersData> monsters = new List<MonstersData>();
    int sizeMaxInTeam;
    int OffsetDown = 400;
    int OffsetNomal = 250;
    static int sizeSmall = (int)MonstersData.Size.Small;
    static int sizeNormal = (int)MonstersData.Size.Normal;
    static int sizeGiant = (int)MonstersData.Size.Giant;


    void SetupSlot(int _slot, int _size)
    {
        slots[_slot].SetImage(monsters[_slot].sprite);
        Vector2 temp = new Vector2
            (slots[_slot].GetImgSize().x,
            slots[_slot].GetImgSize().y * (int)monsters[_slot].size);
        slots[_slot].SetImgSize(temp);
        slots[_slot].ChangeHPText(monsters[_slot].stats.HP.ToString());
        slots[_slot].ChangeMPText(monsters[_slot].stats.MP.ToString());
        if (sizeMaxInTeam < sizeGiant)
            sizeMaxInTeam += _size;
    }

    void InitMonsterList()
    {
        int _size = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            MonstersData tempMonster = slots[i].GetMonster();
            if (tempMonster == null)
                continue;
            _size += (int)tempMonster.size;
            if (_size > 3)
                return;
            slots[i].SetTeam(team);
            monsters.Add(tempMonster);
        }
    }
    void InitMonsterSlotData()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetImage(null);
        }
        if (monsters.Count != 0)
        {
            sizeMaxInTeam = 0;
            for (int i = 0; i < monsters.Count; i++)
            {
                switch (monsters[i].size)
                {
                    case (MonstersData.Size.Small):
                        SetupSlot(i, sizeSmall);
                        break;
                    case (MonstersData.Size.Normal):
                        SetupSlot(i, sizeNormal);
                        break;
                    case (MonstersData.Size.Giant):
                        SetupSlot(i, sizeGiant);
                        break;
                }
            }
        }
    }

    void SetPosition(int _slot, int _offset = 0, int _particularOffset = 0)
    {
        Vector3 pos;
        if (_particularOffset != 0)
        {
            pos = new Vector3(slots[_slot].transform.position.x, transform.position.y + _offset * _slot - _particularOffset, 0);
            slots[_slot].transform.position = pos;
            return;
        }
        pos = new Vector3(slots[_slot].transform.position.x, transform.position.y + _offset * _slot - _offset / 2, 0);
        slots[_slot].transform.position = pos;
    }

    void InitMonstersSlotPos()
    {        
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].GetImage() == null)
                slots[i].SetActive(false);

            if (monsters.Count == 1)
            {
                SetPosition(i);
                continue;
            }

            if (sizeMaxInTeam == 3)
            {
                if (slots[i].GetImage() != null)
                {
                    if (monsters.Count == 1)
                    {
                        SetPosition(i);
                    }
                    else if (monsters.Count == 2)
                    {
                        if (monsters[0].size == MonstersData.Size.Normal)
                        {
                            SetPosition(i, OffsetDown, 150);
                            Vector3 pos = new Vector3(slots[i].transform.position.x, transform.position.y + OffsetDown * i - 150, 0);
                            slots[i].transform.position = pos;
                        }
                        else
                        {
                            SetPosition(i, OffsetDown, 150);
                            Vector3 pos = new Vector3(slots[i].transform.position.x, transform.position.y + OffsetDown * i - 250, 0);
                            slots[i].transform.position = pos;
                        }
                    }
                }
            }
            else if (sizeMaxInTeam == 2)
            {
                if (slots[i].GetImage() != null)
                {
                    SetPosition(i, OffsetNomal);
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        InitMonsterList();
        InitMonsterSlotData();
        InitMonstersSlotPos();
    }
    
    public int GetSizeTeam()
    {
        return monsters.Count;
    }

    public SlotScript GetSlotsById(int _id)
    {
        if (_id >= 0 && _id <= 3)
            return slots[_id];
        return null;
    }

    public List<SlotScript> GetAllSlot()
    {
        List<SlotScript> temp = new List<SlotScript>();
        foreach (var item in slots)
        {
            if(item.isActiveAndEnabled)
                temp.Add(item);
        }
        return temp;
    }

    public MonstersData GetMonsterById(int _id)
    {
        if (_id >= 0 && _id <= 3)
            return slots[_id].GetMonster();
        return null;
    }
    
    public List<MonstersData> GetAllMonster()
    {
        return monsters;
    }

    public void ResetAllMonster()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].isActiveAndEnabled)
                slots[i].ResetValue();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
