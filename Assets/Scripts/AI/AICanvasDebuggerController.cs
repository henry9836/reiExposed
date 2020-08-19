using Steamworks.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICanvasDebuggerController : MonoBehaviour
{

#if UNITY_EDITOR
    public UnityEngine.UI.Image healthBar;
    public UnityEngine.UI.Image eye;
    public UnityEngine.UI.Image circle;
    public UnityEngine.UI.Text health;
    public UnityEngine.UI.Text stamina;
    public UnityEngine.UI.Text behaviour;
    public UnityEngine.UI.Text attackMode;
    public UnityEngine.UI.Text visionText;
    public UnityEngine.UI.Text rangeText;

    public Sprite eyeSpot;
    public Sprite eyeLost;
    public Sprite eyeCannotSee;

    public UnityEngine.Color red;
    public UnityEngine.Color yellow;
    public UnityEngine.Color orange;
    public UnityEngine.Color green;
    public UnityEngine.Color grey;

    AIObject ai;
    AITracker tracker;
    Animator animator;
    Transform player;
    Transform cam;

    public void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        ai = transform.root.GetComponent<AIObject>();
        tracker = ai.tracker;
        animator = ai.animator;
        player = tracker.target;

        eye.sprite = eyeCannotSee;
        eye.color = red;

        healthBar.color = green;
        attackMode.text = "A: Unbound";
    }

    private void FixedUpdate()
    {
        //Look at cam
        transform.LookAt(transform.position + (transform.position - cam.position).normalized);

        //Get attack and vision and range
        if (ai.getSelectedAttack() != null) {
            attackMode.text = "A: " + ai.getSelectedAttack().attackName;
            float visAmount = 0.0f;
            bool canSee = false;
            float dis = Vector3.Distance(player.position, ai.transform.position);
            bool inRange = (dis <= ai.getSelectedAttack().rangeForAttack.y && dis >= ai.getSelectedAttack().rangeForAttack.x);
            if (ai.getSelectedAttack().overrideTrackingVisionCone)
            {
                Vector3 dir = (player.position - ai.transform.position).normalized;
                visAmount = Vector3.Dot(dir, tracker.eyes.transform.forward);
                canSee = (visAmount >= ai.getSelectedAttack().facePlayerThreshold);
            }
            else
            {
                Vector3 dir = (player.position - ai.transform.position).normalized;
                visAmount = Vector3.Dot(dir, tracker.eyes.transform.forward);
                canSee = (visAmount >= tracker.visionCone);
            }
            visionText.text = "VIS: " + visAmount.ToString();
            rangeText.text = "RNG: " + dis.ToString();
            if (canSee)
            {
                visionText.color = green;
            }
            else
            {
                visionText.color = red;
            }
            if (inRange)
            {
                rangeText.color = green;
            }
            else
            {
                rangeText.color = red;
            }
        }
        else
        {
            attackMode.text = "A: NULL";
            visionText.text = "VIS: NULL";
        }

        //Health Bar
        healthBar.fillAmount = ai.health / ai.startHealth;
        healthBar.color = UnityEngine.Color.Lerp(red, green, healthBar.fillAmount);
        health.text = "Health: " + ai.health.ToString(); 

        //Stamina
        stamina.text = "Stamina: " + ai.stamina.ToString();

        //Behaviour
        behaviour.text = "Behaviour: " + ai.currentMode.ToString();

        //Eyes
        if (tracker.canSeePlayer())
        {
            eye.color = green;
            eye.sprite = eyeSpot;
        }
        else if (animator.GetBool("LosingPlayer"))
        {
            eye.color = yellow;
            eye.sprite = eyeLost;
        }
        else
        {
            eye.color = red;
            eye.sprite = eyeCannotSee;
        }

        //Circle
        if (animator.GetBool("Sleeping"))
        {
            circle.color = grey;
        }
        else if (animator.GetBool("LosingPlayer"))
        {
            circle.color = orange;
        }
        else if (animator.GetBool("Attacking"))
        {
            circle.color = red;
        }
        else
        {
            circle.color = green;
        }

    }

#else

    private void Start(){
        Destroy(gameObject);
    }

#endif
}
