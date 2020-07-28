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


    //BAD NAMING...
    //public enum AllItems
    //{
    //    NONE,
    //    DUCK,
    //    GOOD5HP,
    //    GOOD10HP,
    //    DOUBLEDAMAGE,
    //    DOUBLESTAMINAREGEN,
    //    MOVESPEED1POINT5,
    //    MOVESPEED0POINT75,
    //    LOSE5HP,
    //};


    public enum AllItems
    {
        NONE,
        HEALTHDEBUFF_SMALL,
        HEALTHBUFF,
        HEALTHBUFF_SMALL,
        DAMAGEBUFF,
        STAMINABUFF,
        MOVEBUFF,
        MOVEDEBUFF,
        DUCK,
    };

    public List<singleItem> biginvin = new List<singleItem>();
    public List<singleItem> equipped = new List<singleItem>();

    public int biginvinsize = 50;
    public int equpiiedsize = 8;


    void Start()
    {
        loaditems();
        ////////////////////demo/////////////////////
        //for (int i = 0; i < 52; i++)
        //{
        //    gaineditem(AllItems.PLUSHEALH);
        //}
        //removeitembiginvin(0, false);
        //gaineditem(AllItems.PLUSSPEED);
        //slotsref.itemchange();

    }

    public void loaditems()
    {
        for (int i = 0; i < SaveSystemController.saveInfomation.Count - 1; i++)
        {
            //If is item
            if (SaveSystemController.saveInfomation[i].id.Contains("[ITEM]")){
                //Decode

                singleItem tmp = new singleItem();
                //tmp.itemtype = SaveSystemController.saveInfomation[i].id. bit before [item];
                //tmp.biginvinpos = SaveSystemController.saveInfomation[i].value.
                //tmp.equippedpos = SaveSystemController.saveInfomation[i].value.
                //if (tmp.equippedpos != -1)
                //{
                //    tmp.equipped = true;

                //}
                //else
                //{
                //    tmp.equipped = false;

                //}

                //#{ID}#[ITEM]
                //#{VAL}#12$0

                //Add to our list of items 0

                //Logic for eqipped

            }
        }
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

                //SaveSystemController.updateValue((int)toadd + "[ITEM]", tmp.biginvinpos + "$" + tmp.equippedpos);
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
            //implment useagebehavior
            switch (toremove.itemtype)
            {
                case AllItems.NONE:
                    break;
                case AllItems.HEALTHDEBUFF_SMALL:
                    {
                        //hurt a bit
                        break;
                    }
                case AllItems.HEALTHBUFF:
                    {
                        //heal a lot
                        break;
                    }
                case AllItems.HEALTHBUFF_SMALL:
                    {
                        //heal
                        break;
                    }
                case AllItems.DAMAGEBUFF:
                    {
                        //Higher damage for time
                        break;
                    }
                case AllItems.STAMINABUFF:
                    {
                        //Regen faster stamina for time
                        break;
                    }
                case AllItems.MOVEBUFF:
                    {
                        //Faster movement for time
                        break;
                    }
                case AllItems.MOVEDEBUFF:
                    {
                        //Slower movement for time
                        break;
                    }
                case AllItems.DUCK:
                    {
                        //Random effect
                        break;
                    }
                default:
                    {
                        Debug.LogWarning($"No effect behaviour set up for type: {toremove.itemtype.ToString()}");
                        break;
                    }
            }

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
