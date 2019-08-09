using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using DragonBones;

public class createCloseUp : MonoBehaviour
{
    public static createCloseUp instance
    {
        get
        {
            return manager;
        }
    }
    private static createCloseUp manager;

    public List<UnityArmatureComponent> armatures;
    public int countOfComplete;
    public bool startDamageAnimation;

    private void Start()
    {
        manager = this;
        armatures = new List<UnityArmatureComponent>();
        startDamageAnimation = false;
        foreach (var unit in Spawner.instance.unitNames)
        {
            UnityFactory.factory.LoadDragonBonesData("Units/ske/" + unit + "_ske",unit);
            UnityFactory.factory.LoadTextureAtlasData("Units/tex/" + unit + "_tex",unit,1,true);
        }
    }

    public static void CreateAnimation(int unitType, Vector3 transType, int team)
    {
        createCloseUp.instance.armatures.Add(
            UnityFactory.factory.BuildArmatureComponent(Spawner.instance.unitNames[unitType], Spawner.instance.unitNames[unitType],"", Spawner.instance.unitNames[unitType],null,true)
            );
        UnityArmatureComponent curentArmature = createCloseUp.instance.armatures[createCloseUp.instance.armatures.Count - 1];
        curentArmature.gameObject.transform.SetParent(GameObject.Find("CloseScene").transform);
        curentArmature.gameObject.transform.localPosition = transType;
        curentArmature.AddDBEventListener(EventObject.COMPLETE, createCloseUp.DeleteOnEnd);
        curentArmature.gameObject.transform.localScale = new Vector3(100 * team, 100, 1);
        if (team>0)
        {
            GameObject.Find("BoardView").GetComponent<MeshRenderer>().enabled = true;
            animationManager.instance.CreateUniqueAnimationSequence(
                new animationManager.particleAnimation() { name = "SceneEffect", position = new Vector3(8,8,-65)},
                new animationManager.particleAnimation() { name = "MoveSlide", position = new Vector3(1480, 300, -0.3f), rotation = new Vector3(180, 90, 180), scale = new Vector3(150, 750, 330f), parent = GameObject.Find("CloseScene").transform },
                new animationManager.particleAnimation() { name = "AttackAnimation", position = new Vector3(990, 211, -0.3f), scale = new Vector3(1500,875, 180), parent = GameObject.Find("CloseScene").transform }
                );
            animationManager.instance.TryPlayUniqueAnimation("SceneEffect");
            animationManager.instance.TryPlayUniqueAnimation("MoveSlide");
            curentArmature.animation.timeScale = 1.25f;
            curentArmature.animation.Play(createCloseUp.instance.GetRandomAttack(curentArmature.animation.animationNames));
        }        
    }

    private static void DeleteOnEnd(string type, EventObject eventObject)
    {
        createCloseUp.instance.countOfComplete++;
    }

    private void Update()
    {
        if (armatures.Count>0 && armatures[armatures.Count - 1].gameObject.transform.localPosition.x > 1100)
        {
            armatures[armatures.Count - 1].gameObject.transform.localPosition += new Vector3(-700*Time.deltaTime, 0f, 0);
            startDamageAnimation = true;
        }
        else
        {
            if (startDamageAnimation)
            {
                for (int i = 0; i < armatures.Count - 1; i++)
                {
                    if (string.IsNullOrEmpty(armatures[i].animationName))
                    {
                        armatures[i].animation.Play("Damage");
                    }
                }
                startDamageAnimation = false;
                animationManager.instance.TryPlayUniqueAnimation("AttackAnimation");
            }
        }
        if (countOfComplete>0&&countOfComplete==armatures.Count)
        {
            foreach(var armature in armatures)
            {
                Destroy(armature.gameObject);
            }
            createCloseUp.instance.countOfComplete = 0;
            armatures.Clear();
            EntityQuery actorQuery = World.Active.EntityManager.CreateEntityQuery(typeof(WaitForAttackAnimEnd));
            var actor = actorQuery.ToEntityArray(Unity.Collections.Allocator.TempJob);
            EntityQuery targetQuery = World.Active.EntityManager.CreateEntityQuery(typeof(Target), typeof(Unit));
            var targets = targetQuery.ToEntityArray(Unity.Collections.Allocator.TempJob);
            foreach (var target in targets)
            {
                World.Active.EntityManager.RemoveComponent<Target>(target);
            }
            GameObject.Find("BoardView").GetComponent<MeshRenderer>().enabled = false;
            animationManager.instance.TryStopUniqueAnimationSeuqence("SceneEffect", "MoveSlide","AttackAnimation");
            World.Active.EntityManager.RemoveComponent<WaitForAttackAnimEnd>(actor[0]);
            animationManager.instance.ShowMassAnimation("UnitCircle");
            actor.Dispose();
            targets.Dispose();
        }
    }
    private string GetRandomAttack(List<string> attackNames)
    {
        List<string> randomizerList = new List<string>();
        foreach (var name in attackNames)
        {
            if (name.Contains("Attack")||name.Contains("attack"))
            {
                randomizerList.Add(name);
            }
        }        
        if (randomizerList.Count==0)
        {
            return attackNames[0];
        }
        else
        {
            return randomizerList[Random.Range(0, randomizerList.Count - 1)];
        }
    }
}

