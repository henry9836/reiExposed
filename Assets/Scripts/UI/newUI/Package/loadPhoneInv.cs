using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadPhoneInv : MonoBehaviour
{
    public Packager packager;
    Items items;

    public void attach(int childIndex)
    {
        packager.attach(items.equipped[childIndex].itemtype);
    }

    public void remove(int childIndex)
    {
        packager.remove(items.equipped[childIndex].itemtype);
    }

    //When UI is toggled
    private void OnEnable()
    {
        if (items == null)
        {
            items = transform.root.GetComponent<Items>();
        }

        //Clear all childern sprites
        for (int i = 0; i < transform.childCount; i++)
        {

            //items.equipped.Count
            if (i < items.equipped.Count)
            {
                transform.GetChild(i).GetComponent<Image>().sprite = items.images[(int)items.equipped[i].itemtype];
                transform.GetChild(i).GetComponent<Image>().color = Color.white;
                transform.GetChild(i).GetComponent<Button>().interactable = true;
            }
            else
            {
                transform.GetChild(i).GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }
    }
}
