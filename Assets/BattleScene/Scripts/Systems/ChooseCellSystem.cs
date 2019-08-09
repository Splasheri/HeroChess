using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System.Collections.Generic;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(AvailableCellsSystem))]
[UpdateBefore(typeof(UpdateCells))]
public class ChooseCellSystem : ComponentSystem
{
    EntityQuery availableCellsQueue, actionQueue, unitsQueue;
    EntityQueryDesc unitQuery;
    protected override void OnCreate()
    {        
        unitQuery = new EntityQueryDesc
        {
            None = new ComponentType[]{ typeof(Action), typeof(Dead) },
            All = new ComponentType[]{ typeof(Unit), typeof(BoardPosition), typeof(Cost), typeof(Team)}
        };
    }
    protected override void OnUpdate()
    {
        availableCellsQueue = GetEntityQuery(typeof(AvailableCell),typeof(Cell));
        actionQueue = GetEntityQuery(typeof(Action), typeof(Attack), typeof(Team), typeof(BoardPosition));        
        unitsQueue   = GetEntityQuery(unitQuery);
        int bestValue = 0;
        int bestCell = 0;
        int bestDistance = int.MaxValue;
        if (actionQueue.CalculateLength()==1&&availableCellsQueue.CalculateLength()>0)
        {
            var attacker = actionQueue.ToEntityArray(Allocator.TempJob);
            unitsQueue.SetFilter(new Team() { value = EntityManager.GetSharedComponentData<Team>(attacker[0]).value * -1 });
            var availableCells = availableCellsQueue.ToEntityArray(Allocator.TempJob);
            var availableCellsNumbers = availableCellsQueue.ToComponentDataArray<Cell>(Allocator.TempJob);
            var attackType = actionQueue.ToComponentDataArray<Attack>(Allocator.TempJob);
            var attackerPosition = actionQueue.ToComponentDataArray<BoardPosition>(Allocator.TempJob);
            var targetPosition = unitsQueue.ToComponentDataArray<BoardPosition>(Allocator.TempJob);
            var targetCost = unitsQueue.ToComponentDataArray<Cost>(Allocator.TempJob);
            for (int i = 0; i < availableCells.Length; i++)
            {
                int value = 0;
                int distance = int.MaxValue;
                for (int j = 0; j < unitsQueue.CalculateLength(); j++)
                {
                        if (AttackPatterns.chooseType(attackType[0].index,availableCellsNumbers[i].number,targetPosition[j].cell))
                        {
                            value+=targetCost[j].value;
                        }
                        if (Mathf.Abs(availableCellsNumbers[i].number-targetPosition[j].cell)<distance)
                        {                            
                            distance = (availableCellsNumbers[i].number % 8 - targetPosition[j].cell % 8) * (availableCellsNumbers[i].number % 8 - targetPosition[j].cell % 8) + (availableCellsNumbers[i].number / 8 - targetPosition[j].cell / 8) * (availableCellsNumbers[i].number / 8 - targetPosition[j].cell / 8);
                        }
                }
                if (value==0&&bestValue==0)
                {
                    if (bestDistance>distance)
                    {
                        bestDistance = distance;
                        bestCell = i;
                    }
                }
                else
                {
                    if (value>=bestValue)
                    {
                        bestValue = value;
                        bestCell = i;
                        
                    }          
                }     
            }
            PostUpdateCommands.AddComponent<Chosen>(availableCells[bestCell],new Chosen(){});
            PostUpdateCommands.SetComponent<BoardPosition>(attacker[0], new BoardPosition(){cell = availableCellsNumbers[bestCell].number});
            attacker.Dispose();
            attackerPosition.Dispose();
            availableCells.Dispose();
            availableCellsNumbers.Dispose();
            attackType.Dispose();
            targetCost.Dispose();
            targetPosition.Dispose();
        }
    }
}