using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyUnit : MonoBehaviour
{
    public new string name;
    public void OnClick()
    {
        var am = GameObject.Find("AssemblyManager").GetComponent<AssemblyManager>();
        am.SetCurrentUnit = GameManager.AllChars[name];
        GameObject.Find("SelectHeroPanel").SetActive(false);
    }
}
