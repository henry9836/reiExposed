using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rpgController : MonoBehaviour
{
    public float rotSpeed = 500.0f;
    public float movementSpeed = 5.0f;
    public float accelerateSpeed = 1.0f;
    public float maxSpeed = 20.0f;

    public float damage = 400.0f;
    public float damageRadius = 5.0f;

    public GameObject smokeVFX;
    public GameObject explodeVFX;
    public AudioClip explodeSFX;

    float angle = 0.0f;

    List<string> objsHit = new List<string>();

    // Update is called once per frame
    void Update()
    {
        //Move
        transform.position += transform.forward * Time.deltaTime * movementSpeed;

        //Rotate
        angle += Time.deltaTime * rotSpeed;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, angle);


        //Accelerate
        movementSpeed += accelerateSpeed * Time.deltaTime;
        if (movementSpeed > maxSpeed)
        {
            movementSpeed = maxSpeed;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Finish" && other.tag != "Player" && other.tag != "PlayerAttackSurface" && !other.name.Contains("rocket"))
        {
            Debug.Log($"KABOOOOOM! {other.tag}|{other.name}");
            //Deparent smoke vfx
            smokeVFX.transform.parent = null;
            smokeVFX.GetComponent<DestoryObject>().Trigger();

            Collider[] hits = Physics.OverlapSphere(transform.position, damageRadius);
            for (int i = 0; i < hits.Length; i++)
            {
                //Damage Myths
                Debug.Log(hits[i].tag);

                if (hits[i].tag == "Myth")
                {
                    if (hits[i].gameObject.GetComponent<MythCollisionHandler>() != null)
                    {
                        //Prevent dupe dmg
                        bool seen = false;
                        for (int j = 0; j < objsHit.Count; j++)
                        {
                            if (objsHit[j] == hits[i].name)
                            {
                                seen = true;
                                break;
                            }
                        }
                        if (seen)
                        {
                            continue;
                        }
                        objsHit.Add(hits[i].name);

                        //Debug.Log($"RPG HIT: {hits[i].name}");
                        //Apply damage based on distance from the explosion
                        hits[i].gameObject.GetComponent<MythCollisionHandler>().overrideDamage(damage * ((damageRadius - Vector3.Distance(hits[i].transform.position, transform.position)) / damageRadius));
                    }
                }
                else if (hits[i].tag == "Boss")
                {
                    bool seen = false;
                    for (int j = 0; j < objsHit.Count; j++)
                    {
                        if (objsHit[j] == hits[i].name)
                        {
                            seen = true;
                            break;
                        }
                    }
                    if (seen)
                    {
                        continue;
                    }
                    objsHit.Add(hits[i].name);
                    hits[i].gameObject.GetComponent<FlameCollisonOverride>().overrideDamage(damage * ((damageRadius - Vector3.Distance(hits[i].transform.position, transform.position)) / damageRadius));
                }

            }

            GameObject rie = GameObject.Find("PLAYER_rei");
            float dist = Vector3.Distance(rie.transform.position, transform.position);

            if (dist < damageRadius * 3.0f)
            { 
                StartCoroutine(Camera.main.gameObject.GetComponent<cameraShake>().explode((((damageRadius * 3.0f) - dist) / (damageRadius * 3.0f)) * 40.0f));
            }


            //Explode SFX
            explodeVFX.GetComponent<AudioSource>().PlayOneShot(explodeSFX);

            //Explode VFX
            explodeVFX.transform.parent = null;
            explodeVFX.GetComponent<groupPartcle>().Play();
            explodeVFX.AddComponent<DestoryObject>().Trigger();

            //Kill RPG
            Destroy(gameObject);
        }
    }

}
