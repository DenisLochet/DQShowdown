using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI victoryTxt;
    [SerializeField] GameObject text;
    [SerializeField] Transform content;


    public void SetVictory(int playerWin, List<FightManager.Action> _recap)
    {
        victoryTxt.text = "Victoire du joueur " + playerWin;
        if (_recap != null)
        {
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }

            int totalTxt = 0;
            for (int i = 0; i < _recap.Count; i++)
            {
                for (int j = 0; j < _recap[i].targets.Count; j++)
                {
                    GameObject go = Instantiate(text, content);
                    go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _recap[i].user.GetMonster().monsterName + " Utilise " + _recap[i].aptitude.spellname;
                    if (_recap[i].aptitude.spellType == AptitudeData.SpellType.Soutien || _recap[i].aptitude.spellType == AptitudeData.SpellType.Soin)
                    {
                        go.GetComponent<RawImage>().color = Color.green;
                        go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _recap[i].targets[j].GetMonster().monsterName + " Recupere " + "TODO" + " PV";
                    }
                    else
                    {
                        go.GetComponent<RawImage>().color = Color.red;
                        go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                            _recap[i].targets[j].GetMonster().monsterName + " Prend " + "TODO" + " degat";
                    }
                    totalTxt++;
                }
            }
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().sizeDelta.x, 115 * totalTxt - 15);
        }
    }

    public void RestartBtn()
    {
        FightManager.instance.Restart();
    }

    public void ReturnMenuBtn()
    {
        SceneManager.LoadScene(0);
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
