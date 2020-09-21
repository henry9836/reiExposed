using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueController : MonoBehaviour
{

    class slot
    {
        public Image image;
        public Text text;

        public slot(Image img, Text txt)
        {
            image = img;
            text = txt;
        }

    }

    public BossArenaController BossArenaControllerOne;
    public BossArenaController BossArenaControllerTwo;
    public BossArenaController BossArenaControllerThree;


    [Header("Setup")]
    public List<string> cluesNeededBossOne = new List<string>();
    public List<string> clueLore = new List<string>();

    [Header("Player")]
    public List<string> cluesCollected = new List<string>();
    public List<string> clueLoreCollected = new List<string>();

    public bool bossOneCollected = false;
    public bool bossTwoCollected = false;
    public bool bossThreeCollected = false;
    public bool qrFound = false;

    int clueCollectedOne = 0;

    private List<TraceController> traces = new List<TraceController>();
    private List<slot> slots = new List<slot>();
    private Image keyProgress;
    private Text keyProgressText;


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

    public void addOntoLore(string newLore)
    {
        for (int i = 0; i < clueLore.Count; i++)
        {
            if (newLore == clueLore[i])
            {
                clueLoreCollected.Add(newLore);
                return;
            }
        }

        Debug.LogWarning($"New Lore Doesn't Exist {newLore}");

    }

    //Update Phone UI
    public void updateUI(GameObject rootKeyObj)
    {
        //Set up values if they haven't been
        if (slots.Count == 0)
        {
            keyProgress = rootKeyObj.transform.GetChild(4).GetChild(1).GetComponent<Image>();
            keyProgressText = rootKeyObj.transform.GetChild(4).GetChild(2).GetComponent<Text>();

            //For each clue slot
            for (int i = 0; i < rootKeyObj.transform.GetChild(3).GetChild(0).childCount; i++)
            {
                slots.Add(new slot(rootKeyObj.transform.GetChild(3).GetChild(0).GetChild(i).GetComponent<Image>(), rootKeyObj.transform.GetChild(3).GetChild(0).GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>()));
            }
        }

        //Set Progress
        keyProgressText.text = $"{cluesCollected.Count}/3";
        keyProgress.fillAmount = cluesCollected.Count / 3.0f;

        //Update Slots
        for (int i = 0; i < slots.Count; i++)
        {
            bool found = false;

            slots[i].text.text = "?";

            //For all of our collected clues
            for (int j = 0; j < cluesCollected.Count; j++)
            {
                //If it matches the position which the slot is
                if (cluesCollected[j] == cluesNeededBossOne[i])
                {
                    found = true;
                    slots[i].image.color = Color.green;
                }
            }

            //If we didn't find a matching clue
            if (!found)
            {
                slots[i].image.color = Color.red;
            }
        }

        //Setup text
        for (int i = 0; i < clueLore.Count; i++)
        {
            for (int j = 0; j < clueLoreCollected.Count; j++)
            {
                if (clueLoreCollected[j] == clueLore[i])
                {
                    slots[i].text.text = clueLoreCollected[j];
                }
            }
        }

    }


    IEnumerator clueCheckLoop()
    {
        //Check conditions periodically
        int checkIntThreshold = 120;
        int checkInt = 0;

        while (true)
        {
            checkInt++;

            //Limit how often we check info
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
