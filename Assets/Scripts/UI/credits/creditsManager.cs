using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class creditsManager : MonoBehaviour
{
    public GameObject textParent;
    public GameObject prefabText;
    public List<string> textlist;


    void Start()
    {
        StartCoroutine(action());
    }


    public IEnumerator action()
    {
        for (int j = 0; j < textlist.Count; j++)
        {
            for (float i = 0.0f; i < 2.0f; i += Time.deltaTime)
            {

                yield return new WaitForEndOfFrame();
            }

            GameObject tmp = GameObject.Instantiate(prefabText);
            tmp.transform.SetParent(textParent.transform);
            tmp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            tmp.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            tmp.GetComponent<Text>().text = textlist[j];

            yield return null;
        }


        yield return null;
    }
}
