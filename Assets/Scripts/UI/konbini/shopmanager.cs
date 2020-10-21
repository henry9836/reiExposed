using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shopmanager : MonoBehaviour
{
    public List<string> names = new List<string>() { };
    public List<string> description = new List<string>() { };
    public List<string> shortDescription = new List<string>() { };

    public List<int> costs = new List<int>() { };

    public Text nameDisp;
    public Text descDisp;
    public GameObject cost;
    public Image todisp;

    public int selected;

    public Items canvasitems;

    //Audio
    public AudioClip Purchase;
    public AudioClip CannotBuy;
    private AudioSource audioSrc;

    void Start()
    {
        selectitem(8);
        audioSrc = GetComponent<AudioSource>();
    }


    public void selectitem(int item)
    {
        selected = item;
        nameDisp.text = names[item];
        descDisp.text = description[item];
        cost.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = costs[item].ToString() + "¥";
        todisp.sprite = canvasitems.images[item];

        if (costs[item] > SaveSystemController.getIntValue("MythTraces"))
        {
            cost.transform.GetChild(0).GetComponent<Button>().interactable = false;
        }
        else
        {
            cost.transform.GetChild(0).GetComponent<Button>().interactable = true;
        }
    }

    public void buyitem()
    {
        int curr = SaveSystemController.getIntValue("MythTraces") - costs[selected];
        SaveSystemController.updateValue("MythTraces", curr);
        canvasitems.gaineditem((Items.AllItems)selected);

        if (costs[selected] > SaveSystemController.getIntValue("MythTraces"))
        {
            audioSrc.PlayOneShot(CannotBuy);
            cost.transform.GetChild(0).GetComponent<Button>().interactable = false;
        }
        else
        {
            audioSrc.PlayOneShot(Purchase);
            cost.transform.GetChild(0).GetComponent<Button>().interactable = true;
        }
    }
}
