using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueController : MonoBehaviour
{

    public BossArenaController BossArenaControllerOne;
    public BossArenaController BossArenaControllerTwo;
    public BossArenaController BossArenaControllerThree;

    public List<string> cluesNeededBossOne = new List<string>();
    public List<string> cluesNeededBossTwo = new List<string>();
    public List<string> cluesNeededBossThree = new List<string>();

    public List<string> cluesCollected = new List<string>();

    public bool bossOneCollected = false;
    public bool bossTwoCollected = false;
    public bool bossThreeCollected = false;

    int clueCollectedOne = 0;
    int clueCollectedTwo = 0;
    int clueCollectedThree = 0;

    //Reload clues from the save system
    public void reloadClues()
    {
        //Empty list so that we do not dupe our elements
        cluesCollected.Clear();
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
    }

    private void Start()
    {
        reloadClues();
        StartCoroutine(clueCheckLoop());
    }

    IEnumerator clueCheckLoop()
    {
        while (true)
        {
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
                }
                //Reset counter
                else
                {
                    clueCollectedOne = 0;
                }
            }

            //Boss Clue Two Group
            if (!bossTwoCollected && BossArenaControllerTwo != null)
            {
                //For each string in our clues group one
                for (int j = 0; j < cluesNeededBossTwo.Count; j++)
                {
                    //For each string in our collected clues
                    for (int i = 0; i < cluesCollected.Count; i++)
                    {
                        if (cluesNeededBossTwo[j] == cluesCollected[i])
                        {
                            //Add to counter if it matches
                            clueCollectedTwo++;
                        }
                        yield return null;
                    }
                }

                BossArenaControllerTwo.updateState(clueCollectedTwo);

                //If we have enough clues collected then set bool
                if (clueCollectedOne >= cluesNeededBossTwo.Count)
                {
                    bossTwoCollected = true;
                }
                //Reset counter
                else
                {

                    clueCollectedTwo = 0;
                }
            }

            //Boss Clue Three Group
            if (!bossThreeCollected && BossArenaControllerThree != null)
            {
                //For each string in our clues group one
                for (int j = 0; j < cluesNeededBossThree.Count; j++)
                {
                    //For each string in our collected clues
                    for (int i = 0; i < cluesCollected.Count; i++)
                    {
                        if (cluesNeededBossThree[j] == cluesCollected[i])
                        {
                            //Add to counter if it matches
                            clueCollectedThree++;
                        }
                        yield return null;
                    }
                }

                BossArenaControllerThree.updateState(clueCollectedThree);

                //If we have enough clues collected then set bool
                if (clueCollectedThree >= cluesNeededBossThree.Count)
                {
                    bossThreeCollected = true;
                }
                //Reset counter
                else
                {
                    clueCollectedThree = 0;
                }
            }
            
            //I don't know why, I don't want to know why but for some reason you 
            //can't put this statment in the while loop and this break is the only 
            //way exit the while loop correctly
            if (bossOneCollected && bossTwoCollected && bossThreeCollected)
            {
                //break;
            }

            yield return null;
        }

        Debug.Log($"Comparing has finished :D {bossOneCollected}|{bossTwoCollected}|{bossThreeCollected}");

        yield return null;
    }

}
