using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconnaissanceManager : MonoBehaviour
{
    public UnitPlacer activeUnit;
    public OnBoard activeCell;
    // Start is called before the first frame update
    void Start()
    {
        UnitList.LoadUserSquadRec();
        LoadEnemyUnits();
    }

    private void LoadEnemyUnits()
    {
        foreach (var unit in GameManager.enemySquad)
        {
            GameObject parent = GameObject.Find("Board").transform.GetChild(unit.position / 8).GetChild(unit.position % 8).gameObject;
            GameObject instance = GameObject.Instantiate(parent);
            instance.transform.SetParent(parent.transform);
            instance.transform.localPosition = Vector3.zero;
            instance.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Units/fig/"+unit.fileName);
            instance.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("UserSquad").transform.childCount == 0)
        {
            if (GameObject.Find("BattleButton").GetComponent<UnityEngine.UI.Button>().interactable == false)
            {
                GameObject.Find("BattleButton").GetComponent<UnityEngine.UI.Button>().interactable = true;
            }        
        }
        else
        {
            GameObject.Find("BattleButton").GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
    }
}
