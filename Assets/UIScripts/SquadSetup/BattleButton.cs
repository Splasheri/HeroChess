using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleButton : MonoBehaviour
{
    public void OnCLick(string sceneName)
    {
        StartCoroutine(LoadYourAsyncScene());

        IEnumerator LoadYourAsyncScene()
        {
            var asyncLoad = SceneManager.LoadSceneAsync("Scenes/"+sceneName, LoadSceneMode.Single);
            yield return null;
        }
    }
}
