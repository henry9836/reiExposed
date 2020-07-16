using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class eqitems : MonoBehaviour
{
    private Items itemref;
    private Sprite item;


    public List<GameObject> allslots = new List<GameObject> { };
    private GameObject imagelayer;

    private void Awake()
    {
        itemref = this.transform.root.GetComponent<Items>();

        //    for (int i = 0; i < itemref.equipped.Count; i++)
        //    {
        //        allslots.Add(this.gameObject.transform.GetChild(i).gameObject);
        //        this.gameObject.transform.GetChild(i).gameObject.GetComponent<slotno>().slotnumber = i;
        //    }
    }

    public void itemchange()
    {
        for (int i = 0; i < itemref.equpiiedsize; i++)
        {
            imagelayer = allslots[i];

            if (itemref.equipped.Count > i)
            {
                singleItem tmp = itemref.equipped[i];
                item = itemref.images[(int)tmp.itemtype];
                imagelayer.GetComponent<Image>().sprite = item;
            }
            else
            {
                imagelayer.GetComponent<Image>().sprite = null;
            }

        }

        Debug.Log(itemref.equipped.Count);

    }


}
