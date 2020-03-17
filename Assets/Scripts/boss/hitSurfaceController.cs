using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitSurfaceController : MonoBehaviour
{

    public float damageAmount = 25.0f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"I HIT {other.gameObject.name}");

        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().health -= damageAmount;
            GetComponent<AudioSource>().Play();
        }
    }
}
