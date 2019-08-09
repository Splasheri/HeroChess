using Unity.Entities;
[System.Serializable]
public struct HP : IComponentData
{
    public int startValue;
    public int currentValue;
}
