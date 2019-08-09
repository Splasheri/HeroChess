using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct BadSign : IComponentData
{
    public int duration;
}

[Serializable]
public struct MichaelCurse : IComponentData
{
    public int duration;
}
[Serializable]
public struct Curse : IComponentData
{
    public int duration;
}
[Serializable]
public struct Bless : IComponentData
{
    public int duration;
}
[Serializable]
public struct DragonBless : IComponentData
{
}
[Serializable]
public struct XoerCurse : IComponentData
{
    public int duration;
}
[Serializable]
public struct ChilBlain : IComponentData
{
    public int duration;
}

[Serializable]
public struct Amoure : IComponentData
{
    public int indexM;
    public int indexA;
}
[Serializable]
public struct FireShield : IComponentData
{
    public int damage;
}
[Serializable]
public struct Water : IComponentData //CELL COMPONENT
{
}
[Serializable]
public struct ElectricTrap : IComponentData //CELL COMPONENT
{
    public int damage;
}

[Serializable]
public struct Stone : IComponentData //STATE COMPONENT
{
}

[Serializable]
public struct ExperiencedFighter : IComponentData
{
}
[Serializable]
public struct Evasion : IComponentData
{
}
[Serializable]
public struct FesteringWound : IComponentData
{
}
[Serializable]
public struct FishTail : IComponentData
{
}
[Serializable]
public struct Hunter : IComponentData
{
}
[Serializable]
public struct SkillPowerfullWings : IComponentData
{
}
[Serializable]
public struct Rebirth : IComponentData
{
}
[Serializable]
public struct SoulEater : IComponentData
{
}
[Serializable]
public struct PizarroStrike : IComponentData
{
}
public struct MythicalPatron : IComponentData
{
    public int dessaDiceType;
    public int dessaDiceNumber;
    public int dessa_hp;
    public int dessaIndex;
    public int state;
}
