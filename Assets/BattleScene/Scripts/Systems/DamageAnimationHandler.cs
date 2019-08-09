using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(AttackSystem))]
public class DamageAnimationHandler : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<TakinDamage>().ForEach
            (
                (Entity id)=>
                    {
                       Entities.WithAll<AttackEffect>().ForEach
                            (
                                (Entity effect) =>
                                {
                                    PostUpdateCommands.AddComponent(effect, new View() { state = 0, frame = 0 });
                                }
                            );
                        PostUpdateCommands.DestroyEntity(id);
                    }
            );
            
    }
}
