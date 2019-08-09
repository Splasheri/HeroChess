using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBinom : MonoBehaviour
{
    public int value;

    public void OnCLick()
    {
        GameManager.ChangeBinom(value);
    }
}
