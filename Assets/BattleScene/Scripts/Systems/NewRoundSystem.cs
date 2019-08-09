using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System.Collections.Generic;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(SkillCastSystem))]
[UpdateBefore(typeof(AvailableCellsSystem))]
public class NewRoundSystem : ComponentSystem
{
    int turnCounter;
    const int amountTurnsToDraw = 50;
    EntityQuery readyCheck, updateQuery;
    EntityQueryDesc readyQueue;
    protected override void OnCreate()
    {
        turnCounter = 0;
        readyQueue = new EntityQueryDesc{
            None = new ComponentType[]{typeof(Dead), typeof(Action)},
            All  = new ComponentType[]{typeof(Unit), typeof(ReadyToAction)}
        };
    }
    protected override void OnUpdate()
    {
        if (turnCounter==amountTurnsToDraw)
        {
            Debug.Log("Draw");
            World.GetExistingSystem(typeof(InitiativeSystem)).Enabled = false;
            this.Enabled = false;
        }
        readyCheck = GetEntityQuery(readyQueue);
        updateQuery  = GetEntityQuery(typeof(Unit));
        if (readyCheck.CalculateLength()==0)
        {
            turnCounter++;
            var updateArray = updateQuery.ToEntityArray(Allocator.TempJob);
            for (int i = 0; i < updateArray.Length; i++)
            {
                PostUpdateCommands.AddComponent<ReadyToAction>(updateArray[i], new ReadyToAction(){});
            }
            updateArray.Dispose();
        }
    }
}