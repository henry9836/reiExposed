using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class bosswall : MonoBehaviour
{
    public UnityEvent onEnter;

    public GameObject phoneMenuUI;
    private umbrella TU;
    private bool entered = false;
    AIObject ai;
    public GameObject bossHPUI;

    private void Start()
    {
        TU = GameObject.FindGameObjectWithTag("Player").GetComponent<umbrella>();
        ai = GameObject.FindGameObjectWithTag("Boss").GetComponent<AIObject>();
        ai.sleepOverride(true);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (entered == false)
            {
                ai.sleepOverride(false);
                bossHPUI.SetActive(true);
                entered = true;

                this.GetComponent<BoxCollider>().enabled = true;
                TU.bossroomtrigger();

                onEnter.Invoke();
                phoneMenuUI.transform.GetChild(0).GetComponent<Button>().interactable = false;
                phoneMenuUI.transform.GetChild(1).GetComponent<Button>().interactable = false;
                phoneMenuUI.transform.GetChild(2).GetComponent<Button>().interactable = false;

            }

        }
    }
}
