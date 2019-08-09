using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

[DisableAutoCreation]
public class ClearShit : ComponentSystem
{
    protected override void OnCreate()
    {        
        this.Enabled = false;
    }
    protected override void OnUpdate()
    {
        EntityQuery eq = GetEntityQuery(typeof(Id));
        if (eq.CalculateLength()>0)
        {
            var allE = eq.ToEntityArray(Allocator.TempJob);
            foreach(var E in allE)
            {
                PostUpdateCommands.DestroyEntity(E);
            }
            allE.Dispose();
        }
    }
}