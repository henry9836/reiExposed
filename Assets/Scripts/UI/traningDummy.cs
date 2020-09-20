using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class traningDummy : MonoBehaviour
{
    private PlayerController pc;
    public GameObject damagedtext;
    void Start()
    {
        pc = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttackSurface"))
        {
            //thing got hit 
            if (pc.gameObject.GetComponent<Animator>().GetBool("HeavyAttack"))
            {
                GameObject tmp = GameObject.Instantiate(damagedtext, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
                tmp.transform.GetChild(0).GetComponent<Text>().text = pc.umbreallaHeavyDmg.ToString();
            }
            else
            {
                GameObject tmp = GameObject.Instantiate(damagedtext, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
                tmp.transform.GetChild(0).GetComponent<Text>().text = pc.umbreallaDmg.ToString();
            }

        }
    }
}
