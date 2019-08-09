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
[UpdateBefore(typeof(TakingDamageSystem))]
public class TickEffectSystem : ComponentSystem
{
    EntityQuery units;
    EntityQueryDesc tickEffectsDesc;
    protected override void OnCreate()
    {
        tickEffectsDesc = new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(Action), typeof(Unit), typeof(HP) },
            Any = new ComponentType[] { typeof(Poison), typeof(Stunned), typeof(Bleeding), typeof(DestroyedArmour) }
        };
    }

    protected override void OnUpdate()
    {
        units = GetEntityQuery(tickEffectsDesc);
        var unit = units.ToEntityArray(Allocator.TempJob);
        foreach(var entity in unit)
        {
            if (EntityManager.HasComponent<BadSign>(entity))
            {
                if (DiceSystem.DiceRoll(20)<2)
                {
                    PostUpdateCommands.RemoveComponent<Action>(entity);
                }
                if (EntityManager.GetComponentData<BadSign>(entity).duration == 0)
                {
                    PostUpdateCommands.RemoveComponent<BadSign>(entity);
                }
                else
                {
                    PostUpdateCommands.SetComponent<BadSign>(entity, new BadSign()
                    {
                        duration = EntityManager.GetComponentData<BadSign>(entity).duration - 1
                    });
                }
            }
            if (EntityManager.HasComponent<FishTail>(entity))
            {
                var move = EntityManager.GetComponentData<Move>(entity);
                PostUpdateCommands.SetComponent<Move>(entity, new Move() { index = 0});
            }
            if (EntityManager.HasComponent<Cooldown>(entity))
            {
                if (EntityManager.GetComponentData<Cooldown>(entity).duration == 0)
                {
                    PostUpdateCommands.RemoveComponent<Cooldown>(entity);
                }
                else
                {
                    PostUpdateCommands.SetComponent<Cooldown>(entity, new Cooldown()
                    {
                        duration = EntityManager.GetComponentData<Cooldown>(entity).duration - 1
                    });
                }
            }
            if (EntityManager.HasComponent<Poison>(entity))
            {
                if (EntityManager.GetComponentData<Poison>(entity).duration==0)
                {
                    PostUpdateCommands.RemoveComponent<Poison>(entity);
                }
                else
                {
                    PostUpdateCommands.SetComponent<Poison>(entity, new Poison()
                    {

                        duration = EntityManager.GetComponentData<Poison>(entity).duration - 1
                    });
                    PostUpdateCommands.SetComponent<HP>(entity, new HP()
                    {
                        startValue = EntityManager.GetComponentData<HP>(entity).startValue,
                        currentValue = EntityManager.GetComponentData<HP>(entity).currentValue - 5
                    });
                }
            }
            if (EntityManager.HasComponent<Stunned>(entity))
            {
                if (EntityManager.GetComponentData<Stunned>(entity).duration == 0)
                {
                    PostUpdateCommands.RemoveComponent<Stunned>(entity);
                }
                else
                PostUpdateCommands.SetComponent<Stunned>(entity, new Stunned()
                {
                    duration = EntityManager.GetComponentData<Stunned>(entity).duration - 1
                });
                PostUpdateCommands.RemoveComponent<Stunned>(entity);

            }
            if (EntityManager.HasComponent<Bleeding>(entity))
            {
                PostUpdateCommands.SetComponent<HP>(entity, new HP()
                {
                    startValue = EntityManager.GetComponentData<HP>(entity).startValue,
                    currentValue = EntityManager.GetComponentData<HP>(entity).currentValue - EntityManager.GetComponentData<Bleeding>(entity).damage
                });
            }
            if (EntityManager.HasComponent<DestroyedArmour>(entity)&&!EntityManager.HasComponent<BrokenArmour>(entity))
            {
                PostUpdateCommands.AddComponent<BrokenArmour>(entity, new BrokenArmour() { });
            }
        }
        unit.Dispose();
    }
}
