using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public AIObject boss;
    public UnityEvent GameOverEvent;


    bool once = false;

    private void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<AIObject>();
    }

    public void GameOver()
    {
        GameOverEvent.Invoke();
        //StartCoroutine(goToMainMenu());
    }

    IEnumerator goToMainMenu()
    {
        yield return new WaitForSeconds(10.0f);
        SceneManager.LoadScene(0);
    }

}
