using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System.Collections.Generic;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(FreeActorCell))]
[UpdateAfter(typeof(RenderSystem))]
public class InitiativeSystem : ComponentSystem
{
    EntityQuery actionCheck, readyQuery;
    int currentInitiative;
    EntityQueryDesc actorQuery, readyQueue;
    protected override void OnCreate()
    {
        currentInitiative = 99;
        readyQueue = new EntityQueryDesc
        {
            None = new ComponentType[] { typeof(Dead), typeof(Action) },
            All = new ComponentType[] { typeof(Unit), typeof(ReadyToAction), typeof(Initiative), typeof(UnitType), typeof(Translation), typeof(Team)}
        };
        actorQuery = new EntityQueryDesc
        {
            Any = new ComponentType[] { typeof(Action), typeof(MoveAnimation), typeof(UnitAttack), typeof(WaitForAttackAnimEnd), typeof(UnitAnimation), typeof(DamageDealing) },
        };
        this.Enabled = false;
    }

    protected override void OnUpdate()
    {
        actionCheck = GetEntityQuery(actorQuery);
        readyQuery = GetEntityQuery(readyQueue);
        if (actionCheck.CalculateLength() == 0 && readyQuery.CalculateLength() != 0)
        {
            int pret = -1;
            int number = 1;
            var initiatives = readyQuery.ToComponentDataArray<Initiative>(Allocator.TempJob);
            var type = readyQuery.ToComponentDataArray<UnitType>(Allocator.TempJob);
            var entities = readyQuery.ToEntityArray(Allocator.TempJob);
            for (int i = 0; i < initiatives.Length; i++)
            {
                if (initiatives[i].value <= currentInitiative && initiatives[i].value > pret)
                {
                    pret = initiatives[i].value;
                    number = i;
                }
            }
            PostUpdateCommands.RemoveComponent<ReadyToAction>(entities[number]);
            PostUpdateCommands.AddComponent<Action>(entities[number], new Action() { });
            var pos = EntityManager.GetComponentData<Translation>(entities[number]);
            animationManager.instance.DestroyMassAnimationInstance(new animationManager.particleAnimation() { name = "UnitCircle", position = new Vector3(pos.Value.x, pos.Value.y, -30) });
            animationManager.instance.CreateMassAnimationName(new animationManager.particleAnimation() { name = "UnitCircle", position = new Vector3(pos.Value.x, pos.Value.y, -30) }, EntityManager.GetSharedComponentData<Team>(entities[number]).value, true);

            //UnityEngine.Debug.Log(entities[number]);
            Entities.WithAll<CloseUp>().ForEach(
                    (Entity id)=>
                        {
                            PostUpdateCommands.SetComponent(id, new View() { state = 0, frame = 0});
                            PostUpdateCommands.SetComponent(id, new UnitType() {index = type[number].index});
                        }
                );
            type.Dispose();
            initiatives.Dispose();
            entities.Dispose();
        }
    }
}
