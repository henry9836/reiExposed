using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBallController : MonoBehaviour
{

    public float travelSpeed = 5.0f;
    public float damage = 10.0f;
    public float killOverrideTime = 5.0f;
    public LayerMask hittableSurfaces;
    public AIModeSwitcher behaviour;

    private bool canDie = false;
    private float killTimer = 0.0f;

    void Start()
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
        if (canDie && (((1 << other.gameObject.layer) & hittableSurfaces) != 0))
        {

            if (other.tag == "Player")
            {
                if (!other.gameObject.GetComponent<umbrella>().ISBLockjing) {
                    other.gameObject.GetComponent<PlayerController>().health -= damage;
                    other.gameObject.GetComponent<umbrella>().cooldown = true;
                }

                //make myth agro
                behaviour.switchMode(2);

            }
            Destroy(gameObject);
        }
    }

    IEnumerator liveThenDie()
    {
        yield return new WaitForSeconds(0.1f);
        canDie = true;
        yield return null;
    }

}
