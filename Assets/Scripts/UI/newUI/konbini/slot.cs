using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class slot : MonoBehaviour
{
    public Items itemref;

    private Sprite item;
    public Sprite equippedoverlay;

    private GameObject imagelayer;
    private GameObject overlayLayer;

    public List<GameObject> allslots = new List<GameObject> {};

    public void itemchange()
    {


        for (int i = 0; i < itemref.biginvinsize; i++)
        {

            imagelayer = allslots[i];
            overlayLayer = allslots[i].transform.GetChild(0).gameObject;

            if (itemref.biginvin.Count > i)
            {
                singleItem tmp = itemref.biginvin[i];
                item = itemref.images[(int)tmp.itemtype];
                imagelayer.GetComponent<Image>().sprite = item;
                overlayLayer.GetComponent<Image>().sprite = equippedoverlay;


                if (tmp.equipped == true)
                {
                    overlayLayer.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }
                else
                {
                    overlayLayer.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                }
            }
            else
            {
                imagelayer.GetComponent<Image>().sprite = null;
                overlayLayer.GetComponent<Image>().sprite = null;
            }
        }


    }

    public void onclick(Object self)
    {
        GameObject test = (GameObject)self;
        int test2 = test.gameObject.GetComponent<slotno>().slotnumber;
        singleItem tmp = null;

        try
        {
            if (itemref.biginvin[test2] != null)
            {
                tmp = itemref.biginvin[test2];
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }

        if (tmp != null)
        {
            item = itemref.images[(int)tmp.itemtype];

            if (tmp.equipped == true)
            {
                itemref.upequipItem(test2);
            }
            else
            {
                itemref.equipItem(test2);
            }

            itemchange();
        }


    }



    public void onclickphone(Object self)
    {
        GameObject test = (GameObject)self;
        int test2 = test.gameObject.GetComponent<slotno>().slotnumber;
        singleItem tmp = null;

        try
        {
            if (itemref.equipped[test2] != null)
            {
                tmp = itemref.equipped[test2];
            }

        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }

        if (tmp != null)
        {
            item = itemref.images[(int)tmp.itemtype];

            if (tmp.equipped == true)
            {
                itemref.upequipItem(tmp.biginvinpos);
            }
            else
            {
                itemref.equipItem(tmp.biginvinpos);
            }

            itemchange();
        }


    }
}
