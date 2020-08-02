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
        //Enable remove button if attached
        if (packager.attach(items.equipped[childIndex].itemtype))
        {
            //Show remove button
            transform.GetChild(childIndex).GetChild(0).gameObject.SetActive(true);
            //Turn off interaction on main button
            transform.GetChild(childIndex).GetComponent<Button>().interactable = false;
        }
    }

    public void remove(int childIndex)
    {
        packager.remove(items.equipped[childIndex].itemtype);
        //Hide remove button
        transform.GetChild(childIndex).GetChild(0).gameObject.SetActive(false);
        //Turn on interaction on main button
        transform.GetChild(childIndex).GetComponent<Button>().interactable = true;
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
                //If our remove button is active then disable interactable
                //transform.GetChild(i).GetComponent<Button>().interactable = true;
                transform.GetChild(i).GetComponent<Button>().interactable = !(transform.GetChild(i).GetChild(0).gameObject.activeInHierarchy);
            }
            else
            {
                transform.GetChild(i).GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }
    }
}
