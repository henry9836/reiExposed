using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nonstaticCrosshair : MonoBehaviour
{
    private float bulletspread;
    public RectTransform xplus;
    public RectTransform yplus;
    public RectTransform xminus;
    public RectTransform yminus;


    void Update()
    {
        bulletspread = GameObject.FindGameObjectWithTag("Player").GetComponent<umbrella>().bulletSpread;
        applyPos(bulletspread);
    }


    public void applyPos(float pos)
    {
        float yleft = 947.5f;
        float ybot = 490.0f - (pos * 1500.0f);
        float ytop = 490.0f + (pos * 1500.0f);

        float xtop = 490.0f;
        float xleft = 947.5f - (pos * 1500.0f);
        float xright = 947.5f + (pos * 1500.0f);


        yplus.SetTop(ytop);
        yplus.SetLeft(yleft);
        yplus.SetRight(1920.0f - yleft - 25.0f);
        yplus.SetBottom(1080.0f - ytop - 100.0f);

        yminus.SetTop(ybot);
        yminus.SetLeft(yleft);
        yminus.SetRight(1920.0f - yleft - 25.0f);
        yminus.SetBottom(1080.0f - ybot - 100.0f);

        xplus.SetTop(xtop);
        xplus.SetLeft(xright);
        xplus.SetRight(1920.0f - xright - 25.0f);
        xplus.SetBottom(1080.0f - xtop - 100.0f);

        xminus.SetTop(xtop);
        xminus.SetLeft(xleft);
        xminus.SetRight(1920.0f - xleft - 25.0f);
        xminus.SetBottom(1080.0f - xtop - 100.0f);
    }


}
