using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SquadsManagement : MonoBehaviour
{
    private List<attackData> attacks;
    private static SquadsManagement manager;
    public static SquadsManagement instance
    {
        get
        {
            return manager;
        }
    }

    public List<attackData> Attacks { get => attacks; set => attacks = value; }

    public Dictionary<int, SquadManager.UnitStats> userSquad;
    public Dictionary<int, SquadManager.UnitStats> enemySquad;
    public Dictionary<int, GameObject> allCards;

    public GameObject userScrollListContent;
    public GameObject enemyScrollListContent;
    public GameObject unitCardPrefab;
    public GameObject damagePrefab;
    public GameObject activePrefab;

    private void Start()
    {
        manager = this;
        userSquad = new Dictionary<int, SquadManager.UnitStats>();
        enemySquad = new Dictionary<int, SquadManager.UnitStats>();
        allCards = new Dictionary<int, GameObject>();
        userScrollListContent = GameObject.Find("UserSquad");
        enemyScrollListContent = GameObject.Find("EnemySquad");
        unitCardPrefab = Resources.Load<GameObject>("Prefabs/HeroCardView");
        damagePrefab = Resources.Load<GameObject>("Prefabs/DamagePopup");
        activePrefab = Resources.Load<GameObject>("Prefabs/ActivePrefab");
    }

    public void CreateSquads()
    {
        CreateSquad(GameManager.userSquad);
        CreateSquad(GameManager.enemySquad);
    }

    public void CreateSquad(List<SquadManager.UnitStats> units) //cards creation
    {
        if (units[0].team>0)
        {
            foreach (var unit in units)
            {
                userSquad.Add(unit.id,unit);
                userScrollListContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0,userSquad.Count*280);                
                allCards.Add(unit.id, GameObject.Instantiate(unitCardPrefab));
                allCards[unit.id].transform.parent = GameObject.Find("UserSquad").transform;
                allCards[unit.id].transform.localPosition = new Vector3(210,-100-280* (userSquad.Count-1), 150);
                allCards[unit.id].transform.localScale = new Vector3(0.66f, 0.66f, 0.66f);
                allCards[unit.id].GetComponent<UnitCardFight>().stats = unit;
            }
        }
        else
        {
            foreach (var unit in units)
            {
                enemySquad.Add(unit.id, unit);
                enemyScrollListContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, enemySquad.Count * 280);
                allCards.Add(unit.id, GameObject.Instantiate(unitCardPrefab));
                allCards[unit.id].transform.parent = GameObject.Find("EnemySquad").transform;
                allCards[unit.id].transform.localPosition = new Vector3(90, -100 - 280 * (enemySquad.Count - 1), 150);
                allCards[unit.id].transform.localScale = new Vector3(0.66f, 0.66f , 0.66f);
                allCards[unit.id].GetComponent<UnitCardFight>().stats = unit;
            }
        }
    }    

    public struct attackData
    {
        public int targetId;
        public int damage;
        public Entity target;
        public Vector3 position;
        public string effect;
    }
    public void DisplayDamage ()
    {
        foreach(var attack in Attacks)
        {
            UnitCardFight affectedCard = SquadsManagement.instance.allCards[attack.targetId].GetComponent<UnitCardFight>();
            if (affectedCard.UpdateData(attack.target, attack.damage))
            {
                if (userSquad.ContainsKey(attack.targetId))
                {
                    userSquad.Remove(attack.targetId);
                    userScrollListContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, userSquad.Count * 280);
                    foreach (var card in SquadsManagement.instance.allCards)
                    {
                        if (userSquad.ContainsKey(card.Key) && card.Value.transform.localPosition.y < allCards[attack.targetId].transform.localPosition.y)
                        {
                            card.Value.transform.localPosition -= new Vector3(0, 280, 0);
                        }
                    }
                }
                else
                {
                    enemySquad.Remove(attack.targetId);
                    enemyScrollListContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, enemySquad.Count * 280);
                    foreach (var card in SquadsManagement.instance.allCards)
                    {
                        if (enemySquad.ContainsKey(card.Key)&&card.Value.transform.localPosition.y<allCards[attack.targetId].transform.localPosition.y)
                        {
                            card.Value.transform.localPosition -= new Vector3(0,280,0);
                        }
                    }
                }
                Destroy(SquadsManagement.instance.allCards[attack.targetId]);
                SquadsManagement.instance.allCards.Remove(attack.targetId);
            }
            CreatePopUp(attack.damage, attack.position);
            CreatePopUp(attack.effect, attack.position);
        }
    }
    private void CreatePopUp(int damage, Vector3 target)
    {
        var instance = GameObject.Instantiate(damagePrefab);
        instance.transform.SetParent(GameObject.Find("CloseView").transform);
        instance.transform.localPosition = target + new Vector3(-650,-400,0);
        instance.transform.localScale = new Vector3(4,4,1);
        instance.GetComponent<TMPro.TextMeshProUGUI>().text = "-" + damage.ToString();
    }
    private void CreatePopUp(string effect, Vector3 target)
    {
        var instance = GameObject.Instantiate(damagePrefab);
        instance.transform.SetParent(GameObject.Find("CloseView").transform);
        instance.transform.localPosition = target + new Vector3(-450, -400, 0);
        instance.transform.localScale = new Vector3(4, 4, 1);
        instance.GetComponent<TMPro.TextMeshProUGUI>().text = effect;
    }


    public void MarkActiveUnit(int id)
    {
        GameObject.Destroy(GameObject.Find("ActiveUnitCardGlow"));
        var instance = GameObject.Instantiate(activePrefab);
        instance.name = "ActiveUnitCardGlow";
        instance.transform.SetParent(SquadsManagement.instance.allCards[id].transform);
        instance.transform.localPosition = new Vector3(18,-260,-228);
        instance.transform.localRotation = Quaternion.Euler(-90,0,0);
        instance.transform.localScale = new Vector3(250,215,398);
    }

}
