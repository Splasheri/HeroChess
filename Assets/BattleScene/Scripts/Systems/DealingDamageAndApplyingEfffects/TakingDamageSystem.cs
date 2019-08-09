using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System.Collections.Generic;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(FreeActorCell))]
[UpdateBefore(typeof(SkillCastSystem))]
public class TakingDamageSystem : ComponentSystem
{
    EntityQuery activeUnit;
    protected override void OnUpdate()
    {   
        activeUnit = GetEntityQuery(typeof(Action),typeof(HP),typeof(BoardPosition), typeof(Id));
        var unitId = activeUnit.ToComponentDataArray<Id>(Allocator.TempJob);
        if (activeUnit.CalculateLength()==1)
        {
            var actor  = activeUnit.ToEntityArray(Allocator.TempJob);
            var unitHP = activeUnit.ToComponentDataArray<HP>(Allocator.TempJob);
            var unitPosition = activeUnit.ToComponentDataArray<BoardPosition>(Allocator.TempJob);
            if (unitHP[0].currentValue<=0)
            {
                bool readyToDeath = true;
                if (EntityManager.HasComponent<Rebirth>(actor[0]))
                {
                    readyToDeath = false;
                    PostUpdateCommands.RemoveComponent<Rebirth>(actor[0]);
                    PostUpdateCommands.SetComponent<HP>(actor[0], new HP() { startValue = unitHP[0].startValue, currentValue = unitHP[0].startValue});
                }
                if (EntityManager.HasComponent<MythicalPatron>(actor[0])&&!EntityManager.HasComponent<Cooldown>(actor[0]))
                {
                    PostUpdateCommands.AddComponent<Cooldown>(actor[0], new Cooldown() { duration = 2});
                    var attack = EntityManager.GetComponentData<Attack>(actor[0]);
                    var mp = EntityManager.GetComponentData<MythicalPatron>(actor[0]);
                    if (mp.state==0)
                    {
                        readyToDeath = false;
                        PostUpdateCommands.SetComponent<Attack>(actor[0], new Attack() { amountOfCubes = mp.dessaDiceNumber, typeOfCubes = mp.dessaDiceType, index = mp.dessaIndex });
                        PostUpdateCommands.SetComponent<HP>(actor[0], new HP() { startValue = unitHP[0].startValue, currentValue = mp.dessa_hp });
                        PostUpdateCommands.SetComponent<MythicalPatron>(actor[0], new MythicalPatron() { state = 1, dessaDiceNumber = attack.amountOfCubes, dessaDiceType = attack.typeOfCubes, dessa_hp = mp.dessa_hp, dessaIndex = attack.index });
                    }
                }
                if (readyToDeath)
                {
                    animationManager.instance.DestroyMassAnimationInstance(new animationManager.particleAnimation()
                    {
                        name = "UnitCircle",
                        position = new Vector3(unitPosition[0].cell % 8 * 2.25f, unitPosition[0].cell / 8 * 2.25f, -30)
                   });
                    KillUnit(actor[0]);
                }
            }
            else
            {
                //SquadsManagement.instance.MarkActiveUnit(unitId[0].value);
            }
            unitPosition.Dispose();
            actor.Dispose();
            unitHP.Dispose();
        }
        unitId.Dispose();
    }
    private void KillUnit(Entity unit)
    {
        EntityQuery thanatosQ = GetEntityQuery(typeof(SoulEater), typeof(HP), typeof(Attack));
        if (thanatosQ.CalculateLength()>0)
        {
            var thanatosA = thanatosQ.ToEntityArray(Allocator.TempJob);
            foreach (var thanatos in thanatosA)
            {
                var thanatosAttack = EntityManager.GetComponentData<Attack>(thanatos);
                var thanatosHP = EntityManager.GetComponentData<HP>(thanatos);
                PostUpdateCommands.SetComponent<Attack>(thanatos,new Attack() { amountOfCubes = thanatosAttack.amountOfCubes+1, index = thanatosAttack.index, typeOfCubes = thanatosAttack.typeOfCubes});
                PostUpdateCommands.SetComponent<HP>(thanatos, new HP() { startValue = thanatosHP.startValue, currentValue = thanatosHP.currentValue+8});
            }
            thanatosA.Dispose();
            thanatosQ.Dispose();
        }
        int targetId = EntityManager.GetComponentData<Id>(unit).value;
        if (SquadsManagement.instance.userSquad.ContainsKey(targetId))
        {
            SquadsManagement.instance.userSquad.Remove(targetId);
            SquadsManagement.instance.userScrollListContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, SquadsManagement.instance.userSquad.Count * 280);
            foreach (var card in SquadsManagement.instance.allCards)
            {
                if (SquadsManagement.instance.userSquad.ContainsKey(card.Key) && card.Value.transform.localPosition.y < SquadsManagement.instance.allCards[targetId].transform.localPosition.y)
                {
                    card.Value.transform.localPosition += new Vector3(0, 280, 0);
                }
            }
            GameObject.Destroy(SquadsManagement.instance.allCards[targetId]);
        }
        PostUpdateCommands.RemoveComponent<Unit>(unit);
        PostUpdateCommands.RemoveComponent<Action>(unit);
        PostUpdateCommands.AddComponent(unit, new View() { frame = 0, state = 0 });
        PostUpdateCommands.AddComponent(unit, new Dead(){});
        PostUpdateCommands.AddComponent(unit, new UnitAnimation() {});
    }
}
