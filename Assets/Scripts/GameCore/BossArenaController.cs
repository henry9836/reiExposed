using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArenaController : MonoBehaviour
{

    public List<GameObject> room;
    public List<GameObject> roomLvl2;
    public AIObject boss;

    int latestClueLevel = 0;

    private void Start()
    {
        if (boss == null)
        {
            Debug.LogWarning("No boss set");
        }
    }

    public void updateState(int cluesFound)
    {
        if (cluesFound > latestClueLevel)
        {
            //Level one
            if (cluesFound >= 1 && latestClueLevel <= 0)
            {
                for (int i = 0; i < room.Count; i++)
                {
                    room[i].GetComponent<MeshRenderer>().enabled = true;
                    room[i].GetComponent<Collider>().enabled = true;
                }
            }
            //Level two
            if (cluesFound >= 2 && latestClueLevel <= 1)
            {
                for (int i = 0; i < roomLvl2.Count; i++)
                {
                    roomLvl2[i].GetComponent<MeshRenderer>().enabled = true;
                    roomLvl2[i].GetComponent<Collider>().enabled = true;
                }
            }
            //Level three
            if (cluesFound >= 3 && latestClueLevel <= 2)
            {
                //Debuff Boss
                boss.staminaRegen *= 0.5f;
            }

            latestClueLevel = cluesFound;
        }
    }
}
