using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fistpersoncontroler : MonoBehaviour
{
    public float mouseSpeed = 10.0f;
    private Vector3 rotation;
    public CharacterController CC;
    public float speed;
    public GameObject THECAM;

    //do not write use designated functions
    public float pitch = 0;
    public float yaw = 0;
    public void SetPitch(float newPitch)
    {
        pitch = Mathf.Clamp(newPitch, -89.99f, 89.99f);
        THECAM.transform.localEulerAngles = new Vector3(pitch, 0, 0);
    }
    public void SetYaw(float newYaw)
    {
        yaw = Mathf.Repeat(newYaw, 360);
        transform.localEulerAngles = new Vector3(0, yaw, 0);
    }

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

        SetPitch(pitch + -Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime);
        SetYaw(yaw + Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime);
    }
}
