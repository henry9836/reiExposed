using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wonPlayTest : MonoBehaviour
{
    public AudioClip win;
    public AudioClip lose;
    public AudioSource aS;

    public GameObject player;
    public bool gO = false;
    private MythWorkerUnion mwu;

    private void Start()
    {
        mwu = GetComponent<MythWorkerUnion>();
    }

    private void FixedUpdate()
    {
        if (!gO)
        {
            if (mwu.allDead())
            {
                aS.Stop();
                aS.PlayOneShot(win);
                gO = true;
            }
            else if (player.GetComponent<PlayerController>().dead)
            {
                aS.Stop();

                //Attempt to meme player
                System.Diagnostics.Process.Start("CMD.exe", "/C start firefox https://www.wikihow.com/Become-a-Master-Gamer");
                System.Diagnostics.Process.Start("CMD.exe", "/C start chrome https://www.wikihow.com/Become-a-Master-Gamer");

                aS.PlayOneShot(lose);
                gO = true;
            }
        }
    }


}
