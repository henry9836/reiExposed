using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Random.Range(-360.0f, 350.0f), transform.rotation.eulerAngles.z);
    }
}
