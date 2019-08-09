//using System.Collections;
//using Unity.Transforms;
//using System.Collections.Generic;
//using Unity.Entities;
//using Unity.Mathematics;
//using UnityEngine;

//[UpdateInGroup(typeof(SimulationSystemGroup))]
//[UpdateAfter(typeof(AttackSystem))]
//[UpdateBefore(typeof(BackGroundTypeCheck))]
//public class AttackBackgroundSystem : ComponentSystem
//{
//    protected override void OnUpdate()
//    {
//        EntityQuery back = GetEntityQuery(typeof(AttackBackground));        
//        if (back.CalculateLength()==1)
//        {
//            var backArr = back.ToEntityArray(Unity.Collections.Allocator.TempJob);
//            GameObject prefab = Resources.Load<GameObject>("Prefabs/Test");
//            Debug.Log(prefab);
//            var e = PostUpdateCommands.Instantiate(GameObjectConversionUtility.ConvertGameObjectHierarchy(prefab,World.Active));
//            PostUpdateCommands.DestroyEntity(backArr[0]);
//            backArr.Dispose();
//            World.GetExistingSystem<BackGroundTypeCheck>().Enabled = true;
//        }
//    }
//}

//[UpdateInGroup(typeof(SimulationSystemGroup))]
//[UpdateAfter(typeof(AttackBackgroundSystem))]
//public class BackGroundTypeCheck :ComponentSystem
//{
//    EntityQueryDesc b = new EntityQueryDesc()
//    {
//        None = new ComponentType[] { typeof(UnitType) },
//        All = new ComponentType[] { typeof(AttackerModel) }

//    };
//    protected override void OnCreate()
//    {
//        Enabled = false;
//    }
//    protected override void OnUpdate()
//    {
//        EntityQuery back = GetEntityQuery(b);
//        EntityQuery actor = GetEntityQuery(typeof(WaitForAttackAnimEnd), typeof(UnitType));
//        if (back.CalculateLength() == 1)
//        {
//            var backArr = back.ToEntityArray(Unity.Collections.Allocator.TempJob);
//            var actorArr = actor.ToComponentDataArray<UnitType>(Unity.Collections.Allocator.TempJob);
//            PostUpdateCommands.AddComponent(backArr[0],new UnitType() {index = actorArr[0].index });
//            Debug.Log("123");
//            backArr.Dispose();
//            actorArr.Dispose();
//            Enabled = false;
//        }
//    }
//}