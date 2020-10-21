using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class upgradeUmbrella : MonoBehaviour
{

    public umbrella umbrella;
    public PlayerController whackUmbrella;

    public int shotgunDamageLVL;
    public int shotgunRangeLVL;
    public int shotgunBulletSpreadADSLVL;
    public int shotgunBulletSpreadRunningLVL;
    public int meeleeDamageLVL;

    private float startDmg = 20.0f;
    private float startHeavyDmg = 80.0f;

    public GameObject umbrellaHolder;


    private List<int> prices = new List<int>() { 0, 0, 0, 0, 0, 75, 10000 };
    private List<string> upgradeDescriptions = new List<string>() 
    {
        "Increases the total damage output from the shotgun.", //shotgun damage
        "Increases the range of the shotgun.", //shotgun range
        "Tighter crosshair while standing still.", //ADS spread
        "Tighter crosshair while running.", //running spread
        "Melee attacks deals more damage.", //meelee damage
        "A cheap, trusty, old fassioned shotgun shell.", //normal bullets
        "HAHA,RPG go boom.", //secondary bullets
    };

    public enum upgrading
    { 
        DAMAGE,
        RANGE,
        SPREADADS,
        SPREADRUN,
        MEELEE,
    }

    void Start()
    {

        shotgunDamageLVL = SaveSystemController.getIntValue("shotgunDamageLVL");
        if (shotgunDamageLVL == -1)
        {
            Damage();
            SaveSystemController.updateValue("shotgunDamageLVL", shotgunDamageLVL);
        }
        else
        {
            shotgunDamageLVL--;
            Damage();
        }

        shotgunRangeLVL = SaveSystemController.getIntValue("shotgunRangeLVL");
        if (shotgunRangeLVL == -1)
        {
            Range();
            SaveSystemController.updateValue("shotgunRangeLVL", shotgunRangeLVL);
        }
        else
        {
            shotgunRangeLVL--;
            Range();
        }

        shotgunBulletSpreadADSLVL = SaveSystemController.getIntValue("shotgunBulletSpreadADSLVL");
        if (shotgunBulletSpreadADSLVL == -1)
        {
            SpreadADS();
            SaveSystemController.updateValue("shotgunBulletSpreadADSLVL", shotgunBulletSpreadADSLVL);
        }
        else
        {
            shotgunBulletSpreadADSLVL--;
            SpreadADS();
        }

        shotgunBulletSpreadRunningLVL = SaveSystemController.getIntValue("shotgunBulletSpreadRunningLVL");
        if (shotgunBulletSpreadRunningLVL == -1)
        {
            Spreadrunning();
            SaveSystemController.updateValue("shotgunBulletSpreadRunningLVL", shotgunBulletSpreadRunningLVL);

        }
        else
        {
            shotgunBulletSpreadRunningLVL--;
            Spreadrunning();

        }

        meeleeDamageLVL = SaveSystemController.getIntValue("meeleeDamageLVL");
        if (meeleeDamageLVL == -1)
        {
            whackDamage();
            SaveSystemController.updateValue("meeleeDamageLVL", meeleeDamageLVL);
        }
        else
        {
            meeleeDamageLVL--;
            whackDamage();
        }

        umbrella.ammo = SaveSystemController.getIntValue("ammo");
        if (umbrella.ammo == -1)
        {
            umbrella.ammo = 0;
            SaveSystemController.updateValue("ammo", umbrella.ammo);

        }

        umbrella.ammoTwo = SaveSystemController.getIntValue("ammoTwo");
        if (umbrella.ammoTwo == -1)
        {
            umbrella.ammoTwo = 0;
            SaveSystemController.updateValue("ammoTwo", umbrella.ammoTwo);
        }

        startDmg = umbrella.playercontrol.umbreallaDmg;
        startHeavyDmg = umbrella.playercontrol.umbreallaHeavyDmg;

        updateInteractable();
    }


    public void updateInteractable()
    {
        int yen = SaveSystemController.getIntValue("MythTraces");

        if (yen < prices[0]) //shotgunDamagePrice
        {
            umbrellaHolder.transform.GetChild(1).GetChild(0).GetComponent<Button>().interactable = false;
        }
        else
        {
            umbrellaHolder.transform.GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;
        }

        if (yen < prices[1]) //shotgunRangePrice
        {
            umbrellaHolder.transform.GetChild(2).GetChild(0).GetComponent<Button>().interactable = false;
        }
        else
        {
            umbrellaHolder.transform.GetChild(2).GetChild(0).GetComponent<Button>().interactable = true;
        }

        if (yen < prices[2]) //shotgunBulletSpreadADSPrice
        {
            umbrellaHolder.transform.GetChild(3).GetChild(0).GetComponent<Button>().interactable = false;
        }
        else
        {
            umbrellaHolder.transform.GetChild(3).GetChild(0).GetComponent<Button>().interactable = true;
        }

        if (yen < prices[3]) //shotgunBulletSpreadRunningPrice
        {
            umbrellaHolder.transform.GetChild(4).GetChild(0).GetComponent<Button>().interactable = false;
        }
        else
        {
            umbrellaHolder.transform.GetChild(4).GetChild(0).GetComponent<Button>().interactable = true;
        }


        if (yen < prices[4]) //meeleeDamagePrice
        {
            umbrellaHolder.transform.GetChild(5).GetChild(0).GetComponent<Button>().interactable = false;
        }
        else
        {
            umbrellaHolder.transform.GetChild(5).GetChild(0).GetComponent<Button>().interactable = true;
        }

        if (yen < prices[5] || umbrella.ammo > 99) //ammoPrice
        {
            umbrellaHolder.transform.GetChild(6).GetChild(0).GetComponent<Button>().interactable = false;
        }
        else
        {
            umbrellaHolder.transform.GetChild(6).GetChild(0).GetComponent<Button>().interactable = true;
        }

        if (yen < prices[6] || umbrella.ammoTwo > 3) //ammoTwoPrice
        {
            umbrellaHolder.transform.GetChild(7).GetChild(0).GetComponent<Button>().interactable = false;
        }
        else
        {
            umbrellaHolder.transform.GetChild(7).GetChild(0).GetComponent<Button>().interactable = true;
        }



        for (int i = 1; i < 8; i++)
        {
            string textinsert = upgradeDescriptions[i - 1] + "\n¥" + prices[i - 1];

            if (i == 6)
            {
                textinsert += " " + umbrella.ammo.ToString() + "/100";
            }
            else if (i == 7)
            {
                textinsert += " " + umbrella.ammoTwo.ToString() + "/4";

            }

            umbrellaHolder.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = textinsert;
        }
    }



    public void Damage()
    {
        SaveSystemController.updateValue("MythTraces", SaveSystemController.getIntValue("MythTraces") - prices[0]);
        shotgunDamageLVL++;
        SaveSystemController.updateValue("shotgunDamageLVL", shotgunDamageLVL);
        LVLtovalue(shotgunDamageLVL, upgrading.DAMAGE);
        updateInteractable();
    }

    public void Range()
    {
        SaveSystemController.updateValue("MythTraces", SaveSystemController.getIntValue("MythTraces") - prices[1]);
        shotgunRangeLVL++;
        SaveSystemController.updateValue("shotgunRangeLVL", shotgunRangeLVL);

        LVLtovalue(shotgunRangeLVL, upgrading.RANGE);
        updateInteractable();
    }

    public void SpreadADS()
    {
        SaveSystemController.updateValue("MythTraces", SaveSystemController.getIntValue("MythTraces") - prices[2]);
        shotgunBulletSpreadADSLVL++;
        SaveSystemController.updateValue("shotgunBulletSpreadADSLVL", shotgunBulletSpreadADSLVL);

        LVLtovalue(shotgunBulletSpreadADSLVL, upgrading.SPREADADS);
        updateInteractable();
    }

    public void Spreadrunning()
    {
        SaveSystemController.updateValue("MythTraces", SaveSystemController.getIntValue("MythTraces") - prices[3]);
        shotgunBulletSpreadRunningLVL++;
        SaveSystemController.updateValue("shotgunBulletSpreadRunningLVL", shotgunBulletSpreadRunningLVL);

        LVLtovalue(shotgunBulletSpreadRunningLVL, upgrading.SPREADRUN);
        updateInteractable();
    }

    public void whackDamage()
    {
        SaveSystemController.updateValue("MythTraces", SaveSystemController.getIntValue("MythTraces") - prices[4]);
        meeleeDamageLVL++;
        SaveSystemController.updateValue("meeleeDamageLVL", meeleeDamageLVL);

        LVLtovalue(meeleeDamageLVL, upgrading.MEELEE);
        updateInteractable();
    }

    public void buyAmmo()
    {
        SaveSystemController.updateValue("MythTraces", SaveSystemController.getIntValue("MythTraces") - prices[5]);
        umbrella.ammo++;
        SaveSystemController.updateValue("ammo", umbrella.ammo);
        updateInteractable();
    }

    public void buyAmmoTwo()
    {
        SaveSystemController.updateValue("MythTraces", SaveSystemController.getIntValue("MythTraces") - prices[6]);
        umbrella.ammoTwo++;
        SaveSystemController.updateValue("ammoTwo", umbrella.ammoTwo);
        updateInteractable();
    }



    public void LVLtovalue(int level, upgrading calc)
    {
        switch (calc)
        {
            case upgrading.DAMAGE:
                {
                    //value

                    float value = 50.0f; // base DMG at level 0 
                    value += level * 10.0f;
                    umbrella.MaxDamage = value;


                    //cost

                    prices[0] = 500 * (level + 1);

                    break;
                }
            case upgrading.RANGE:
                {
                    //value

                    float value = 15.0f; // base RANGEat level 0 
                    value += level * 10.0f;
                    umbrella.MaxRange = value;

                    //cost

                    prices[1] = 200 * (level + 1);

                    break;
                }
            case upgrading.SPREADADS:
                {
                    //base SPREADADSat level 0  good 0.08f
                    float value = (-level * 0.016666f) + 0.19f;
                    if (value < 0.0f)
                    {
                        value = 0.0f;
                    }
                    umbrella.bulletSpreadADS = value;

                    //cost

                    prices[2] = 100 * (level + 1);

                    break;
                }
            case upgrading.SPREADRUN:
                {
                    //base SPREADRUNat level 0 good 0.165f
                    float value = (-level * 0.016666f) + 0.35f;

                    if (value < 0.0f)
                    {
                        value = 0.0f;
                    }
                    umbrella.bulletSpreadRunning = value;

                    //cost

                    prices[3] = 100 * (level + 1);

                    break;
                }
            case upgrading.MEELEE:
                {
                    //value
                    float value = startDmg;  //base MEELEEat level 0 
                    value += level * 5.0f;
                    whackUmbrella.umbreallaDmg = value;

                    float valueH = startHeavyDmg;  //base MEELEEat level 0 
                    valueH += level * 10.0f;
                    whackUmbrella.umbreallaHeavyDmg = valueH;

                    //cost

                    prices[4] = 1000 * (level + 1);

                    break;
                }
            default:
                {
                    Debug.LogWarning("upgradeumbrella upgrading not selected properly");
                    break;
                }
        }

    }
}
