using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHost : MonoBehaviour
{
    public enum ATTACKS
    {
        SLAM
    }

    public GameObject player;
    public PlayerController pc;

    public float slamDmg = 10.0f;

    public float slamRange = 3.0f;

    public Transform slamOrigin;

    public groupPartcle slamParticles;

    private bool armed = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerController>();
    }

    public void arm(bool mode)
    {
        armed = mode;
    }

    public void Attack(ATTACKS attack)
    {
        switch (attack)
        {
            case ATTACKS.SLAM:
                {
                    AOEHurtPlayer(slamRange, slamDmg, slamOrigin.position);
                    break;
                }
            default:
                {
                    Debug.LogWarning($"Unknown Attack {attack}");
                    break;
                }
        }
    }

    public void triggerVisuals(ATTACKS attack)
    {
        switch (attack)
        {
            case ATTACKS.SLAM:
                {
                    slamParticles.Play();
                    break;
                }
            default:
                {
                    Debug.LogWarning($"Unknown Attack {attack}");
                    break;
                }
        }
    }

    public void resetVisuals()
    {
        slamParticles.Stop();
    }

    public void AOEHurtPlayer(float range, float dmg, Vector3 origin)
    {
        if (armed)
        {
            if (Vector3.Distance(player.transform.position, origin) <= range)
            {
                pc.health -= dmg;
                armed = false;
            }
        }
    }
}
