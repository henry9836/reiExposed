using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class arrowanimation : MonoBehaviour
{
    public float timeToFloat = 3.0f;
    public float distanceToFloat = 10.0f;
    [Range(0.0f, 1.0f)]
    public float maxFade = 0.5f;
    public float timeToFade = 1.0f;

    Image img;
    Vector3 initialPosition;
    Vector3 endPosition;
    Color initalColor;
    Color fadeColor;
    bool goingUp = false;
    bool fadingAway = false;
    float timer = 0.0f;
    float fadeTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        initalColor = img.color;
        fadeColor = new Color(initalColor.r, initalColor.g, initalColor.b, maxFade);
        initialPosition = transform.position;
        endPosition = initialPosition + (Vector3.up * distanceToFloat);
        StartCoroutine(loop());
    }

    IEnumerator loop()
    {
        while (true)
        {
            timer += Time.deltaTime;
            fadeTimer += Time.deltaTime;

            if (timer >= timeToFloat)
            {
                timer = 0.0f;
                goingUp = !goingUp;
            }

            if (fadeTimer >= timeToFade)
            {
                fadeTimer = 0.0f;
                fadingAway = !fadingAway;
            }

            if (goingUp)
            {
                transform.position = Vector3.Lerp(initialPosition, endPosition, timer / timeToFloat);
            }
            else
            {
                transform.position = Vector3.Lerp(endPosition, initialPosition, timer / timeToFloat);
            }

            if (fadingAway)
            {
                img.color = Color.Lerp(initalColor, fadeColor, fadeTimer);
            }
            else
            {
                img.color = Color.Lerp(fadeColor, initalColor, fadeTimer);
            }

            yield return null;
        }
    }

   
}