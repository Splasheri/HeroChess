using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssemblyManager : MonoBehaviour
{
    private SquadManager.UnitStats currentUnit;
    public SquadManager.UnitStats SetCurrentUnit { set { currentUnit = value; UpdateStats(); }  }
    private int gold;
    public int Gold { get => gold; set => gold = value; }
    private Transform stat, figure, buyButton, lvlUpButton, userSquad, toBoard;
    private void Start()
    {
        figure = GameObject.Find("UnitFigure").transform;
        stat = GameObject.Find("Stats").transform;
        buyButton = GameObject.Find("BUYBUTTON").transform;
        lvlUpButton = GameObject.Find("LVLUP").transform;
        userSquad = GameObject.Find("UserSquad").transform;
        toBoard = GameObject.Find("TOBOARD").transform;
        Gold = GameManager.gold;
        UnitList.LoadPanel();
        UnitList.LoadUserSquad();
        if (GameManager.userSquad.Count>0)
        {
            toBoard.GetComponent<Button>().interactable = true;
        }
    }
    private void UpdateStats()
    {
        stat.GetChild(0).GetChild(4).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = currentUnit.hp.ToString();
        stat.GetChild(1).GetChild(4).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = currentUnit.amountofdice.ToString();
        stat.GetChild(2).GetChild(4).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = currentUnit.initiative.ToString();
        stat.GetChild(3).GetChild(4).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = currentUnit.movetype.ToString();
        stat.GetChild(4).GetChild(4).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = currentUnit.dicetype.ToString();
        stat.GetChild(5).GetChild(4).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = currentUnit.attacktype.ToString();
        stat.GetChild(6).GetChild(4).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = currentUnit.attackEffect==AttackPatterns.attackEffect.JustHit ? currentUnit.skilltype.ToString() : currentUnit.attackEffect.ToString();
        stat.GetChild(7).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = currentUnit.fileName;
        figure.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Units/fig/" + currentUnit.fileName) as Sprite;
        if (currentUnit.cost<=gold)
        {
            buyButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            buyButton.GetComponent<Button>().interactable = false;
        }
        if (gold>150)
        {
            lvlUpButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            lvlUpButton.GetComponent<Button>().interactable = false;
        }
        if (GameManager.userSquad.Count > 0)
        {
            toBoard.GetComponent<Button>().interactable = true;
        }
        GameObject.Find("SubCost").GetComponent<TMPro.TextMeshProUGUI>().text = currentUnit.cost.ToString();
    }
    public void LvlUp()
    {
        currentUnit.hp += 15;
        currentUnit.amountofdice += 1;
        currentUnit.initiative += 1;
        currentUnit.cost += 150;
        UpdateStats();
    }
    public void BuyUnit()
    {
        currentUnit.team = 1;
        GameManager.userSquad.Add(currentUnit);
        gold -= currentUnit.cost;
        GameManager.gold = gold;
        currentUnit = GameManager.AllChars["BladeMaster"];
        UpdateStats();
        UnitList.LoadUserSquad(GameManager.userSquad.Count-1);
    }
}
