using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StorageManager : MonoBehaviour
{
    public GameObject phonestarage;
    public GameObject itemsref;

    void Update()
    {
        for (int i = 0; i < 8; i++)
        {

            if (i < itemsref.GetComponent<Items>().equipped.Count)
            {
                switch (itemsref.GetComponent<Items>().equipped[i].itemtype)
                {
                    case Items.AllItems.NONE:
                        {
                            phonestarage.transform.GetChild(i).GetComponent<Image>().sprite = null;
                            break;
                        }
                    case Items.AllItems.HEALTHDEBUFF_SMALL:
                        {
                            phonestarage.transform.GetChild(i).GetComponent<Image>().sprite = itemsref.GetComponent<Items>().images[1];
                            break;
                        }
                    case Items.AllItems.HEALTHBUFF:
                        {
                            phonestarage.transform.GetChild(i).GetComponent<Image>().sprite = itemsref.GetComponent<Items>().images[3];

                            break;
                        }
                    case Items.AllItems.HEALTHBUFF_SMALL:
                        {
                            phonestarage.transform.GetChild(i).GetComponent<Image>().sprite = itemsref.GetComponent<Items>().images[2];

                            break;
                        }
                    case Items.AllItems.DAMAGEBUFF:
                        {
                            phonestarage.transform.GetChild(i).GetComponent<Image>().sprite = itemsref.GetComponent<Items>().images[4];

                            break;
                        }
                    case Items.AllItems.STAMINABUFF:
                        {
                            phonestarage.transform.GetChild(i).GetComponent<Image>().sprite = itemsref.GetComponent<Items>().images[5];

                            break;
                        }
                    case Items.AllItems.MOVEBUFF:
                        {
                            phonestarage.transform.GetChild(i).GetComponent<Image>().sprite = itemsref.GetComponent<Items>().images[6];

                            break;
                        }
                    case Items.AllItems.MOVEDEBUFF:
                        {
                            phonestarage.transform.GetChild(i).GetComponent<Image>().sprite = itemsref.GetComponent<Items>().images[7];

                            break;
                        }
                    case Items.AllItems.DUCK:
                        {
                            phonestarage.transform.GetChild(i).GetComponent<Image>().sprite = itemsref.GetComponent<Items>().images[8];

                            break;
                        }
                    case Items.AllItems.MOVEBUFF_SMALL:
                        {
                            phonestarage.transform.GetChild(i).GetComponent<Image>().sprite = itemsref.GetComponent<Items>().images[9];

                            break;
                        }
                    default:
                        {
                            Debug.LogWarning("how did u get here");
                            break;
                        }
                }
            }
            else
            {
                phonestarage.transform.GetChild(i).GetComponent<Image>().sprite = null;

            }
        }
    }

}
