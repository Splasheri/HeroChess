using Unity.Entities;
[System.Serializable]
public struct View : IComponentData
{
    public int state;
    public int frame;
}