using Steamworks.Ugc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public List<singleItem> biginvin = new List<singleItem>(50);
    public List<singleItem> equipped = new List<singleItem>() { null, null, null, null, null, null, null, null};

    public int biginvinsize = 50;
    public int equpiiedsize = 8;


    void Start()
    {
        for (int i = 0; i < 50; ++i)
        {
            biginvin.Add(null);
        }

        Debug.Log(SaveSystemController.saveInfomation.Count);

        StartCoroutine(loaditems());
        ////////////////////demo/////////////////////
        //for (int i = 0; i < 52; i++)
        //{
        //    gaineditem(AllItems.PLUSHEALH);
        //}
        //removeitembiginvin(0, false);
        //gaineditem(AllItems.PLUSSPEED);
        //slotsref.itemchange();

    }

    IEnumerator loaditems() {

        while (!SaveSystemController.loadedValues)
        {
            yield return null;
        }

        for (int i = 0; i < SaveSystemController.saveInfomation.Count; i++)
        {
            //If is item
            if (SaveSystemController.saveInfomation[i].id.Contains("[ITEM]"))
            {

                //Decode
                //#{ID}#7[ITEM]49
                //#{VAL}#12$0

                string savid = SaveSystemController.saveInfomation[i].id; //7[ITEM]49
                string savevalue = SaveSystemController.saveInfomation[i].value; //12$0
                string savevalue2 = savevalue; //12$0

                singleItem tmp = new singleItem();
                int positionID = savid.IndexOf("[");
                int positionVAL = savevalue.IndexOf("$");


                tmp.itemtype = (AllItems)int.Parse(savid.Substring(0, positionID));
                tmp.biginvinpos = int.Parse(savevalue.Substring(0, positionVAL));

                tmp.equippedpos = int.Parse(savevalue2.Substring(positionVAL + 1, (savevalue2.Length - positionVAL) - 1));

                if (tmp.equippedpos != -1)
                {
                    tmp.equipped = true;
                    equipped[tmp.equippedpos] = tmp;
                }
                else
                {
                    tmp.equipped = false;
                }

                biginvin[tmp.biginvinpos] = tmp;
            }
        }

        Debug.Log("before");

        for (int i = 0; i < biginvin.Count; i++)
        {
            Debug.Log("slot " + i + " " + biginvin[i]);
        }

        for (int i = 0; i < biginvin.Count - 1; i++)
        {
            if (biginvin[i] == null)
            {
                biginvin.RemoveRange(i, (biginvin.Count - i));
            }
        }
        Debug.Log("after");

        for (int i = 0; i < biginvin.Count; i++)
        {
            Debug.Log("slot " + i + " " + biginvin[i]);
        }

        for (int i = 0; i < equipped.Count - 1; i++)
        {
            if (equipped[i] == null)
            {
                equipped.RemoveRange(i, (equipped.Count - i));
               
            }
        }



        for (int i = 0; i < equipped.Count; i++)
        {
            Debug.Log("slot " + i + " "+ equipped[i]);
        }

        yield return null;
    }

    public bool gaineditem(AllItems toadd)
    {
        singleItem tmp = new singleItem();
        tmp.itemtype = toadd;

        if (toadd != AllItems.NONE)
        {
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

                SaveSystemController.updateValue((int)toadd + "[ITEM]" + tmp.biginvinpos, tmp.biginvinpos + "$" + tmp.equippedpos);
                SaveSystemController.saveDataToDisk();
                return true;
            }
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
            if (toremove.itemtype == AllItems.BAD5HP)
            { 
                //rei hp -5
            }

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

        SaveSystemController.removeValue((int)toremove.itemtype + "[ITEM]" + toremove.biginvinpos);
        SaveSystemController.saveDataToDisk();

    }
}
