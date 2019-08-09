using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyUpdater : MonoBehaviour
{
    public AssemblyManager am;
    // Update is called once per frame
    void Update()
    {
        this.GetComponent<TMPro.TextMeshProUGUI>().text = am.Gold.ToString();
    }
}
