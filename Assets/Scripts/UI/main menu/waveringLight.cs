using System.Collections;
using UnityEngine;

public class waveringLight : MonoBehaviour
{
    private float speed = 1.0f;
    public Vector2 speedrange;
    public Vector4 minminmaxmaxRange;
    private Vector2 minmax;


    private void Start()
    {
        StartCoroutine(Flicker());
    }

    public IEnumerator Flicker()
    {
        random();
        for (float i = 0.0f; i < 1.0f; i += Time.unscaledDeltaTime * speed)
        {
            gameObject.GetComponent<Light>().spotAngle = Mathf.Lerp(minmax.x, minmax.y, i);
            yield return null; 
        }
        random();
        for (float i = 0.0f; i < 1.0f; i += Time.unscaledDeltaTime * speed)
        {
            gameObject.GetComponent<Light>().spotAngle = Mathf.Lerp(minmax.y, minmax.x, i);
            yield return null;
        }

        StartCoroutine(Flicker());

        yield return null;
    }

    void random()
    {
        speed = Random.Range(speedrange.x, speedrange.y);
        minmax = new Vector2(Random.Range(minminmaxmaxRange.x, minminmaxmaxRange.y), Random.Range(minminmaxmaxRange.z, minminmaxmaxRange.w));
    }

}
