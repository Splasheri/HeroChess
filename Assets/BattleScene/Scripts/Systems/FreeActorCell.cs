using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System.Collections.Generic;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(InitiativeSystem))]
[UpdateBefore(typeof(TakingDamageSystem))]
public class FreeActorCell : ComponentSystem
{
    EntityQuery occupiedCells, activeUnit;
    protected override void OnUpdate()
    {   
        occupiedCells = GetEntityQuery(typeof(Occupied),typeof(Cell));
        activeUnit = GetEntityQuery(typeof(Action),typeof(BoardPosition));
        if (activeUnit.CalculateLength()==1)
        {
            var cells        = occupiedCells.ToComponentDataArray<Cell>(Allocator.TempJob);
            var cell         = occupiedCells.ToEntityArray(Allocator.TempJob);
            var unitPosition = activeUnit.ToComponentDataArray<BoardPosition>(Allocator.TempJob);
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i].number==unitPosition[0].cell)
                {
                    PostUpdateCommands.RemoveComponent<Occupied>(cell[i]);        
                    break;
                }
            }
            cell.Dispose();
            cells.Dispose();
            unitPosition.Dispose();
        }
    }
}
