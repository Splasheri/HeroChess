using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System.Collections.Generic;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(NewRoundSystem))]
[UpdateBefore(typeof(ChooseCellSystem))]
public class AvailableCellsSystem : ComponentSystem
{
    EntityQuery actionQueue, cellQueue;
    EntityQueryDesc cellQuery;
    protected override void OnCreate()
    {
        cellQuery = new EntityQueryDesc
        {
            None = new ComponentType[]{ typeof(Occupied) },
            All = new ComponentType[]{ typeof(Cell)}
        };
    }
    protected override void OnUpdate()
    {
        actionQueue = GetEntityQuery(typeof(Action), typeof(Move), typeof(BoardPosition));
        if (actionQueue.CalculateLength()==1)
        {
            cellQueue = GetEntityQuery(cellQuery);
            var actor = actionQueue.ToEntityArray(Allocator.TempJob);
            PostUpdateCommands.RemoveComponent<UnitAnimation>(actor[0]);
            var position = actionQueue.ToComponentDataArray<BoardPosition>(Allocator.TempJob);
            var move = actionQueue.ToComponentDataArray<Move>(Allocator.TempJob);
            var cells = cellQueue.ToComponentDataArray<Cell>(Allocator.TempJob);
            var entityCells = cellQueue.ToEntityArray(Allocator.TempJob);
            for (int i = 0; i < cells.Length; i++)
            {
                if (MovePatterns.chooseType(move[0].index,position[0].cell,cells[i].number))
                {
                    PostUpdateCommands.AddComponent(entityCells[i],new AvailableCell(){});
                }
                if (EntityManager.HasComponent<Water>(entityCells[i])&&EntityManager.HasComponent<FishTail>(actor[0]))
                {
                    PostUpdateCommands.AddComponent(entityCells[i], new AvailableCell() { });
                }
            }
            position.Dispose();
            move.Dispose();
            cells.Dispose();
            actor.Dispose();
            entityCells.Dispose();
        }
    }
}