using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class eqitems : MonoBehaviour
{
    private Items itemref;
    private Sprite item;


    public List<GameObject> allslots = new List<GameObject> { };
    private GameObject imagelayer;

    private void Awake()
    {
        itemref = this.transform.root.GetComponent<Items>();

        //    for (int i = 0; i < itemref.equipped.Count; i++)
        //    {
        //        allslots.Add(this.gameObject.transform.GetChild(i).gameObject);
        //        this.gameObject.transform.GetChild(i).gameObject.GetComponent<slotno>().slotnumber = i;
        //    }
    }

    public void itemchange()
    {
        for (int i = 0; i < itemref.equpiiedsize; i++)
        {
            imagelayer = allslots[i];

            if (itemref.equipped.Count > i)
            {
                singleItem tmp = itemref.equipped[i];
                item = itemref.images[(int)tmp.itemtype];
                imagelayer.GetComponent<Image>().sprite = item;
                switch (tmp.itemtype)
                {
                    case Items.AllItems.NONE:
                        {
                            imagelayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "no item";
                            break;
                        }
                    case Items.AllItems.DUCK:
                        {
                            imagelayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Debug Duck";
                            break;
                        }
                    case Items.AllItems.HEALTHBUFF_SMALL:
                        {
                            imagelayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Green Tea Can";
                            break;
                        }
                    case Items.AllItems.HEALTHBUFF:
                        {
                            imagelayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Green Tea Bottle";
                            break;
                        }
                    case Items.AllItems.DAMAGEBUFF:
                        {
                            imagelayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Protein Shake";
                            break;
                        }
                    case Items.AllItems.STAMINABUFF:
                        {
                            imagelayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Energy Drink";
                            break;
                        }
                    case Items.AllItems.MOVEBUFF:
                        {
                            imagelayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Coffee";
                            break;
                        }
                    case Items.AllItems.MOVEDEBUFF:
                        {
                            imagelayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Sake";
                            break;
                        }
                    case Items.AllItems.HEALTHDEBUFF_SMALL:
                        {
                            imagelayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Sea Urchin";
                            break;
                        }
                    case Items.AllItems.MOVEBUFF_SMALL:
                        {
                            imagelayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Small Coffee";
                            break;
                        }
                    default:
                        {
                            //USE PROPER DEBUGGING FFS not Debug.Log("how");
                            Debug.LogWarning($"{tmp.itemtype} does not have logic set up");
                            break;
                        }
                }

            }
            else
            {
                imagelayer.GetComponent<Image>().sprite = null;
                imagelayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";

            }

        }

        string insert = itemref.equipped.Count.ToString() + "/8";
        this.transform.GetChild(9).GetComponent<Text>().text = insert;
        this.transform.GetChild(0).GetChild(4).GetComponent<Image>().fillAmount = (float)itemref.equipped.Count / 8.0f;
    }

}
