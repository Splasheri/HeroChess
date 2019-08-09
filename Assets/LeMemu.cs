using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeMemu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {        
        if (Resources.Load<TextAsset>("Squads/" + GameManager.BinomList[GameManager.currentBinom] + "/Lvl" + GameManager.BinomLevelList[GameManager.currentBinom]) != null)
        {
            GameObject.Find("Level").GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.BinomLevelList[GameManager.currentBinom].ToString();
            GameManager.enemySquad = GameManager.ParseSquadFile(Resources.Load<TextAsset>("Squads/" + GameManager.BinomList[GameManager.currentBinom] + "/Lvl" + GameManager.BinomLevelList[GameManager.currentBinom]).text);
        }
        else
        {
            GameManager.BinomLevelList[GameManager.currentBinom]--;
            GameObject.Find("Level").GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.BinomLevelList[GameManager.currentBinom].ToString();
            GameManager.enemySquad = GameManager.ParseSquadFile(Resources.Load<TextAsset>("Squads/" + GameManager.BinomList[GameManager.currentBinom] + "/Lvl" + GameManager.BinomLevelList[GameManager.currentBinom]).text);
        }
    }
}
