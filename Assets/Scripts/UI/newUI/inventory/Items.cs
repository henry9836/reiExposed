using Steamworks.Ugc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class singleItem
{
    public Items.AllItems itemtype;
    public bool equipped;
    public int biginvinpos;
    public int equippedpos;
}

public class Items : MonoBehaviour
{
    public List<Sprite> images = new List<Sprite> { };
    //on biginvin
    public slot slotsref;


    public enum AllItems
    { 
        NONE,
        PLUSSPEED,
        MINUSSPEED,
        PLUSHEALH,
        MINUSHEALTH,
    };


    public List<singleItem> biginvin = new List<singleItem>();
    public List<singleItem> equipped = new List<singleItem>();

    public int biginvinsize = 50;
    public int equpiiedsize = 8;


    void Start()
    {

        for (int i = 0; i < 52; i++)
        {
            gaineditem(AllItems.PLUSHEALH);
        }


        removeitembiginvin(0);
        gaineditem(AllItems.PLUSSPEED);


        slotsref.itemchange();

    }

    public void gaineditem(AllItems toadd)
    {
        singleItem tmp = new singleItem();
        tmp.itemtype = toadd;

        //can fit in big invin
        if (biginvin.Count < biginvinsize)
        {
            tmp.biginvinpos = biginvin.Count;
            biginvin.Add(tmp);

            //can aslo fit in equipped
            if (equipped.Count < equpiiedsize)
            {
                tmp.equipped = true;
                tmp.equippedpos = equipped.Count;
                equipped.Add(tmp);
            }
            else
            {
                tmp.equipped = false;
                tmp.equippedpos = -1;
            }
        }
    }

    public void equipItem(int biginvinpos)
    {
        singleItem tmp = biginvin[biginvinpos];

        if ((equipped.Count < equpiiedsize) && (tmp.equipped == false))
        {
            tmp.equippedpos = equipped.Count;
            tmp.equipped = true;
            equipped.Add(tmp);
        }

    }

    public void upequipItem(int biginvinpos)
    {
        singleItem tmp = biginvin[biginvinpos];

        for (int i = tmp.equippedpos; i < equipped.Count - 1; i++)
        {
            equipped[i] = equipped[i + 1];

        }
        equipped.RemoveAt(equipped.Count - 1);

        tmp.equipped = false;
        tmp.equippedpos = -1;
    }

    public void removeitembiginvin(int biginvinpos)
    {
        removeitem(biginvin[biginvinpos]);
    }

    public void removeitemequipped(int equippedpos)
    {
        removeitem(equipped[equippedpos]);
    }

    private void removeitem(singleItem toremove)
    {
        if (toremove.equipped == true)
        {
            for (int i = toremove.equippedpos; i < equipped.Count - 1; i++)
            {
                equipped[i] = equipped[i + 1];
            }

            equipped.RemoveAt(equipped.Count - 1);
        }


        for (int i = toremove.biginvinpos; i < biginvin.Count - 1; i++)
        {
            biginvin[i] = biginvin[i + 1];
        }

        biginvin.RemoveAt(biginvin.Count - 1);


        toremove = null;
    }
}
