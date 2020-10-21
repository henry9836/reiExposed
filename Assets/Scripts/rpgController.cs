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

            RaycastHit[] hits = Physics.SphereCastAll(transform.position, damageRadius, transform.forward);
            for (int i = 0; i < hits.Length; i++)
            {
                //Damage Myths
                if (hits[i].collider.tag == "Myth")
                {
                    if (hits[i].collider.gameObject.GetComponent<MythCollisionHandler>() != null) {
                        //Prevent dupe dmg
                        bool seen = false;
                        for (int j = 0; j < objsHit.Count; j++)
                        {
                            if (objsHit[j] == hits[i].collider.name)
                            {
                                seen = true;
                                break;
                            }
                        }
                        if (seen)
                        {
                            continue;
                        }

                        Debug.Log($"RPG HIT: {hits[i].collider.name}");
                        objsHit.Add(hits[i].collider.name);
                        //Apply damage based on distance from the explosion
                        hits[i].collider.gameObject.GetComponent<MythCollisionHandler>().overrideDamage(damage * (Vector3.Distance(hits[i].collider.transform.position, transform.position) / damageRadius));
                    }
                }
            }

            //Kill RPG
            Destroy(gameObject);
        }
    }

}
