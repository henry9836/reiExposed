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

    private void FixedUpdate()
    {
        if (boss.health <= 0.0f)
        {
            if (once)
            {
                once = false;
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        GameOverEvent.Invoke();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        //StartCoroutine(goToMainMenu());
    }

    IEnumerator goToMainMenu()
    {
        yield return new WaitForSeconds(10.0f);
        SceneManager.LoadScene(0);
    }

}
