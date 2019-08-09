using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelChangingScene : MonoBehaviour
{
    public FromMainMenuToSquadAssemble loader;

    public void OnClick()
    {
        loader.readyToChange = false;
    }
}
