using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoad : MonoBehaviour
{
    public Image bar;

    void Start()
    {   
        StartCoroutine(loadAsync(SceneToLoadPersistant.sceneToLoadInto));
    }


    public IEnumerator loadAsync(int level)
    {
        AsyncOperation gamelevel = SceneManager.LoadSceneAsync(level);

        while (!gamelevel.isDone)
        {
            bar.fillAmount = gamelevel.progress;

            yield return null;
        }

        yield return null;
    }
}
