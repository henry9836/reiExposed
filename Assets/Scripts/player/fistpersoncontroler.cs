using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fistpersoncontroler : MonoBehaviour
{
    public float mouseSpeed = 1.0f;
    private Vector3 rotation;
    public CharacterController CC;
    public float speed;

    void Update()
    {
        if (CC.isGrounded)
        {
            Vector3 hoz = Input.GetAxis("Horizontal") * transform.right;
            Vector3 vrt = Input.GetAxis("Vertical") * transform.forward;

            CC.Move((hoz + vrt).normalized * Time.deltaTime * speed);

        }
        else
        {
            Vector3 hoz = Input.GetAxis("Horizontal") * transform.right;
            Vector3 vrt = Input.GetAxis("Vertical") * transform.forward;
            Vector3 fall = new Vector3(0.0f, -9.81f, 0.0f);

            CC.Move((hoz + vrt + fall).normalized * Time.deltaTime * speed);
        }


        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");

        Mathf.Clamp(rotation.x, -89.0f, 89.0f);


        transform.eulerAngles = (Vector2)rotation * mouseSpeed;
    }
}
