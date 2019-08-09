using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(ChooseCellSystem))]
[UpdateBefore(typeof(MoveSystem))]
public class UpdateCells : ComponentSystem
{
    protected override void OnUpdate()
    {   
        Entities.WithAll<Cell,AvailableCell>().ForEach
        (
            (Entity id)=>
            {
                PostUpdateCommands.RemoveComponent<AvailableCell>(id);
            }
        );
    }
}