using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClueController : MonoBehaviour
{

    public BossArenaController BossArenaControllerOne;
    public BossArenaController BossArenaControllerTwo;
    public BossArenaController BossArenaControllerThree;

    
    public List<string> cluesNeededBossOne = new List<string>();

    //public List<string> cluesNeededBossOne = new List<string>();
    //public List<string> cluesNeededBossTwo = new List<string>();
    //public List<string> cluesNeededBossThree = new List<string>();

    public List<string> cluesCollected = new List<string>();

    public bool bossOneCollected = false;
    public bool bossTwoCollected = false;
    public bool bossThreeCollected = false;
    public bool qrFound = false;

    int clueCollectedOne = 0;
    int clueCollectedTwo = 0;
    int clueCollectedThree = 0;

    private List<TraceController> traces = new List<TraceController>();


    void Start()
    {        
        for (int i = 0; i < SaveSystemController.saveInfomation.Count; i++)
        {
            //If element is a clue
            if (SaveSystemController.saveInfomation[i].id.Contains("[CLUE]"))
            {
                //If element has a valid clue
                if (SaveSystemController.saveInfomation[i].value.Contains("yes"))
                {
                    //Add to list
                    cluesCollected.Add(SaveSystemController.saveInfomation[i].id.Substring(0, SaveSystemController.saveInfomation[i].id.IndexOf("[CLUE]")));
                }
            }
        }

        qrFound = SaveSystemController.getBoolValue("QRCodeFound");

        TraceController[] tracesTmp = GameObject.FindObjectsOfType<TraceController>();

        for (int i = 0; i < tracesTmp.Length; i++)
        {
            traces.Add(tracesTmp[i]);
        }

        //Make spotted clues disapear
        for (int i = 0; i < cluesCollected.Count; i++)
        {
            for (int j = 0; j < traces.Count; j++)
            {
                if (traces[j].name == cluesCollected[i])
                {
                    traces[j].Trigger();
                }
            }
        }

        StartCoroutine(clueCheckLoop());
    }

    IEnumerator clueCheckLoop()
    {
        //Check conditions periodically
        int checkIntThreshold = 100;
        int checkInt = 0;

        while (true)
        {
            checkInt++;

            if (checkInt > checkIntThreshold) {
                //Boss Clue One Group
                if (!bossOneCollected && BossArenaControllerOne != null)
                {
                    //For each string in our clues group one
                    for (int j = 0; j < cluesNeededBossOne.Count; j++)
                    {
                        //For each string in our collected clues
                        for (int i = 0; i < cluesCollected.Count; i++)
                        {
                            if (cluesNeededBossOne[j] == cluesCollected[i])
                            {
                                //Add to counter if it matches
                                clueCollectedOne++;
                            }
                            yield return null;
                        }
                    }

                    BossArenaControllerOne.updateState(clueCollectedOne);

                    //If we have enough clues collected then set bool
                    if (clueCollectedOne >= cluesNeededBossOne.Count)
                    {
                        bossOneCollected = true;
                        break;
                    }
                    //Reset counter
                    else
                    {
                        clueCollectedOne = 0;
                    }
                }

                ////I don't know why, I don't want to know why but for some reason you 
                ////can't put this statment in the while loop and this break is the only 
                ////way exit the while loop correctly
                //if (bossOneCollected && bossTwoCollected && bossThreeCollected)
                //{
                //    //break;
                //}

                checkInt = 0;

            }
            yield return null;
        }

        Debug.Log($"Comparing has finished :D {bossOneCollected}");

        yield return null;
    }

}
