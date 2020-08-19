using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MythWorkerUnion : MonoBehaviour
{
    public List<GameObject> mythObjects = new List<GameObject>();
    public List<MythModeSwitcher> mythControllers = new List<MythModeSwitcher>();
    [Range(0.0f, 1.0f)]
    public float maxAgroPercent = 0.5f;

    private int amountOfMythsAlive = 0;
    private float checkTime = 5.0f;
    private float checkTimer = 0.0f;


    public bool allDead()
    {
        for (int i = 0; i < mythControllers.Count - 1; i++)
        {
            if (mythControllers[i].enabled)
            {
                return false;
            }
        }

        return true;
    }
    
    void Start()
    {
        //Find all myths
        GameObject[] myths = GameObject.FindGameObjectsWithTag("Myth");
        for (int i = 0; i < myths.Length; i++)
        {
            mythObjects.Add(myths[i]);
            mythControllers.Add(myths[i].GetComponent<MythModeSwitcher>());
        }

        amountOfMythsAlive = mythControllers.Count;
    }

    private void FixedUpdate()
    {
        checkTimer += Time.deltaTime;
        if (checkTimer > checkTime)
        {
            //Check ratios of agro and non
            int agroCount = 0;


            amountOfMythsAlive = 0;
            for (int i = 0; i < mythControllers.Count-1; i++)
            {
                if (mythControllers[i] != null)
                {
                    if (mythControllers[i].ai.currentMode == (int)MythModeSwitcher.MYTHMODES.AGRO)
                    {
                        agroCount++;
                    }
                    amountOfMythsAlive++;
                }
            }

            //If more than maxagro remove one agro
            if ((maxAgroPercent / amountOfMythsAlive) > maxAgroPercent)
            {
                for (int i = 0; i < mythControllers.Count - 1; i++)
                {
                    if (mythControllers[i] != null) {
                        //Problem fixed
                        if ((maxAgroPercent / amountOfMythsAlive) <= maxAgroPercent)
                        {
                            break;
                        }
                        //If Agro don't
                        if (mythControllers[i].ai.currentMode == (int)MythModeSwitcher.MYTHMODES.AGRO)
                        {
                            mythControllers[i].switchMode((int)MythModeSwitcher.MYTHMODES.PASSIVE);
                        }
                    }
                }
            }

            checkTimer = 0.0f;
        }
    }

    //public void ISeeThePlayer(int workerID)
    //{
    //    //Get Inform Range
    //    float range = mythControllers[workerID].informRange;
    //    Vector3 informerPos = mythObjects[workerID].transform.position;

    //    //For all mythobjects within range of our worker inform of player position
    //    for (int i = 0; i < mythObjects.Count; i++)
    //    {
    //        if (i == workerID)
    //        {
    //            continue;
    //        }
    //        else
    //        {
    //            //Check distance
    //            if (Vector3.Distance(informerPos, mythObjects[i].transform.position) <= range)
    //            {
    //                //Inform of player position
    //                mythControllers[i].lastKnownPlayerPosition = mythControllers[workerID].lastKnownPlayerPosition;
    //                mythControllers[i].animator.SetBool("AttackMode", true);
    //            }
    //        }
    //    }

    //}


}
