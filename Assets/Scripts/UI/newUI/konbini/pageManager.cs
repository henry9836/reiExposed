using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pageManager : MonoBehaviour
{
    public GameObject upgradespage;
    public GameObject convertpage;
    public GameObject selectedUI;
    public List<Vector2> positions = new List<Vector2>() { };
    private Vector2 canvaspos = new Vector2 (0.0f, 0.0f);
    private GameObject canvas;
    private void Start()
    {
        canvas = GameObject.Find("Canvas");
    }

    public void upgradesPage()
    {
        upgradespage.SetActive(true);
        convertpage.SetActive(false);
        selectedUI.GetComponent<RectTransform>().localPosition = positions[0] + canvaspos;
    }

    public void convertPage()
    {
        upgradespage.SetActive(false);
        convertpage.SetActive(true);
        selectedUI.GetComponent<RectTransform>().localPosition = positions[1] + canvaspos;
    }


    public IEnumerator move()
    {
 

        yield return null;
    }
}
