using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rpgController : MonoBehaviour
{
    public float rotSpeed = 500.0f;
    public float movementSpeed = 5.0f;
    public float accelerateSpeed = 1.0f;
    public float maxSpeed = 20.0f;

    public GameObject smokeVFX;

    float angle = 0.0f;

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

        Debug.Log(movementSpeed);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Finish" && other.tag != "Player" && other.tag != "PlayerAttackSurface" && !other.name.Contains("rocket"))
        {
            Debug.Log($"KABOOOOOM! {other.tag}|{other.name}");
            //Deparent smoke vfx
            smokeVFX.transform.parent = null;
            smokeVFX.GetComponent<DestoryObject>().Trigger();

            //Kill RPG
            Destroy(gameObject);
        }
    }

}
