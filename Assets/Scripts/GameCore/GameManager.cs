using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public AIObject boss;
    public UnityEvent GameOverEvent;
    public GameObject rei;


    private void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<AIObject>();
    }

    public void GameOver()
    {
        GameOverEvent.Invoke();
    }

    //true stops the player doing anything
    public void stopPlayer(bool tostop)
    {
        rei.GetComponent<CharacterController>().enabled = !tostop;
        rei.GetComponent<movementController>().enabled = !tostop; // movemnt control turns back on automatically
        rei.GetComponent<PlayerController>().enabled = !tostop;
        rei.GetComponent<umbrella>().enabled = !tostop;
        rei.transform.GetChild(0).GetComponent<cameraControler>().enabled = !tostop;
        rei.GetComponent<Animator>().enabled = !tostop;
        
    }


}
