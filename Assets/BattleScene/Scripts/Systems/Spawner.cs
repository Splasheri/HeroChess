using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;
public class Spawner : MonoBehaviour
{
    private static Spawner spawner;
    public static Spawner instance
    {
        get
        {
            return spawner;
        }
    }
    private static SkillPatterns sp;
    public static SkillPatterns publicSP
    {
        get
        {
            return sp;
        }
    }
    public List<string> unitNames;
    public List<SquadManager.UnitStats> userSquad;
    public List<SquadManager.UnitStats> enemySquad;
    public EntityArchetype unitArchetype, cellArchetype;
    public int currentId
    {
        get
        {
            return id;
        }
    }
    private int id;
    private Mesh _mesh;
    private string charactersFile;
    private string[] characters;
    public List<SquadManager.UnitStats> allChars;
    private GameObject endGameScreen;
    void Awake()
    {
        var w = new World("world");
        DefaultWorldInitialization.Initialize("world", false);
        unitNames = new List<string>();
        spawner = this;
        Material gpmat = Resources.Load<Material>("BestMaterial");
        var entityManager = World.Active.EntityManager;
        id = 0;
        sp = new SkillPatterns();
        unitArchetype = entityManager.CreateArchetype(
            typeof(Id),
            typeof(Lvl),
            typeof(Translation),
            typeof(Unit),
            typeof(UnitType),
            typeof(HP),
            typeof(Team),
            typeof(Initiative),
            typeof(Attack),
            typeof(Move),
            typeof(Skill),
            typeof(Cost),
            typeof(ReadyToAction),
            typeof(BoardPosition)
        );
        cellArchetype = entityManager.CreateArchetype(
            typeof(Cell),
            typeof(Translation)
        );
        endGameScreen = Resources.Load<GameObject>("Prefabs/EndGame");
    }

    private void Start()
    {        
        var entityManager = World.Active.EntityManager;
        StartFight(entityManager);
        SquadsManagement.instance.CreateSquads();
    }

    public static Mesh CreateQuadMesh(float height, float width)
    {
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];
        vertices[0] = new Vector3(-.5f * width, -.5f * height);
        vertices[1] = new Vector3(-.5f * width, +.5f * height);
        vertices[2] = new Vector3(+.5f * width, +.5f * height);
        vertices[3] = new Vector3(+.5f * width, -.5f * height);

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;
        triangles[3] = 1;
        triangles[4] = 2;
        triangles[5] = 3;
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        return mesh;
    }

    public Entity CreateUnit(EntityManager entityManager, SquadManager.UnitStats stats)
    {
        Entity instance = entityManager.CreateEntity(unitArchetype);
        entityManager.SetComponentData(instance, new Id() { value = stats.id });
        entityManager.SetSharedComponentData(instance, new Team() { value = stats.team });
        entityManager.SetComponentData(instance, new Lvl() { value = stats.lvl});
        entityManager.SetComponentData(instance, new Translation() { Value = new float3( (stats.position % 8)*2.25f, (stats.position / 8)*2.25f , 0.4f) });
        entityManager.SetComponentData(instance, new HP() { startValue = stats.hp, currentValue = stats.hp});
        entityManager.SetComponentData(instance, new Initiative() { value = stats.initiative });
        entityManager.SetComponentData(instance, new Attack() { index = stats.attacktype, amountOfCubes = stats.amountofdice, typeOfCubes = stats.dicetype, effect = stats.attackEffect});
        entityManager.SetComponentData(instance, new Move() { index = stats.movetype });
        entityManager.SetComponentData(instance, new Cost() { value = stats.cost });
        entityManager.SetComponentData(instance, new BoardPosition() { cell = stats.position });
        entityManager.SetComponentData(instance, new Skill() { skill = stats.skilltype});
        animationManager.instance.CreateMassAnimationName(new animationManager.particleAnimation() { name = "UnitCircle", position = new Vector3(stats.position % 8 * 2.25f, stats.position / 8 * 2.25f, -30) }, stats.team);
        return instance;
    }

    public void StartFight(EntityManager entityManager)
    {
        for (int i = 0; i < 64; i++)
        {
            var cell = entityManager.CreateEntity(cellArchetype);
            entityManager.SetComponentData(cell, new Cell() { number = i });
            entityManager.SetComponentData<Translation>(cell, new Translation() { Value = new float3((i % 8) * 2.25f, (i / 8) * 2.25f, 15) });
        }
        for (int u = 0; u < GameManager.userSquad.Count; u++)
        {
            SquadManager.UnitStats x = GameManager.userSquad[u];
            x.ChangeId(GameManager.enemySquad.Count + u);
            GameManager.userSquad[u] = x;
            var i = CreateUnit(entityManager, GameManager.userSquad[u]);
            if (this.unitNames.Contains(GameManager.userSquad[u].fileName))
            {
                entityManager.SetComponentData<UnitType>(i, new UnitType() { index = unitNames.IndexOf(GameManager.userSquad[u].fileName) });
            }
            else
            {
                unitNames.Add(GameManager.userSquad[u].fileName);
                entityManager.SetComponentData<UnitType>(i, new UnitType() { index = unitNames.Count - 1 });
            }
        }
        for (int u = 0; u < GameManager.enemySquad.Count; u++)
        {
            SquadManager.UnitStats x = GameManager.enemySquad[u];
            x.ChangeId(u);
            GameManager.enemySquad[u] = x;
            var i = CreateUnit(entityManager, GameManager.enemySquad[u]);
            if (this.unitNames.Contains(GameManager.enemySquad[u].fileName))
            {
                entityManager.SetComponentData<UnitType>(i, new UnitType() { index = unitNames.IndexOf(GameManager.enemySquad[u].fileName) });
            }
            else
            {
                unitNames.Add(GameManager.enemySquad[u].fileName);
                entityManager.SetComponentData<UnitType>(i, new UnitType() { index = unitNames.Count - 1 });
            }
        }
        Entity sF = entityManager.CreateEntity();
        entityManager.AddComponentData<startFlag>(sF, new startFlag() { });
        foreach(var s in World.Active.Systems)
        {
            if (s!=World.Active.GetExistingSystem(typeof(ClearShit)))
            {
                s.Enabled = true;
            }
        }
    }

    public void EndFight(bool win)
    {
        SquadsManagement.instance.enemySquad.Clear();
        SquadsManagement.instance.userSquad.Clear();
        SquadsManagement.instance.allCards.Clear();
        foreach (var s in World.Active.Systems)
        {
            s.Enabled = false;
        }
        foreach (var e in World.Active.EntityManager.GetAllEntities())
        {
            World.Active.EntityManager.DestroyEntity(e);
        }        
        var instance = GameObject.Instantiate(endGameScreen);
        instance.transform.parent = GameObject.Find("Canvas").transform;
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localScale = Vector3.one;
        if (win)
        {
            instance.transform.GetChild(0).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.BinomLevelList[GameManager.currentBinom].ToString();
            instance.transform.GetChild(0).GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.BinomList[GameManager.currentBinom];
            SquadManager.UnitStats reward = GameManager.AllChars[GameManager.rewards[GameManager.BinomList[GameManager.currentBinom]][GameManager.BinomLevelList[GameManager.currentBinom]-1]];
            if (!GameManager.availableUnits.Contains(reward))
            {
                GameManager.availableUnits.Add(reward);
            }
            GameManager.BinomLevelList[GameManager.currentBinom]+=1;            
            GameManager.gold += (GameManager.currentBinom + 1) * 200;
            instance.transform.GetChild(0).GetChild(4).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = reward.fileName;
            instance.transform.GetChild(0).GetChild(4).GetChild(5).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = reward.initiative.ToString();
            instance.transform.GetChild(0).GetChild(4).GetChild(6).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = reward.hp.ToString();
            instance.transform.GetChild(0).GetChild(4).GetChild(7).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = reward.amountofdice.ToString();
            instance.transform.GetChild(0).GetChild(4).GetChild(4).GetComponent<Image>().sprite = Resources.Load<Sprite>("Units/fig/" + reward.fileName);
        }
        else
        {
            instance.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "YOU LOSE";
            instance.transform.GetChild(2).localPosition = new Vector3(-20, 120, 0);
            Destroy(instance.transform.GetChild(0).gameObject);
        }
        spawner.unitNames.Clear();

        GameManager.Save actualSave = new GameManager.Save()
        {
            gold = GameManager.gold,
            levels = GameManager.BinomLevelList.ToArray()
        };
        actualSave.availableUnits = new string[GameManager.availableUnits.Count];
        for (int i = 0; i < actualSave.availableUnits.Length; i++)
        {
            actualSave.availableUnits[i] = GameManager.availableUnits[i].fileName;
        }
        actualSave.squad = new string[GameManager.userSquad.Count];
        for (int i = 0; i < actualSave.squad.Length; i++)
        {
            actualSave.squad[i] = GameManager.userSquad[i].fileName;
        }
        GameManager.Serialize(actualSave, "gameData");
    }
}
