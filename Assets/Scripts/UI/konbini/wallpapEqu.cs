using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wallpapEqu : MonoBehaviour
{
    public GameObject phoneBGref;
    public List<Sprite> BGsprites;
    public GameObject selector;
    public List<Vector2> selectorpos;
    public int equipped;

    public void init()
    {
        equipped = SaveSystemController.getIntValue("equippedWallpaper");
        if (equipped > BGsprites.Count || equipped < 0)
        {
            Debug.Log("whoops equpiied value was " + equipped + " you should look into that");
            equipped = 0;
        }

        selector.GetComponent<RectTransform>().localPosition = selectorpos[equipped];
        phoneBGref.GetComponent<Image>().sprite = BGsprites[equipped];


        Debug.Log("yaya init");
    }

    public void equip(int toequ)
    {
        equipped = toequ;
        SaveSystemController.updateValue("equippedWallpaper", toequ);
        phoneBGref.GetComponent<Image>().sprite = BGsprites[toequ];
        selector.GetComponent<RectTransform>().localPosition = selectorpos[toequ];
    }
}
