using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SkillAnimationHandler : MonoBehaviour
{
    public new string name;
    public bool isMass;
    private void Update()
    {
        if (GameObject.Find(name+"(Clone)")==null)
        {
            animationManager.instance.RemoveFromList(name, isMass);
            Destroy(this.gameObject);
        }
    }
}
