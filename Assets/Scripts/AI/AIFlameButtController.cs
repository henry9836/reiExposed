using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFlameButtController : MonoBehaviour
{

    public float staminaCost = 5.0f;
    public float damage = 10.0f;
    public GameObject particlePrefab;
    public Transform sourcePos;

    AIMovement movement;
    Animator animator;
    AIObject ai;
    PlayerController playerCtrl;
    bool flamed = false;
    bool playerInBox = false;


    //Triggered by animator
    public void flameItUp()
    {
        //stop moving
        movement.stopMovement();

        //Create prefab
        GameObject tmp = Instantiate(particlePrefab, sourcePos.position, Quaternion.identity);
        tmp.GetComponent<groupPartcle>().Play();

        StartCoroutine(killParticle(tmp));

        //If the player is in our hitbox hurt time
        if (playerInBox)
        {
            playerCtrl.EffectHeatlh(-damage);
        }
    }
    
    private void Start()
    {
        ai = transform.root.GetComponent<AIObject>();
        movement = ai.movement;
        animator = ai.animator;
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    //Detection
    private void OnTriggerStay(Collider other)
    {
        if (!flamed && ai.stamina >= staminaCost)
        {
            if (other.tag == "Player")
            {
                animator.SetTrigger("FlameButt");
                animator.SetBool("Attacking", true);
                flamed = true;
                playerInBox = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetBool("Attacking", false);
            animator.ResetTrigger("FlameButt");
            flamed = false;
            playerInBox = false;
        }
    }

    IEnumerator killParticle(GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(obj);
    }

}
