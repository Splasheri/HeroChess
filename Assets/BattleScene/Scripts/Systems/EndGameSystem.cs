using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public class EndGameSystem : ComponentSystem
{
    protected override void OnCreate()
    {
        this.Enabled = false;
    }
    protected override void OnUpdate()
    {
        bool firstTeamExist = false, secondTeamExist = false;
        EntityQuery unitsQuery  = GetEntityQuery(typeof(Unit), typeof(Team));
        unitsQuery.ResetFilter();
        if (unitsQuery.CalculateLength()==0)
        {
            Debug.Log("Draw");
            Spawner.instance.EndFight(true);
        }
        else
        {
            unitsQuery.SetFilter(new Team() { value = 1 });
            if (unitsQuery.CalculateLength() != 0) { firstTeamExist = true; }
            unitsQuery.ResetFilter();
            unitsQuery.SetFilter(new Team() { value = -1});
            if (unitsQuery.CalculateLength() != 0) { secondTeamExist = true; }
            if (firstTeamExist==true&&secondTeamExist==false)
            {
                foreach (var ui in GameObject.FindGameObjectsWithTag("FightLog"))
                {
                    ui.GetComponent<Text>().text = "";
                }
                Debug.Log("You won");
                Spawner.instance.EndFight(true);
            }
            else if (firstTeamExist==false&&secondTeamExist==true)
            {
                foreach (var ui in GameObject.FindGameObjectsWithTag("FightLog"))
                {
                    ui.GetComponent<Text>().text = "";
                }
                Debug.Log("Enemy won");
                Spawner.instance.EndFight(false);
            }
        }
    }
}