using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBallController : MonoBehaviour
{

    public float travelSpeed = 5.0f;
    public float damage = 10.0f;
    public float killOverrideTime = 5.0f;


    private bool canDie = false;
    private float killTimer = 0.0f;
    public void fullSpeedAheadCaptain()
    {
        StartCoroutine(liveThenDie());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * Time.deltaTime * travelSpeed, Space.World);


        killTimer += Time.deltaTime;

        if (killTimer > killOverrideTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canDie)
        {
            if (other.tag == "Player")
            {
                other.gameObject.GetComponent<PlayerController>().health -= damage;
            }

            Destroy(gameObject);
        }
    }

    IEnumerator liveThenDie()
    {
        yield return new WaitForSeconds(0.5f);
        canDie = true;
        yield return null;
    }

}
