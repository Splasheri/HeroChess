using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System.Collections.Generic;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(UpdateCells))]
[UpdateBefore(typeof(MotionSystem))]
public class MoveSystem : ComponentSystem
{    
    EntityQuery action,moveCheck,chosenCell;
    protected override void OnUpdate()
    {
        action = GetEntityQuery(typeof(Action), typeof(BoardPosition),typeof(Translation));
        moveCheck = GetEntityQuery(typeof(OneMoreAttack));
        chosenCell = GetEntityQuery(typeof(Cell),typeof(Chosen));
        var actor             = action.ToEntityArray(Allocator.TempJob);
        var actorPosition     = action.ToComponentDataArray<BoardPosition>(Allocator.TempJob);
        var actorTranslation  = action.ToComponentDataArray<Translation>(Allocator.TempJob);
        var cell = chosenCell.ToEntityArray(Allocator.TempJob);        
        if (action.CalculateLength()==1)
        {
            if (chosenCell.CalculateLength()==1)
            {
                PostUpdateCommands.AddComponent<Occupied>(cell[0], new Occupied(){});
                PostUpdateCommands.RemoveComponent<Chosen>(cell[0]);                
                PostUpdateCommands.SetComponent(actor[0], new Move() { index = World.Active.EntityManager.GetComponentData<Move>(actor[0]).index, moveTarget = new float3((actorPosition[0].cell % 8) * 2.25f, (actorPosition[0].cell / 8) * 2.25f, 10) });
                PostUpdateCommands.AddComponent<MoveAnimation>(actor[0], new MoveAnimation() { });
                PostUpdateCommands.RemoveComponent<Action>(actor[0]);
            }
        }
        actor.Dispose();
        actorPosition.Dispose();
        actorTranslation.Dispose();
        cell.Dispose();
    }
}
