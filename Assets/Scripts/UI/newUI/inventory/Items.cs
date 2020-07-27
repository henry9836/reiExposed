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


    //public enum AllItems
    //{ 
    //    NONE,
    //    PLUSSPEED,
    //    MINUSSPEED,
    //    PLUSHEALH,
    //    MINUSHEALTH,
    //};

    public enum AllItems
    {
        NONE,
        BAD5HP,
        GOOD5HP,
        GOOD10HP,
        DOUBLEDAMAGE,
        DOUBLESTAMINAREGEN,
        MOVESPEED1POINT5,
        MOVESPEED0POINT75,
    };

    public List<singleItem> biginvin = new List<singleItem>();
    public List<singleItem> equipped = new List<singleItem>();

    public int biginvinsize = 50;
    public int equpiiedsize = 8;


    void Start()
    {

        ////////////////////demo/////////////////////
        //for (int i = 0; i < 52; i++)
        //{
        //    gaineditem(AllItems.PLUSHEALH);
        //}
        //removeitembiginvin(0, false);
        //gaineditem(AllItems.PLUSSPEED);
        //slotsref.itemchange();

    }

    public bool gaineditem(AllItems toadd)
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

            return true;
        }

        return false;
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

    public void removeitembiginvin(int biginvinpos, bool useitem)
    {
        removeitem(biginvin[biginvinpos], useitem);
    }

    public void removeitemequipped(int equippedpos, bool useitem)
    {
        removeitem(equipped[equippedpos], useitem);
    }

    private void removeitem(singleItem toremove, bool useitem)
    {
        if (useitem == true)
        { 
            //implment useagebehavior
        }

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

    }
}
