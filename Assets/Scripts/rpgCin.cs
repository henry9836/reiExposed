using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rpgCin : MonoBehaviour
{

    Camera mainCam;
    Camera rpgCam;

    Vector3 oldRot;
    Vector3 moveDir;
    Vector3 startPos;
    Vector3 endPos;

    bool showTime = false;
    float distanceToMove = 2.0f;
    float timeToRot = 0.75f;
    float timeToMove = 3.75f;
    float timer = 0.0f;

    public void Toggle(Camera cam)
    {
        if (showTime == false) { 
            mainCam = cam;
            Debug.Log(mainCam.name);
            rpgCam = this.GetComponent<Camera>();
            oldRot = transform.localRotation.eulerAngles;
            moveDir = -transform.forward;
            startPos = transform.position;
            endPos = transform.position + (moveDir * distanceToMove);

            showTime = true;
        }
    }

    public void Update()
    {
        if (showTime)
        {
            timer += Time.unscaledDeltaTime;
            transform.localRotation = Quaternion.Slerp(Quaternion.Euler(oldRot), Quaternion.Euler(oldRot.x, oldRot.y, 0.0f), (timer/ timeToRot));

            transform.position = Vector3.Slerp(startPos, endPos, (timer / timeToMove));

            if (timer > (timeToMove + 5.0f))
            {
                mainCam.enabled = true;
                rpgCam.enabled = false;
                Destroy(gameObject);
            }
        }
    }

}
