using UnityEngine;
using UnityEditor;
using Unity.Entities;

public struct Poison : IComponentData
{
    public int duration;
}