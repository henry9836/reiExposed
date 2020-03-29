using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class updatePos : MonoBehaviour
{

    public GameObject fireball;

    void Update()
    {
        fireball.GetComponent<VisualEffect>().SetVector3("New Vector", this.gameObject.transform.position);
    }
}
