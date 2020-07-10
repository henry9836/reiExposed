using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    Vector2 startpos;
    Vector2 finpos;
    Color startcolor;

    void Start()
    {
        startpos = this.gameObject.GetComponent<RectTransform>().anchoredPosition;
        float randx = Random.Range(-0.3f, 0.3f);
        float randy = Random.Range(0.2f, 0.5f);
        finpos = new Vector2(startpos.x + randx, startpos.y + randy);
        startcolor = this.gameObject.GetComponent<Text>().color;

        StartCoroutine(fade());
    }

    IEnumerator fade()
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime)
        {
            float modt1 = 1 - Mathf.Cos(Mathf.Pow(t, 0.3f) * (Mathf.PI / 2));
            float modt2 = Mathf.Cos(Mathf.Pow(t, 3) * (Mathf.PI / 2));

            float x = Mathf.Lerp(startpos.x, finpos.x, modt1);
            float y = Mathf.Lerp(startpos.y, finpos.y, modt1);
            this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

            this.gameObject.GetComponent<Text>().color = new Color(startcolor.r, startcolor.g, startcolor.b, modt2);
            this.gameObject.GetComponent<Outline>().effectColor = new Color(0, 0, 0, modt2);


            yield return null;

        }

        Destroy(this.gameObject.transform.parent.gameObject);
    }
}
