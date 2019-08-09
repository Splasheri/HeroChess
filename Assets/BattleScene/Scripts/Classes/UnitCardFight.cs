using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using DragonBones;

public class UnitCardFight : MonoBehaviour
{
    public SquadManager.UnitStats stats;
    private void Start()
    {
        this.transform.GetChild(0).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = stats.characterName;
        this.transform.GetChild(3).GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = stats.dicetype.ToString();
        this.transform.GetChild(4).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = stats.initiative.ToString();
        this.transform.GetChild(5).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = stats.hp.ToString();
        this.transform.GetChild(6).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = stats.amountofdice.ToString();        
        UnityFactory.factory.BuildArmatureComponent(stats.fileName, stats.fileName, "", stats.fileName, this.transform.GetChild(1).GetChild(1).gameObject, true);
        this.transform.GetChild(1).GetChild(1).gameObject.GetComponent<UnityArmatureComponent>().animation.Play("Idle", 0);
    }

    public bool UpdateData(Entity entity, int damage=0)
    {
        var em = World.Active.EntityManager;
        var a = em.GetComponentData<Attack>(entity);
        var i = em.GetComponentData<Initiative>(entity);
        var hp = em.GetComponentData<HP>(entity);
        stats.amountofdice = a.amountOfCubes;
        stats.initiative = i.value;
        stats.hp = hp.currentValue-damage;
        stats.dicetype = a.typeOfCubes;
        this.transform.GetChild(3).GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = stats.dicetype.ToString();
        this.transform.GetChild(4).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = stats.initiative.ToString();
        this.transform.GetChild(5).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = stats.hp.ToString();
        this.transform.GetChild(6).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = stats.amountofdice.ToString();
        return false;
    }
}
