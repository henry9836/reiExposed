using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class creditsManager : MonoBehaviour
{
    public enum textType
    { 
        TEXT,
        HEADER,
        TITLE,
    }

    public GameObject textParent;
    public GameObject prefabText;
    public GameObject prefabTextHeader;
    public GameObject prefabTextTitle;

    [TextArea(15, 20)]
    public string dump;

    [HideInInspector]
    public List<string> textlist;
    [HideInInspector]
    public List<textType> type;

    public int score = 0;
    public GameObject scoreRef;


    public float timeNormal;
    public float timeHeader;
    public float timeTitle;
    public float timeBig;

    public float speed;


    void Start()
    {
        decode(dump);
        StartCoroutine(action());
    }

    public void decode(string all)
    {
        List<string> decoding = new List<string>() { };

        while (true)
        {
            if (all.IndexOf("--") == -1)
            {
                break;
            }
            else
            {
                decoding.Add(all.Substring(0, all.IndexOf("--")));
                all = all.Substring(all.IndexOf("--") + 2);
            }
        }

        for (int i = 0; i < decoding.Count; i++)
        {
            string ty = decoding[i].Substring(0, 1);
            textlist.Add(decoding[i].Substring(2, decoding[i].Length - 2));

            Debug.Log(ty + "  " + textlist[i]);

            switch (ty)
            {
                case "T":
                    {
                        type.Add(textType.TITLE);
                        break;
                    }
                case "H":
                    {
                        type.Add(textType.HEADER);
                        break;
                    }
                case "N":
                    {
                        type.Add(textType.TEXT);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }


    public IEnumerator action()
    {
        for (int j = 0; j < textlist.Count; j++)
        {
            GameObject tmp = null;

            switch (type[j])
            {
                case textType.TEXT:
                    {
                        tmp = GameObject.Instantiate(prefabText);
                        break;
                    }
                case textType.HEADER:
                    {
                        tmp = GameObject.Instantiate(prefabTextHeader);
                        break;
                    }
                case textType.TITLE:
                    {
                        tmp = GameObject.Instantiate(prefabTextTitle);
                        break;
                    }
                default:
                    {
                        Debug.Log("somthing wrogn with text type");
                        break;
                    }
            }

            tmp.transform.SetParent(textParent.transform);
            tmp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            tmp.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            tmp.GetComponent<Text>().text = textlist[j];

            switch (type[j])
            {
                case textType.TEXT:
                    {
                        for (float i = 0.0f; i < timeNormal; i += Time.deltaTime)
                        {
                            yield return new WaitForEndOfFrame();
                        }
                        break;
                    }
                case textType.HEADER:
                    {
                        for (float i = 0.0f; i < timeHeader; i += Time.deltaTime)
                        {
                            yield return new WaitForEndOfFrame();
                        }
                        break;
                    }
                case textType.TITLE:
                    {
                        for (float i = 0.0f; i < timeTitle; i += Time.deltaTime)
                        {
                            yield return new WaitForEndOfFrame();
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }


            yield return null;
        }

        yield return null;
    }


}
