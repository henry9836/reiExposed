using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameAOEController : MonoBehaviour
{
    public float damage = 30.0f;
    public float timeTillDestory = 1.0f;

    private SphereCollider col;
    private float dmgMulitiplier = 1.0f;
    private float rangeForDMG = 1.0f;

    private void Start()
    {
        if(col == null)
        {
            col = GetComponent<SphereCollider>();
        }

        rangeForDMG = col.radius;

        StartCoroutine(killMe());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (col == null)
        {
            col = GetComponent<SphereCollider>();
        }

        if (other.tag == "Player")
        {
            //Deal damage based on distance
            float dmgMulitiplier = Vector3.Distance(transform.position, other.transform.position) / rangeForDMG;

            //Deal Damage
            other.GetComponent<PlayerController>().health -= damage * dmgMulitiplier;
            other.GetComponent<Animator>().SetTrigger("KnockDown");

            //Disable
            col.enabled = false;
        }
    }

    IEnumerator killMe()
    {
        yield return new WaitForSeconds(timeTillDestory);
        Destroy(gameObject);
    }

}
