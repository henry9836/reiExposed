using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialTextCount : MonoBehaviour
{
    public int maxWords;
    public Text input;
    public Color badColor;
    public Color goodColor;

    Image counter;

    private void Start()
    {
        counter = GetComponent<Image>();
    }

    public void FixedUpdate()
    {
        float percent = ((float)input.text.Length / (float)maxWords);
        if (percent > 0.75f)
        {
            counter.color = badColor;
        }
        else
        {
            counter.color = goodColor;
        }
        counter.fillAmount = percent;
    }

}
