using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct Cooldown : IComponentData
{
    public int duration;   
}
