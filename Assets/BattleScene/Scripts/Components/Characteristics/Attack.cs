using Unity.Entities;
[System.Serializable]
public struct Attack : IComponentData
{
    public int index;
    public int amountOfCubes;
    public int typeOfCubes;
    public AttackPatterns.attackEffect effect;
}
