using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(MotionSystem))]
[UpdateBefore(typeof(DamageAnimationHandler))]
public class AttackSystem : ComponentSystem
{
    EntityQueryDesc unitQuery;
    EntityArchetype closeUpAttack, closeUpTarget, attackEffect;
    protected override void OnCreate()
    {
        unitQuery = new EntityQueryDesc
        {
            None = new ComponentType[] { typeof(UnitAttack) },
            All = new ComponentType[] { typeof(Unit), typeof(BoardPosition), typeof(Team), typeof(HP), typeof(UnitType), typeof(Id) }
        };
        closeUpAttack = World.Active.EntityManager.CreateArchetype
            (
                typeof(Translation),
                typeof(CloseUp),
                typeof(View),
                typeof(UnitType)
            );
        closeUpTarget = World.Active.EntityManager.CreateArchetype(
                typeof(Translation),
                typeof(View),
                typeof(UnitType),
                typeof(CloseUp),
                typeof(Target)
            );
        attackEffect = World.Active.EntityManager.CreateArchetype(
                typeof(Translation),
                typeof(AttackEffect)
            );
    }
    protected override void OnUpdate()
    {
        var attacker = GetEntityQuery(typeof(UnitAttack), typeof(Attack), typeof(Team), typeof(BoardPosition), typeof(UnitType));
        var units    = GetEntityQuery(unitQuery);
        bool didHit = false;
        if (attacker.CalculateLength()==1)
        {
            var attackerEntity = attacker.ToEntityArray(Allocator.TempJob);
            units.SetFilter(new Team() { value = EntityManager.GetSharedComponentData<Team>(attackerEntity[0]).value * -1 });
            var target = units.ToEntityArray(Allocator.TempJob);
            var targetHP = units.ToComponentDataArray<HP>(Allocator.TempJob);
            var targetId = units.ToComponentDataArray<Id>(Allocator.TempJob);
            var targetPosition = units.ToComponentDataArray<BoardPosition>(Allocator.TempJob);
            var targetType = units.ToComponentDataArray<UnitType>(Allocator.TempJob);
            var attackType = attacker.ToComponentDataArray<Attack>(Allocator.TempJob);
            var attackerPos = attacker.ToComponentDataArray<BoardPosition>(Allocator.TempJob);
            float shift = -1;
            List<SquadsManagement.attackData> attacks = new List<SquadsManagement.attackData>();
            int damage = 0;
            if (EntityManager.HasComponent<ExperiencedFighter>(attackerEntity[0]))
            {
                damage = DiceSystem.RollBunchOfDice(attackType[0].amountOfCubes, attackType[0].typeOfCubes, true);
            }
            else
            {
                damage = DiceSystem.RollBunchOfDice(attackType[0].amountOfCubes, attackType[0].typeOfCubes);
            }
            for (int i = 0; i < units.CalculateLength(); i++)
            {
                /*ATTACK DAMAGE DEAL*/
                if (AttackPatterns.chooseType(attackType[0].index, attackerPos[0].cell, targetPosition[i].cell) || (EntityManager.HasComponent<PizarroStrike>(attackerEntity[0])&& EntityManager.HasComponent<Curse>(target[0])))
                {

                    didHit = true;
                    PostUpdateCommands.AddComponent(target[i], new Target() { });
                    /*close-up targets creation*/
                    float delta = 0 + 1.5f * (shift + 1) * (shift % 2 == 0 ? 1 : -1);
                    var e = PostUpdateCommands.CreateEntity(closeUpTarget);
                    PostUpdateCommands.SetComponent(e, new View() { state = 2/*damage*/, frame = 0 });
                    PostUpdateCommands.SetComponent(e, new UnitType() { index = targetType[i].index });
                    PostUpdateCommands.SetComponent(e, new Translation() { Value = new float3(1030f + -100 * Mathf.Abs(delta), 180 + delta * 100, (4 + delta) * 0.1f) });

                    shift += 0.6f;
                    if (EntityManager.HasComponent<Evasion>(target[i]))
                    {
                        damage = AttackPatterns.Evasion(damage);
                    }
                    if (EntityManager.HasComponent<Bleeding>(target[i]))
                    {
                        damage = EntityManager.GetComponentData<Bleeding>(target[i]).damage;
                    }
                    if (EntityManager.HasComponent<BrokenArmour>(target[i]) || EntityManager.HasComponent<DestroyedArmour>(target[i]))
                    {
                        damage *= 2;
                    }

                    string effect = AttackPatterns.chooseEffect(out damage, attackType[0].effect, EntityManager, PostUpdateCommands, attackerEntity[0], target[i], damage);
                    PostUpdateCommands.SetComponent<HP>(target[i], new HP() { currentValue = targetHP[i].currentValue-damage, startValue = targetHP[i].startValue});
                    SquadsManagement.instance.allCards[targetId[i].value].GetComponent<UnitCardFight>().UpdateData(target[i], damage);


                    attacks.Add(new SquadsManagement.attackData()
                    {
                        targetId = targetId[i].value,
                        damage = damage,
                        target = target[i],
                    position = new float3(1030f + -100 * Mathf.Abs(delta), 180 + delta * 100, (4 + delta) * 0.1f)
                    });
                }
                SquadsManagement.instance.Attacks = attacks;
            }
            if (didHit == true)
            {
                /*close-up view creation*/
                var e = PostUpdateCommands.CreateEntity(closeUpAttack);
                PostUpdateCommands.SetComponent(e, new Translation() { Value = new float3(1480, 180f, 1f) });
                PostUpdateCommands.SetComponent(e, new View() { state = 1/*attack*/ });
                PostUpdateCommands.SetComponent(e, new UnitType() { index = EntityManager.GetComponentData<UnitType>(attackerEntity[0]).index });
                PostUpdateCommands.AddComponent(attackerEntity[0], new UnitAnimation() {});                
            }
            else
            {
                Translation pos = EntityManager.GetComponentData<Translation>(attackerEntity[0]);
                animationManager.instance.DestroyMassAnimationInstance(new animationManager.particleAnimation() { name = "UnitCircle", position = new Vector3(pos.Value.x, pos.Value.y, -30) });
                animationManager.instance.CreateMassAnimationName(new animationManager.particleAnimation() { name = "UnitCircle", position = new Vector3(pos.Value.x, pos.Value.y, -30) }, EntityManager.GetSharedComponentData<Team>(attackerEntity[0]).value);
            }
            PostUpdateCommands.RemoveComponent<UnitAttack>(attackerEntity[0]);
            target.Dispose();
            attackerEntity.Dispose();
            targetHP.Dispose();
            targetId.Dispose();
            targetPosition.Dispose();
            targetType.Dispose();
            attackType.Dispose();
            attackerPos.Dispose();
        }
    }
}
