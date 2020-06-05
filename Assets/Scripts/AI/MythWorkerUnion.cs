using Boo.Lang.Environments;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MythWorkerUnion : MonoBehaviour
{
    public List<GameObject> mythObjects = new List<GameObject>();
    public List<EnemyController> mythControllers = new List<EnemyController>();

    private int workerID = 0;

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
