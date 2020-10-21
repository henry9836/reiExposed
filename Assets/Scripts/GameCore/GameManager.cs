using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.HighDefinition;


public class GameManager : MonoBehaviour
{
    public AIObject boss;
    public UnityEvent GameOverEvent;
    public GameObject rei;
    public Volume post; 


    private void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<AIObject>();

        LiftGammaGain tmp;
        if (post.profile.TryGet(out tmp))
        {
            tmp.gamma.value = new Vector4(0.0f, 0.0f, 0.0f, SaveSystemController.getFloatValue("Gamma"));
        }
    }

    public void GameOver()
    {
        GameOverEvent.Invoke();
    }

    //true stops the player doing anything
    public void stopPlayer(bool tostop)
    {
        stopPlayer(tostop, tostop);
    }
    public void stopPlayer(bool tostop, bool stopAnim)
    {
        rei.GetComponent<CharacterController>().enabled = !tostop;
        rei.GetComponent<movementController>().enabled = !tostop; // movemnt control turns back on automatically
        rei.GetComponent<PlayerController>().enabled = !tostop;
        rei.GetComponent<umbrella>().enabled = !tostop;
        rei.transform.GetChild(0).GetComponent<cameraControler>().enabled = !tostop;
        rei.GetComponent<Animator>().enabled = !stopAnim;
        
    }


}
