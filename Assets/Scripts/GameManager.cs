using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public ReprisialOfFlameController rc;
    public UnityEvent GameOverEvent;


    bool once = false;

    private void Start()
    {
        rc = GameObject.FindGameObjectWithTag("Boss").GetComponent<ReprisialOfFlameController>();
    }

    private void FixedUpdate()
    {
        if (rc.health <= 0.0f)
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
        StartCoroutine(goToMainMenu());
    }

    IEnumerator goToMainMenu()
    {
        yield return new WaitForSeconds(10.0f);
        SceneManager.LoadScene(0);
    }

}
