using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class FightManager : MonoBehaviour
{
    public static FightManager instance;

    [SerializeField] List<MonsterSlots> teamPlayer;

    [SerializeField] GameObject allbtn;
    [SerializeField] GameObject spellListCanvas;
    [SerializeField] GameObject spellBtnParent;
    [SerializeField] GameObject spellBtn;
    [SerializeField] GameObject selectMonsterCanvas;
    [SerializeField] GameObject FightRecap;
    [SerializeField] GameObject victoryScreen;
    [SerializeField] TextMeshProUGUI actualMonsterText;

    public bool isSelectingTarget;

    int targetableTeam;
    int actualTurn;
    bool IsTurnTeam1;
    int sizeTeam;
    int actualMonster;
    bool fightStart;
    bool useSupApt;
    int nbTurn;
    SetupBtnAction setupBtnAction;

    List<Action> actionsList;
    public struct Action
    {
        public AptitudeData aptitude;
        public SlotScript user;
        public List<SlotScript> targets;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    private void Start()
    {
        actionsList = new List<Action>();
        InitNewTurn();
        nbTurn = 0;

        setupBtnAction = gameObject.AddComponent<SetupBtnAction>();
    }
    public List<SlotScript> SelectCible(AptitudeData _apt, bool _isSupport)
    {
        //SlotScript user = teamPlayer[actualTurn].GetSlotsById(actualMonster);
        if (!_isSupport)
        {
            if (_apt.isMulticible)
            {
                List<SlotScript> temp = teamPlayer[(actualTurn + 1) % 2].GetAllSlot();
                ChangeMonsterTurn();
                return temp;
            }
            else
            {
                isSelectingTarget = true;
                spellListCanvas.SetActive(false);
                selectMonsterCanvas.SetActive(true);
                useSupApt = false;

            }
        }
        else
        {
            if (_apt.isMulticible)
            {
                List<SlotScript> temp = teamPlayer[actualTurn].GetAllSlot();
                ChangeMonsterTurn();
                return temp;
            }
            else
            {
                isSelectingTarget = true;
                spellListCanvas.SetActive(false);
                selectMonsterCanvas.SetActive(true);
                useSupApt = true;
            }
        }
        return null;
    }

    void InitNewTurn()
    {
        IsTurnTeam1 = true;
        actualTurn = 0;
        actualMonster = 0;
        sizeTeam = teamPlayer[0].GetSizeTeam();
        fightStart = false;
        useSupApt = false;
        actualMonsterText.text = teamPlayer[actualTurn].GetMonsterById(actualMonster).monsterName;
        ResetUI();
    }
    void ResetUI()
    {
        allbtn?.SetActive(true);
        spellListCanvas?.SetActive(false);
        selectMonsterCanvas?.SetActive(false);
        FightRecap?.SetActive(false);
        victoryScreen?.SetActive(false);
    }
    void ChangeMonsterTurn()
    {
        if (actualMonster < sizeTeam - 1)
        {
            actualMonster++;
            if (teamPlayer[actualTurn].GetMonsterById(actualMonster).stats.HP <= 0)
                ChangeMonsterTurn();
            ResetUI();
        }
        else
        {
            actualTurn++;
            if (actualTurn <= 1)
            {
                actualMonster = 0;
                sizeTeam = teamPlayer[actualTurn].GetSizeTeam();
                ResetUI();
            }
            else
            {
                actualTurn = 1;
                SetFightUI();
            }
        }
        actualMonsterText.text = teamPlayer[actualTurn].GetMonsterById(actualMonster).monsterName;
    }
    void SetVictoryScreen(int _winner)
    {
        allbtn?.SetActive(false);
        spellListCanvas?.SetActive(false);
        selectMonsterCanvas?.SetActive(false);
        FightRecap?.SetActive(false);
        victoryScreen?.SetActive(true);

        victoryScreen.GetComponent<VictoryScript>().SetVictory(_winner, actionsList);
    }


    void SetFightUI()
    {
        allbtn.SetActive(false);
        spellListCanvas.SetActive(false);
        selectMonsterCanvas.SetActive(false);
        FightRecap.SetActive(true);
        fightStart = true;
    }
    void UpdateFightRecap()
    {
        actionsList = actionsList.OrderByDescending(agi => agi.user.GetMonster().stats.AGI).ToList();
        FightRecap.GetComponent<FightRecapScript>().InitValue(actionsList);
        int temp = checkTeamDeath();
        if (temp >= 0)
        {
            SetVictoryScreen(temp + 1 % 2);
        }
    }
    int checkTeamDeath()
    {
        int tempAlive = 0; 
        for (int i = 0; i < teamPlayer[0].GetAllMonster().Count; i++)
        {
            if (teamPlayer[0].GetMonsterById(i).stats.HP > 0)
                tempAlive++;
        }
        if (tempAlive <= 0)
            return 0;
        for (int i = 0; i < teamPlayer[1].GetAllMonster().Count; i++)
        {
            if (teamPlayer[1].GetMonsterById(i).stats.HP > 0)
                tempAlive++;
        }
        if (tempAlive <= 0)
            return 1;

        return -1;
    }

    public void SetTarget(SlotScript _monster)
    {
        Action temp = actionsList[actionsList.Count - 1];
        temp.targets = new List<SlotScript> { _monster };
        actionsList[actionsList.Count - 1] = temp;
        isSelectingTarget = false;
        ChangeMonsterTurn();
        if (fightStart)
        {
            UpdateFightRecap();
        }
    }
    public void SetUISelectedMonster(string _nameMonster)
    {
        selectMonsterCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _nameMonster;
    }


    public void EffectBtnSpell(AptitudeData _apt)
    {
        Action action = new Action();
        action.user = teamPlayer[actualTurn].GetSlotsById(actualMonster);
        action.aptitude = _apt;
        action.targets = new List<SlotScript>();
        if (_apt.spellType == AptitudeData.SpellType.Soutien || _apt.spellType == AptitudeData.SpellType.Soin)
        {
            targetableTeam = actualTurn;
            action.targets = SelectCible(_apt, true);
        }
        else
        {
            if (actualTurn == 0)
                targetableTeam = 1;
            else
                targetableTeam = 0;

            action.targets = SelectCible(_apt, false);
        }
        actionsList.Add(action);
        if (fightStart)
        {
            UpdateFightRecap();
        }
    }
    public void DefenseBtn()
    {
        if (IsTurnTeam1)
        {
            //teamPlayer1.
        }
    }
    public void AttaqueBtn()
    {
        allbtn.SetActive(false);
        spellListCanvas.SetActive(true);

        setupBtnAction.InitBtnSpell(spellBtnParent.GetComponent<RectTransform>(), spellBtn, actualMonster, teamPlayer[actualTurn]);
    }
    public void ReturnBtn()
    {
        allbtn.SetActive(true);
        spellListCanvas.SetActive(false);
    }
    public void ReturnBtnSpell()
    {
        spellListCanvas.SetActive(true);
        selectMonsterCanvas.SetActive(false);
        actionsList.Remove(actionsList[actionsList.Count - 1]);
    }
    public void NewTurnBtn()
    {
        InitNewTurn();
        if (teamPlayer[0].GetMonsterById(0).stats.HP <= 0)
        {
            ChangeMonsterTurn();
        }
        nbTurn++;
    }

    public int GetActualTargetableTeam()
    {
        if (useSupApt)
            return (actualTurn + 1) % 2;
        else
            return actualTurn;
    }

    public void Restart()
    {
        teamPlayer[0].ResetAllMonster();
        teamPlayer[1].ResetAllMonster();
        InitNewTurn();
        nbTurn = 0;
    }
}
