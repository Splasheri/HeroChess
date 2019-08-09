using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(TakingDamageSystem))]
[UpdateBefore(typeof(NewRoundSystem))]
public class SkillCastSystem : ComponentSystem
{
    EntityManager em;
    protected override void OnCreate()
    {
        em = World.Active.EntityManager;
    }
    protected override void OnUpdate()
    {
        Entities.WithNone<Cooldown, UnitAnimation>().WithAll<Unit, Action>().ForEach
            (
                (Entity id, ref Skill s) =>
                {
                    PostUpdateCommands.AddComponent<UnitAnimation>(id, new UnitAnimation());
                    PostUpdateCommands.RemoveComponent<Action>(id);
                    animationManager.instance.currentEntity = id;
                    //animationManager.instance.ShowSkillScroll(s.skill.ToString());
                    Spawner.publicSP.chooseSkill(s.skill,EntityManager,PostUpdateCommands,id);
                }
            );
    }
}
