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
    public List<groupPartcle> particleGroups = new List<groupPartcle>();

    //on biginvin
    public slot slotsref;

    public enum AllItems
    {
        NONE,
        HEALTHDEBUFF_SMALL,
        HEALTHBUFF_SMALL,
        HEALTHBUFF,
        DAMAGEBUFF,
        STAMINABUFF,
        MOVEBUFF,
        MOVEDEBUFF,
        DUCK,
        MOVEBUFF_SMALL,
    };

    public List<singleItem> biginvin = new List<singleItem>(50);
    public List<singleItem> equipped = new List<singleItem>() { null, null, null, null, null, null, null, null};

    public int biginvinsize = 50;
    public int equpiiedsize = 8;

    PlayerController player;
    movementController movement;

    void Start()
    {
        for (int i = 0; i < 50; ++i)
        {
            biginvin.Add(null);
        }

        StartCoroutine(loaditems());
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        movement = player.GetComponent<movementController>();
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


        for (int i = 0; i < biginvin.Count; i++)
        {
            if (biginvin[i] == null)
            {
                biginvin.RemoveRange(i, (biginvin.Count - i));
            }
        }

        for (int i = 0; i < equipped.Count; i++)
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

                SaveSystemController.updateValue((int)toadd + "[ITEM]" + tmp.biginvinpos, tmp.biginvinpos + "$" + tmp.equippedpos, true);
                SaveSystemController.saveDataToDisk();
                return true;
            }
        }

        return false;
    }

    public void equipItem(int biginvinpos)
    {
        //tmp is equal to a copy of biginvin[biginvinpos]
        singleItem tmp = biginvin[biginvinpos];

        //If we have a free slot
        if ((equipped.Count < equpiiedsize) && (tmp.equipped == false))
        {
            //set equipped pos
            biginvin[biginvinpos].equippedpos = equipped.Count;
            biginvin[biginvinpos].equipped = true;
            equipped.Add(biginvin[biginvinpos]);

            SaveSystemController.updateValue((int)biginvin[biginvinpos].itemtype + "[ITEM]" + biginvin[biginvinpos].biginvinpos, biginvin[biginvinpos].biginvinpos + "$" + biginvin[biginvinpos].equippedpos, true);

            SaveSystemController.saveDataToDisk();
        }
    }

    public void upequipItem(int biginvinpos)
    {

        int test = biginvin[biginvinpos].equippedpos;
        for (int i = test; i < equipped.Count - 1; i++)
        {
            equipped[i] = equipped[i + 1];
        }
        equipped.RemoveAt(equipped.Count - 1);
        biginvin[biginvinpos].equipped = false;
        biginvin[biginvinpos].equippedpos = -1;

        for (int i = test; i < equipped.Count - 1; i++)
        {
            equipped[i].equippedpos--;
        }

        for (int i = 0; i < SaveSystemController.saveInfomation.Count; i++)
        {
            if (SaveSystemController.saveInfomation[i].id.Contains("[ITEM]"))
            {
                if (!SaveSystemController.saveInfomation[i].value.Contains("-1"))
                {
                    SaveSystemController.removeValue(SaveSystemController.saveInfomation[i].id);
                    i--;
                }
            }
        }

        for (int i = 0; i < equipped.Count; i++)
        {
            SaveSystemController.updateValue((int)equipped[i].itemtype + "[ITEM]" + equipped[i].biginvinpos, equipped[i].biginvinpos + "$" + i, true);
        }

        SaveSystemController.saveDataToDisk();
    }

    public void removeitembiginvin(int biginvinpos, bool useitem)
    {
        removeitem(biginvin[biginvinpos], useitem);
    }

    public void removeitemequipped(AllItems item, bool useitem)
    {
        for (int i = 0; i < equipped.Count; i++)
        {
            if (equipped[i].itemtype == item)
            {
                removeitemequipped(i, useitem);
                return;
            }
        }
    }
    public void removeitemequipped(int equippedpos, bool useitem)
    {
        removeitem(equipped[equippedpos], useitem);
    }

    /*
     * =====================
     * ITEM EFFECTS SECTION
     * =====================
     */

    //Changes health based on a percent of the health
    void HealthEffector(float percent)
    {
        float amount = player.maxHealth * percent;
        player.EffectHeatlh(amount);
    }

    //Applies a random effect
    void DuckBehaviour()
    {
        //Get ampiltude to use
        float amp = Random.Range(-0.3f, 0.3f);

        //Get a random time to apply effect
        float time = Random.Range(10.0f, 45.0f);

        //pick a random effect
        switch (Random.Range(1, 5))
        {
            case 1: //Random Health Effect
                {
                    //VFX
                    if (amp > 25.0f)
                    {
                        particleGroups[(int)AllItems.HEALTHBUFF].Play();
                    }
                    else if(amp > 0.0f)
                    {
                        particleGroups[(int)AllItems.HEALTHBUFF_SMALL].Play();
                    }
                    else
                    {
                        particleGroups[(int)AllItems.HEALTHDEBUFF_SMALL].Play();
                    }

                    HealthEffector(amp);
                    Debug.Log("[DUCK] Health");
                    break;
                }
            case 2: //Random Damage Applier
                {
                    StartCoroutine(ApplyTimedEffect(AllItems.DAMAGEBUFF, amp, time));
                    Debug.Log("[DUCK] Damage");
                    break;
                }
            case 3: //Random Stamina Applier
                {
                    StartCoroutine(ApplyTimedEffect(AllItems.STAMINABUFF, amp, time));
                    Debug.Log("[DUCK] Stamina");
                    break;
                }
            case 4: //Movement Stamina Applier
                {
                    StartCoroutine(ApplyTimedEffect(AllItems.MOVEBUFF, amp, time));
                    Debug.Log("[DUCK] Movement");
                    break;
                }
            default:
                {
                    Debug.LogWarning("[DUCK] No Duck Behaviour Set up");
                    break;
                }
        }
    }
    
    //Timed effects
    IEnumerator ApplyTimedEffect(AllItems item, float percentToChange, float amountOfTimeToApply)
    {
        switch (item)
        {
            case AllItems.DAMAGEBUFF:
                {
                    //VFX
                    if (percentToChange > 0.0f)
                    {
                        particleGroups[(int)AllItems.DAMAGEBUFF].Play();
                    }
                    else
                    {
                        //Hard coded 
                        particleGroups[0].Play();
                    }

                    //Calc
                    float before = player.umbreallaDmg;
                    float result = before * percentToChange;

                    float beforeH = player.umbreallaHeavyDmg;
                    float resultH = beforeH * percentToChange;

                    float beforegun = player.transform.GetComponent<umbrella>().MaxDamage;
                    float resultgun = beforegun * percentToChange;
                    //Apply
                    player.umbreallaDmg += result;
                    player.umbreallaHeavyDmg += resultH;
                    player.transform.GetComponent<umbrella>().MaxDamage += resultgun;
                    //Wait
                    yield return new WaitForSeconds(amountOfTimeToApply);
                    //Unapply
                    player.umbreallaDmg -= result;
                    player.transform.GetComponent<umbrella>().MaxDamage -= resultgun;
                    player.umbreallaHeavyDmg -= resultH;

                    //VFX
                    if (percentToChange > 0.0f)
                    {
                        particleGroups[(int)AllItems.DAMAGEBUFF].Stop();
                    }
                    else
                    {
                        //Hard coded 
                        particleGroups[0].Stop();
                    }

                    Debug.Log("[DUCK] Removed Damage");
                    break;
                }
            case AllItems.STAMINABUFF:
                {
                    //VFX
                    if (percentToChange > 0.0f)
                    {
                        particleGroups[(int)AllItems.STAMINABUFF].Play();
                    }
                    else
                    {
                        //Hard coded 
                        particleGroups[8].Play();
                    }
                    //Calc
                    float before = player.staminaRegenSpeed;
                    float result = before * percentToChange;
                    //Apply
                    player.staminaRegenSpeed += result;
                    //Wait
                    yield return new WaitForSeconds(amountOfTimeToApply);
                    //Unapply
                    player.staminaRegenSpeed -= result;
                    Debug.Log("[DUCK] Removed Stamina");

                    //VFX
                    if (percentToChange > 0.0f)
                    {
                        particleGroups[(int)AllItems.STAMINABUFF].Stop();
                    }
                    else
                    {
                        //Hard coded 
                        particleGroups[8].Stop();
                    }

                    break;
                }
            case AllItems.MOVEBUFF:
                {
                    //VFX
                    if (percentToChange >= 20.0f)
                    {
                        particleGroups[(int)AllItems.MOVEBUFF].Play();
                    }
                    if (percentToChange > 0.0f)
                    {
                        particleGroups[(int)AllItems.MOVEBUFF_SMALL].Play();
                    }
                    else
                    {
                        //Hard coded 
                        particleGroups[(int)AllItems.MOVEDEBUFF].Play();
                    }
                    //Calc
                    float before = movement.moveSpeed;
                    float result = before * percentToChange;
                    //Apply
                    movement.moveSpeed += result;
                    //Wait
                    yield return new WaitForSeconds(amountOfTimeToApply);
                    //Unapply
                    movement.moveSpeed -= result;

                    //VFX
                    if (percentToChange >= 20.0f)
                    {
                        particleGroups[(int)AllItems.MOVEBUFF].Stop();
                    }
                    if (percentToChange > 0.0f)
                    {
                        particleGroups[(int)AllItems.MOVEBUFF_SMALL].Stop();
                    }
                    else
                    {
                        //Hard coded 
                        particleGroups[(int)AllItems.MOVEDEBUFF].Stop();
                    }


                    Debug.Log("[DUCK] Removed Movement");
                    break;
                }
            default:
                {
                    Debug.LogWarning($"No timed behaviour for item: {item}");
                    break;
                }
        }

        yield return null;
    }

    //Use an item
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
                        //VFX
                        particleGroups[(int)toremove.itemtype].Play();
                        //hurt a bit
                        HealthEffector(-0.05f);
                        break;
                    }
                case AllItems.HEALTHBUFF:
                    {
                        //VFX
                        particleGroups[(int)toremove.itemtype].Play();
                        //heal a lot
                        HealthEffector(0.25f);
                        break;
                    }
                case AllItems.HEALTHBUFF_SMALL:
                    {
                        //VFX
                        particleGroups[(int)toremove.itemtype].Play();
                        //heal
                        HealthEffector(0.10f);
                        break;
                    }
                case AllItems.DAMAGEBUFF:
                    {
                        //Higher damage for time
                        StartCoroutine(ApplyTimedEffect(AllItems.DAMAGEBUFF, 0.25f, 25.0f));
                        break;
                    }
                case AllItems.STAMINABUFF:
                    {
                        //Regen faster stamina for time
                        StartCoroutine(ApplyTimedEffect(AllItems.STAMINABUFF, 0.25f, 45.0f));
                        break;
                    }
                case AllItems.MOVEBUFF:
                    {
                        //Faster movement for time
                        StartCoroutine(ApplyTimedEffect(AllItems.MOVEBUFF, 0.05f, 120.0f));
                        break;
                    }
                case AllItems.MOVEBUFF_SMALL:
                    {
                        //Faster movement for time
                        StartCoroutine(ApplyTimedEffect(AllItems.MOVEBUFF, 0.3f, 10.0f));
                        break;
                    }
                case AllItems.MOVEDEBUFF:
                    {
                        //Slower movement for time
                        StartCoroutine(ApplyTimedEffect(AllItems.MOVEBUFF, -0.30f, 15.0f));
                        break;
                    }
                case AllItems.DUCK:
                    {
                        //Random effect
                        DuckBehaviour();
                        break;
                    }
                default:
                    {
                        Debug.LogWarning($"No effect behaviour set up for type: {toremove.itemtype.ToString()}");
                        break;
                    }
            }

        }

        /*
         * ========================
         * ITEM EFFECTS SECTION END
         * ========================
         */

        if (toremove.equipped == true)
        {

            for (int i = toremove.equippedpos; i < equipped.Count - 1; i++)
            {
                //biginvin[equipped[i].biginvinpos].equipped = biginvin[equipped[i + 1].biginvinpos].equipped;
                //biginvin[equipped[i].biginvinpos].equippedpos--;
                equipped[i] = equipped[i + 1];
                equipped[i].equippedpos--;
                //equipped[i].biginvinpos--;
            }
            equipped.RemoveAt(equipped.Count - 1);
        }


        for (int i = toremove.biginvinpos; i < biginvin.Count - 1; i++)
        {
            biginvin[i] = biginvin[i + 1];
            //biginvin[i].equippedpos--;
            biginvin[i].biginvinpos--;
            ////Check for error val
            //if (biginvin[i].equippedpos > -1)
            //{
            //    equipped[biginvin[i].equippedpos].equippedpos--;
            //    biginvin[i].equippedpos--;
            //    biginvin[i].biginvinpos--;
            //}

        }

        biginvin.RemoveAt(biginvin.Count - 1);



        for (int i = 0; i < SaveSystemController.saveInfomation.Count; i++)
        { 
            if (SaveSystemController.saveInfomation[i].id.Contains("[ITEM]"))
            {
                SaveSystemController.removeValue(SaveSystemController.saveInfomation[i].id);
                i--;
            }
        }

        for (int i = 0; i < biginvin.Count; i++)
        {
            SaveSystemController.updateValue((int)biginvin[i].itemtype + "[ITEM]" + i, i + "$" + biginvin[i].equippedpos, true);
        }

        SaveSystemController.saveDataToDisk();

    }

    private void Update()
    {
        if (Input.GetKeyDown((KeyCode.O)))
        {
            for (int i = 0; i < biginvin.Count; i++)
            {
                Debug.Log("i:" + i + " big.biginvinpos:" + biginvin[i].biginvinpos + " big.equpinvinpos;" + biginvin[i].equippedpos + " big.itemtype:" + biginvin[i].itemtype);
            }
            for (int i = 0; i < equipped.Count; i++)
            {
                Debug.Log("i:" + i + " equipped.biginvinpos:" + equipped[i].biginvinpos + " equipped.equpinvinpos;" + equipped[i].equippedpos + " equipped.itemtype:" + equipped[i].itemtype);
            }
        }
    }
}
