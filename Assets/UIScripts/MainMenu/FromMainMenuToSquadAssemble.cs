using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FromMainMenuToSquadAssemble : MonoBehaviour
{
    private AsyncOperation asyncLoad;
    public bool readyToChange;
    public GameObject dialog;
    private void Start()
    {
        readyToChange = false;
        StartCoroutine(LoadYourAsyncScene());
    }
    IEnumerator LoadYourAsyncScene()
    {
        asyncLoad = SceneManager.LoadSceneAsync("Scenes/LevelMenu", LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
            if (readyToChange)
            {
                asyncLoad.allowSceneActivation = true;
            }
        }
    }

    public void OnClick()
    {
        readyToChange = true;
        dialog.SetActive(true);
    }
}
