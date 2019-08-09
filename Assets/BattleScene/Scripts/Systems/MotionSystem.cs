using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(AttackSystem))]
[UpdateAfter(typeof(MoveSystem))]
public class MotionSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<MoveAnimation>().ForEach
            (
                (Entity id, ref Move move, ref Translation trans)=>
                    {
                        if ((trans.Value!=move.moveTarget).x || (trans.Value != move.moveTarget).y)
                        {
                            Vector2 newPos = Vector2.MoveTowards(new Vector2(trans.Value.x, trans.Value.y), new Vector2(move.moveTarget.x, move.moveTarget.y), 3f * Time.deltaTime);
                            PostUpdateCommands.SetComponent(id, new Translation() { Value = new Vector3(newPos.x,newPos.y,0.5f)});
                            animationManager.instance.DestroyMassAnimationInstance(new animationManager.particleAnimation() { name = "UnitCircle", position = new Vector3(trans.Value.x, trans.Value.y, -30) });
                            animationManager.instance.CreateMassAnimationName(new animationManager.particleAnimation() { name = "UnitCircle", position = new Vector3(newPos.x, newPos.y, -30) }, EntityManager.GetSharedComponentData<Team>(id).value, true);
                        }
                        else
                        {
                            animationManager.instance.DestroyMassAnimationInstance(new animationManager.particleAnimation() { name = "UnitCircle", position = new Vector3(trans.Value.x, trans.Value.y, -30) });
                            animationManager.instance.CreateMassAnimationName(new animationManager.particleAnimation() { name = "UnitCircle", position = new Vector3(trans.Value.x, trans.Value.y, -30) }, EntityManager.GetSharedComponentData<Team>(id).value, true);                            
                            PostUpdateCommands.RemoveComponent<MoveAnimation>(id);
                            PostUpdateCommands.AddComponent<UnitAttack>(id, new UnitAttack() { });
                        }
                    }
            );
    }
}
