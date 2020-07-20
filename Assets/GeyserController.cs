using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserController : MonoBehaviour
{
    public GameObject Explode;
    public GameObject Prepare;
    public float timeTillExplode = 1.0f;

    private void Start()
    {
        Explode.SetActive(false);
        Prepare.SetActive(true);
        StartCoroutine(ticktock());
    }

    IEnumerator ticktock()
    {
        yield return new WaitForSeconds(timeTillExplode);
        Explode.SetActive(true);
        Prepare.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }


}
