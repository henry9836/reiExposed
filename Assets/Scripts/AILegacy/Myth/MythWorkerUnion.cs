using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MythWorkerUnion : MonoBehaviour
{
    public List<GameObject> mythObjects = new List<GameObject>();
    public List<EnemyController> mythControllers = new List<EnemyController>();
    [Range(0.0f, 1.0f)]
    public float maxAgroPercent = 0.5f;

    private int workerID = 0;
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
            mythControllers.Add(myths[i].GetComponent<EnemyController>());
            mythControllers[mythControllers.Count - 1].workerID = workerID;
            mythControllers[mythControllers.Count - 1].union = this;
            workerID++;
        }
    }

    private void FixedUpdate()
    {
        checkTimer += Time.deltaTime;
        if (checkTimer > checkTime)
        {
            //Check ratios of agro and non
            int agroCount = 0;

            for (int i = 0; i < mythControllers.Count-1; i++)
            {
                if (mythControllers[i].aggresiveMode)
                {
                    agroCount++;
                }
            }

            //If more than maxagro remove one agro
            if ((maxAgroPercent/mythControllers.Count) > maxAgroPercent)
            {
                for (int i = 0; i < mythControllers.Count - 1; i++)
                {
                    //Problem fixed
                    if ((maxAgroPercent / mythControllers.Count) <= maxAgroPercent)
                    {
                        break;
                    }
                    //If Agro don't
                    if (mythControllers[i].aggresiveMode)
                    {
                        mythControllers[i].aggresiveMode = false;
                    }

                }
            }

            checkTimer = 0.0f;
        }
    }

    public void ISeeThePlayer(int workerID)
    {
        //Get Inform Range
        float range = mythControllers[workerID].informRange;
        Vector3 informerPos = mythObjects[workerID].transform.position;

        //For all mythobjects within range of our worker inform of player position
        for (int i = 0; i < mythObjects.Count; i++)
        {
            if (i == workerID)
            {
                continue;
            }
            else
            {
                //Check distance
                if (Vector3.Distance(informerPos, mythObjects[i].transform.position) <= range)
                {
                    //Inform of player position
                    mythControllers[i].lastKnownPlayerPosition = mythControllers[workerID].lastKnownPlayerPosition;
                    mythControllers[i].animator.SetBool("AttackMode", true);
                }
            }
        }

    }


}
