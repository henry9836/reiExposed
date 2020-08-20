using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShake : MonoBehaviour
{
    float timer;
    public Vector3 pospass;
    public Vector3 rotpass;
    public float speedpass;

    //void Update()
    //{
    //    timer += Time.deltaTime;
    //    if (timer > 5.0f)
    //    {
    //        timer = 0.0f;
    //        StartCoroutine(shake(pospass, rotpass, speedpass));
    //        StartCoroutine(shake(Vector3.zero, -rotpass , speedpass * 2));
    //    }
    //}

    public IEnumerator shake(Vector3 pos, Vector3 rot, float speed)
    {
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime * speed * 2f)
        {
            transform.localPosition = Vector3.Lerp(Vector3.zero, pos, i);
            transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, rot, i));
            yield return null;
        }
        transform.localPosition = pos;
        transform.localRotation = Quaternion.Euler(rot);

        for (float i = 1.0f; i > 0.0f; i -= Time.deltaTime * speed * 2f)
        {
            transform.localPosition = Vector3.Lerp(Vector3.zero, pos, i);
            transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, rot, i));
            yield return null;
        }

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        yield return null;
    }
}
