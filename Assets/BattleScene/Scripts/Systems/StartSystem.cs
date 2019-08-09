using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System.Collections.Generic;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(InitiativeSystem))]
public class StartSystem : ComponentSystem
{
    private List<Material> figures;
    protected override void OnCreate()
    {
        figures = new List<Material>();
    }
    protected override void OnUpdate()
    {
        EntityQuery startCheck = GetEntityQuery(typeof(startFlag));
        if (startCheck.CalculateLength()==1)
        {
            Material gpmat = Resources.Load<Material>("BestMaterial");
            EntityQuery unitQuery  = GetEntityQuery(typeof(Unit), typeof(BoardPosition), typeof(UnitType));
            EntityQuery cellQuery  = GetEntityQuery(typeof(Cell));    
            NativeArray<Entity>   startFlagEntity  = startCheck.ToEntityArray(Allocator.TempJob);    
            NativeArray<Entity>   cellEntity  = cellQuery.ToEntityArray(Allocator.TempJob);
            NativeArray<Cell>     cells       = cellQuery.ToComponentDataArray<Cell>(Allocator.TempJob);
            NativeArray<BoardPosition> unitsPosition = unitQuery.ToComponentDataArray<BoardPosition>(Allocator.TempJob);
            for (int j = 0; j < unitsPosition.Length; j++)
            {
                for (int i = 0; i < cellEntity.Length; i++)
                {                       
                    if (unitsPosition[j].cell == cells[i].number)
                    {
                        PostUpdateCommands.AddComponent<Occupied>(cellEntity[i], new Occupied() { });
                    }
                }
            }
            PostUpdateCommands.DestroyEntity(startFlagEntity[0]);
            startFlagEntity.Dispose();
            cellEntity.Dispose();
            cells.Dispose();
            unitsPosition.Dispose();
        }
    }
}
