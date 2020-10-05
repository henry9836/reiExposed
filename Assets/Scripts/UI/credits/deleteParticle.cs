using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteParticle : MonoBehaviour
{
    private float timer = 25.0f;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            Destroy(this.gameObject);
        }
    }
}
