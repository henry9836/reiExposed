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
    public GameObject hitVFX;
    public GameObject spawnVFX;

    private bool canDie = false;
    private float killTimer = 0.0f;

    void Start()
    {
        StartCoroutine(liveThenDie());
        Instantiate(spawnVFX, transform.position, Quaternion.identity);
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
        if (other.tag != "Myth")
        {
            if (canDie && (((1 << other.gameObject.layer) & hittableSurfaces) != 0))
            {
                Instantiate(hitVFX, transform.position, Quaternion.identity);

                if (other.tag == "Player")
                {
                    //If the player isn't blocking
                    if (!other.gameObject.GetComponent<umbrella>().ISBLockjing)
                    {
                        other.gameObject.GetComponent<PlayerController>().EffectHeatlh(-damage);
                        other.gameObject.GetComponent<umbrella>().cooldown = true;
                    }

                    //make myth agro
                    behaviour.switchMode(2);

                }
                Destroy(gameObject);
            }
        }
    }

    IEnumerator liveThenDie()
    {
        yield return new WaitForSeconds(0.1f);
        canDie = true;
        yield return null;
    }

}
