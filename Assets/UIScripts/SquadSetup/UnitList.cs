using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UnitList
{
   public static void LoadPanel()
    {
        GameObject holder = GameObject.Find("HeroGroup");
        GameObject hero = Resources.Load<GameObject>("Prefabs/HeroObject") as GameObject;
        List<SquadManager.UnitStats> Units = GameManager.availableUnits;
        for (int i = 0; i < Units.Count; i++)
        {
            var instance = GameObject.Instantiate(hero,new Vector3(0,0,0), Quaternion.identity);
            instance.transform.localPosition = new Vector3(120 + (i % 3) * 400f, -150 + (i / 3) * -300f, 0);
            instance.transform.SetParent(holder.transform, false);
            holder.GetComponent<RectTransform>().sizeDelta = new Vector2(holder.GetComponent<RectTransform>().sizeDelta.x, 300*(i%3));
            Sprite icon = Resources.Load<Sprite>("Units/fig/"+Units[i].fileName) as Sprite;
            instance.transform.GetChild(4).gameObject.GetComponent<Image>().sprite = icon;
            instance.transform.GetChild(5).GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = Units[i].initiative.ToString();
            instance.transform.GetChild(6).GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = Units[i].hp.ToString();
            instance.transform.GetChild(7).GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = Units[i].amountofdice.ToString();
            instance.transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = Units[i].cost.ToString();
            instance.GetComponent<BuyUnit>().name = Units[i].fileName;
        }
        GameObject.Find("SelectHeroPanel").SetActive(false);
    }
    public static void LoadUserSquad(int startInt = 0)
    {
        GameObject holder = GameObject.Find("UserSquad");
        GameObject hero = Resources.Load<GameObject>("Prefabs/HeroObject") as GameObject;
        List<SquadManager.UnitStats> Units = GameManager.userSquad;
        for (int i = startInt; i < Units.Count; i++)
        {
            var instance = GameObject.Instantiate(hero, new Vector3(0, 0, 0), Quaternion.identity);
            instance.transform.localPosition = new Vector3(225, -180 + i  * -420, 1);
            instance.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            instance.transform.SetParent(holder.transform, false);
            holder.GetComponent<RectTransform>().sizeDelta = new Vector2(holder.GetComponent<RectTransform>().sizeDelta.x, 630 * i );
            Sprite icon = Resources.Load<Sprite>("Units/fig/" + Units[i].fileName) as Sprite;
            instance.transform.GetChild(4).gameObject.GetComponent<Image>().sprite = icon;
            instance.transform.GetChild(5).GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = Units[i].initiative.ToString();
            instance.transform.GetChild(6).GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = Units[i].hp.ToString();
            instance.transform.GetChild(7).GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = Units[i].amountofdice.ToString();
            instance.transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = Units[i].cost.ToString();
            instance.GetComponent<BuyUnit>().name = Units[i].fileName;
        }
    }
    public static void LoadUserSquadRec()
    {
        GameObject holder = GameObject.Find("UserSquad");
        GameObject hero = Resources.Load<GameObject>("Prefabs/HeroObjectRec") as GameObject;
        List<SquadManager.UnitStats> Units = GameManager.userSquad;
        for (int i = 0; i < Units.Count; i++)
        {
            var instance = GameObject.Instantiate(hero, new Vector3(0, 0, 0), Quaternion.identity);
            instance.transform.localPosition = new Vector3(225, -180 + i * -420, 1);
            instance.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            instance.transform.SetParent(holder.transform, false);
            holder.GetComponent<RectTransform>().sizeDelta = new Vector2(holder.GetComponent<RectTransform>().sizeDelta.x, 630 * i);
            Sprite icon = Resources.Load<Sprite>("Units/fig/" + Units[i].fileName) as Sprite;
            instance.transform.GetChild(4).gameObject.GetComponent<Image>().sprite = icon;
            instance.transform.GetChild(5).GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = Units[i].initiative.ToString();
            instance.transform.GetChild(6).GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = Units[i].hp.ToString();
            instance.transform.GetChild(7).GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = Units[i].amountofdice.ToString();
            instance.transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = Units[i].cost.ToString();
            instance.GetComponent<UnitPlacer>().Id = i;

        }
    }
}