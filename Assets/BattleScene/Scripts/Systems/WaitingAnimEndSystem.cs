//using Unity.Burst;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Jobs;
//using Unity.Rendering;
//using Unity.Mathematics;
//using Unity.Transforms;
//using UnityEngine;
//using System.Collections.Generic;

//[UpdateInGroup(typeof(SimulationSystemGroup))]
//[UpdateAfter(typeof(UpdateCells))]
//[UpdateBefore(typeof(AttackSystem))]

//public class WaitingAnimEndSystem : ComponentSystem
//{
//    EntityQueryDesc backAnim;
//    protected override void OnCreate()
//    {
//        backAnim = new EntityQueryDesc
//        {
//            None = new ComponentType[] { typeof(UnitType) },
//            All = new ComponentType[] { typeof(CloseUp)}
//        };
//    }
//    protected override void OnUpdate()
//    {
//        EntityQuery backCheck = GetEntityQuery(backAnim);
//        if (backCheck.CalculateLength()1)
//        {
//            EntityQuery actor = GetEntityQuery(typeof(WaitForAttackAnimEnd));
//            var backgroundArr = backCheck.ToEntityArray(Allocator.TempJob);
//            var actorArr = actor.ToEntityArray(Allocator.TempJob);
//            PostUpdateCommands.DestroyEntity(backgroundArr[0]);
//            PostUpdateCommands.RemoveComponent<WaitForAttackAnimEnd>(actorArr[0]);
//            PostUpdateCommands.AddComponent(actorArr[0], new MoveAnimation() { });
//            backgroundArr.Dispose();
//            actorArr.Dispose();
//        }
//    }
//}