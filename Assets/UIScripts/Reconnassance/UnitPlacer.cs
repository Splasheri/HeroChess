using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitPlacer : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public int Id;
    public int position;
    private Vector3 oldPosition;
    private void Start()
    {
        oldPosition = this.transform.localPosition;
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("ReconnaissanceManager").GetComponent<ReconnaissanceManager>().activeUnit = this;
        this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
        this.gameObject.transform.position = Input.mousePosition- new Vector3(-300,300,0);
        if (this.transform.localPosition.x<150&&this.transform.localPosition.y>-650)
        {
            GameObject.Find("ReconnaissanceManager").GetComponent<ReconnaissanceManager>().activeCell = null;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (GameObject.Find("ReconnaissanceManager").GetComponent<ReconnaissanceManager>().activeCell == null || GameObject.Find("ReconnaissanceManager").GetComponent<ReconnaissanceManager>().activeCell.transform.parent.localPosition.y > 350)
        {
            this.transform.SetParent(GameObject.Find("UserSquad").transform);
            this.transform.localPosition = oldPosition;
            this.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        }
        else
        {
            this.transform.parent = GameObject.Find("ReconnaissanceManager").GetComponent<ReconnaissanceManager>().activeCell.transform.parent;
            this.transform.localPosition = GameObject.Find("ReconnaissanceManager").GetComponent<ReconnaissanceManager>().activeCell.transform.localPosition;
            SquadManager.UnitStats s = GameManager.userSquad[Id];
            s.position = (int)(this.transform.localPosition.x / 100 + this.transform.parent.localPosition.y * 8 / 100);
            this.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            GameManager.userSquad[Id] = s;
        }
    }
}
