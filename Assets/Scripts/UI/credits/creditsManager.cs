using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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
    public float speeddif;

    private NoRepeatSFX nSFX;

    public GameObject ecsref;
    private float ecsammount = 0.0f;
    private float esctimer = 0.0f;
    public float escBlock;
    private bool esconce = false;

    public GameObject blackfade;

    void Start()
    {
        nSFX = GameObject.Find("SFX").GetComponent<NoRepeatSFX>();
        decode(dump);
        StartCoroutine(action());
    }

    void Update()
    {
        esctimer += Time.deltaTime;

        if (esctimer > escBlock)
        {
            if (esconce == false)
            {
                esconce = true;
                ecsref.SetActive(true);
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                ecsammount += Time.deltaTime;
                ecsref.GetComponent<Image>().fillAmount = ecsammount;
                if (ecsammount >= 1.0f)
                {
                    StartCoroutine(returntomenu());
                }
            }
            else
            {
                if (ecsammount > 0.0f)
                {
                    ecsammount -= Time.deltaTime;
                    ecsref.GetComponent<Image>().fillAmount = ecsammount;
                }
                else if (ecsammount < 0.0f)
                {
                    ecsammount = 0.0f;
                }

            }
        }

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

            //Debug.Log(ty + "  " + textlist[i]);

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
                        for (float i = 0.0f; i < timeNormal * (speeddif / speed); i += Time.deltaTime)
                        {
                            yield return new WaitForEndOfFrame();
                        }
                        break;
                    }
                case textType.HEADER:
                    {
                        for (float i = 0.0f; i < timeHeader * (speeddif / speed); i += Time.deltaTime)
                        {
                            yield return new WaitForEndOfFrame();
                        }
                        break;
                    }
                case textType.TITLE:
                    {
                        for (float i = 0.0f; i < timeTitle * (speeddif / speed); i += Time.deltaTime)
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

        //Debug.Log("done");

        yield return new WaitForSeconds(5.0f);
        StartCoroutine(returntomenu());

        yield return null;
    }


    public IEnumerator scorepading()
    {
        speed *= 1.1f;
        nSFX.Play();
        Text textref = scoreRef.GetComponent<Text>();

        for (int j = 0; j < 3; j++)
        {
            for (float i = 0.0f; i < 1.0f; i += Time.deltaTime * 10.0f)
            {
                textref.color = Color.Lerp(Color.white, Color.green, i);

                yield return null;
            }

            textref.fontSize = 80;
            textref.color = Color.red;

            for (float i = 1.0f; i > 0.0f; i -= Time.deltaTime * 10.0f)
            {
                textref.color = Color.Lerp(Color.white, Color.green, i);

                yield return null;
            }

            textref.fontSize = 74;
            textref.color = Color.white;
        }

        yield return null;

    }


    public IEnumerator returntomenu()
    {
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime)
        {
            blackfade.GetComponent<Image>().color = Color.Lerp(new Color(0.0f, 0.0f, 0.0f, 0.0f), new Color(0.0f, 0.0f, 0.0f, 1.0f), i);
            yield return null;
        }

        blackfade.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        SceneToLoadPersistant.sceneToLoadInto = 2;
        SceneManager.LoadScene(1);
        yield return null;
    }


}
