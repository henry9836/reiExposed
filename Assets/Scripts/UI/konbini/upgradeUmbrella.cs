using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradeUmbrella : MonoBehaviour
{

    public umbrella umbrella;
    public PlayerController whackUmbrella;

    public int shotgunDamageLVL;
    public int shotgunRangeLVL;
    public int shotgunBulletSpreadADSLVL;
    public int shotgunBulletSpreadRunningLVL;
    public int meeleeDamageLVL;

    public float shotgunDamagePrice;
    public float shotgunRangePrice;
    public float shotgunBulletSpreadADSPrice;
    public float shotgunBulletSpreadRunningPrice;
    public float ammoPrice = 100;
    public float ammoTwoPrice = 200;
    public float meeleeDamagePrice;

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


    }


    public void updateInteractable()
    {
        int yen = SaveSystemController.getIntValue("MythTraces");

        if (yen < shotgunDamagePrice)
        {
            //button.interactable = false
        }
        else
        {
            //button.interactable = true
        }

        if (yen < shotgunRangePrice)
        {
            //button.interactable = false
        }
        else
        {
            //button.interactable = true
        }

        if (yen < shotgunBulletSpreadADSPrice)
        {
            //button.interactable = false
        }
        else
        {
            //button.interactable = true
        }

        if (yen < shotgunBulletSpreadRunningPrice)
        {
            //button.interactable = false
        }
        else
        {
            //button.interactable = true
        }


        if (yen < meeleeDamagePrice)
        {
            //button.interactable = false
        }
        else
        {
            //button.interactable = true
        }

        if (yen < ammoPrice)
        {
            //button.interactable = false
        }
        else
        {
            //button.interactable = true
        }

        if (yen < ammoTwoPrice)
        {
            //button.interactable = false
        }
        else
        {
            //button.interactable = true
        }

    }



    public void Damage()
    {
        SaveSystemController.updateValue("MythTraces", SaveSystemController.getIntValue("MythTraces") - shotgunDamagePrice);
        shotgunDamageLVL++;
        SaveSystemController.updateValue("shotgunDamageLVL", shotgunDamageLVL);
        LVLtovalue(shotgunDamageLVL, upgrading.DAMAGE);
    }

    public void Range()
    {
        SaveSystemController.updateValue("MythTraces", SaveSystemController.getIntValue("MythTraces") - shotgunRangePrice);
        shotgunRangeLVL++;
        SaveSystemController.updateValue("shotgunRangeLVL", shotgunRangeLVL);

        LVLtovalue(shotgunRangeLVL, upgrading.RANGE);
    }

    public void SpreadADS()
    {
        SaveSystemController.updateValue("MythTraces", SaveSystemController.getIntValue("MythTraces") - shotgunBulletSpreadADSPrice);
        shotgunBulletSpreadADSLVL++;
        SaveSystemController.updateValue("shotgunBulletSpreadADSLVL", shotgunBulletSpreadADSLVL);

        LVLtovalue(shotgunBulletSpreadADSLVL, upgrading.SPREADADS);
    }

    public void Spreadrunning()
    {
        SaveSystemController.updateValue("MythTraces", SaveSystemController.getIntValue("MythTraces") - shotgunBulletSpreadRunningPrice);
        shotgunBulletSpreadRunningLVL++;
        SaveSystemController.updateValue("shotgunBulletSpreadRunningLVL", shotgunBulletSpreadRunningLVL);

        LVLtovalue(shotgunBulletSpreadRunningLVL, upgrading.SPREADRUN);
    }

    public void whackDamage()
    {
        SaveSystemController.updateValue("MythTraces", SaveSystemController.getIntValue("MythTraces") - meeleeDamagePrice);
        meeleeDamageLVL++;
        SaveSystemController.updateValue("meeleeDamageLVL", meeleeDamageLVL);

        LVLtovalue(meeleeDamageLVL, upgrading.MEELEE);
    }

    public void buyAmmo()
    {
        SaveSystemController.updateValue("MythTraces", SaveSystemController.getIntValue("MythTraces") - ammoPrice);
        umbrella.ammo++;
        SaveSystemController.updateValue("ammo", umbrella.ammo);

    }

    public void buyAmmoTwo()
    {
        SaveSystemController.updateValue("MythTraces", SaveSystemController.getIntValue("MythTraces") - ammoTwoPrice);
        umbrella.ammoTwo++;
        SaveSystemController.updateValue("ammoTwo", umbrella.ammoTwo);

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

                    shotgunDamagePrice = 1000 * (level + 1);

                    break;
                }
            case upgrading.RANGE:
                {
                    //value

                    float value = 15.0f; // base RANGEat level 0 
                    value += level * 10.0f;
                    umbrella.MaxRange = value;

                    //cost

                    shotgunRangePrice = 1000 * (level + 1);

                    break;
                }
            case upgrading.SPREADADS:
                {
                    //value
                    float value;  //base SPREADADSat level 0  good 0.08f
                    float tmp = (level + 6);
                    value = 1.0f / tmp;
                    umbrella.bulletSpreadADS = value;

                    //cost

                    shotgunBulletSpreadADSPrice = 1000 * (level + 1);

                    break;
                }
            case upgrading.SPREADRUN:
                {
                    //value
                    float value;  //base SPREADRUNat level 0 good 0.165f
                    float tmp = (level + 3);
                    value = 1.0f / tmp;
                    umbrella.bulletSpreadRunning = value;

                    //cost

                    shotgunBulletSpreadRunningPrice = 1000 * (level + 1);

                    break;
                }
            case upgrading.MEELEE:
                {
                    //value
                    float value = 30.0f;  //base MEELEEat level 0 
                    value += level * 5.0f;
                    whackUmbrella.umbreallaDmg = value;

                    //cost

                    meeleeDamagePrice = 1000 * (level + 1);

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
