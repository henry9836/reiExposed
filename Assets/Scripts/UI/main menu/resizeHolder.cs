using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class leader
{
    public leader(string position1, string name1, string time1)
    {
        position = position1;
        name = name1;
        time = time1;
    }
    public string position;
    public string name;
    public string time;

}

public class resizeHolder : MonoBehaviour
{
    public GameObject canvas;
    public GameObject leaderboardOBJ;

    public static List<leader> listofLeaderboard = new List<leader>(){};

    public int requestcount = 0;

    void Start()
    {
        canvas.GetComponent<packagetosend>().send(packagetosend.sendpackettypes.REQUESTLEADERBOARD, requestcount.ToString());
        requestcount += 10;
    }

    void Update()
    {

        if (listofLeaderboard.Count > 0)
        {
            create(listofLeaderboard[0]);
            listofLeaderboard.RemoveAt(0);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            canvas.GetComponent<packagetosend>().send(packagetosend.sendpackettypes.REQUESTLEADERBOARD, requestcount.ToString());
            requestcount += 10;
        }

        this.GetComponent<RectTransform>().sizeDelta = new Vector2(1000.0f, (140.0f * gameObject.transform.childCount) + (72.0f * 2.0f));

    }

    public void create(leader lead)
    {
        GameObject tmp = Instantiate(leaderboardOBJ);
        tmp.transform.parent = this.transform;
        tmp.transform.GetChild(0).gameObject.GetComponent<Text>().text = lead.position;
        tmp.transform.GetChild(1).gameObject.GetComponent<Text>().text = lead.name;
        tmp.transform.GetChild(2).gameObject.GetComponent<Text>().text = lead.time;
    }
}
