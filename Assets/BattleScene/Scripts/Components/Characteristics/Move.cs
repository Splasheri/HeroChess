using Unity.Entities;
[System.Serializable]
public struct Move : IComponentData
{
    public int index;
    public Unity.Mathematics.float3 moveTarget;
}
