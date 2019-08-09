using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnBoard : MonoBehaviour
{
    private Color startColor;
    void Start()
    {
        startColor = this.GetComponent<Image>().color;
    }
    void Update()
    {
        if ((Mathf.Abs(Input.mousePosition.x-this.transform.position.x)<=50)&&(Mathf.Abs(Input.mousePosition.y - this.transform.position.y) <= (50f)))
        {
            GameObject.Find("ReconnaissanceManager").GetComponent<ReconnaissanceManager>().activeCell = this;
            this.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            this.GetComponent<Image>().color = startColor;
        }
    }
}
