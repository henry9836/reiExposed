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
                    case Items.AllItems.BAD5HP:
                        {
                            imagelayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Debug Duck";
                            break;
                        }
                    case Items.AllItems.GOOD5HP:
                        {
                            imagelayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Green Tea Can";
                            break;
                        }
                    case Items.AllItems.GOOD10HP:
                        {
                            imagelayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Green Tea Bottle";
                            break;
                        }
                    case Items.AllItems.DOUBLEDAMAGE:
                        {
                            imagelayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Protein Shake";
                            break;
                        }
                    case Items.AllItems.DOUBLESTAMINAREGEN:
                        {
                            imagelayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Energy Drink";
                            break;
                        }
                    case Items.AllItems.MOVESPEED1POINT5:
                        {
                            imagelayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Coffee";
                            break;
                        }
                    case Items.AllItems.MOVESPEED0POINT75:
                        {
                            imagelayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Sake";
                            break;
                        }
                    default:
                        {
                            Debug.Log("how");
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

        Debug.Log(itemref.equipped.Count);

    }


}
