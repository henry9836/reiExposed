using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linerenderFromTo : MonoBehaviour
{
    public GameObject positionto;
    private LineRenderer LR;

    public enum vert
    { 
        TOP,
        MID,
        BOT,
    }

    public enum hor
    { 
        LFT,
        MID,
        RIT,
    }

    public vert vertical;
    public hor horizontal;

    public Vector3 offset;

    void Start()
    {
        LR = GetComponent<LineRenderer>();
        switch (vertical)
        {
            case vert.TOP:
                {
                    offset += Vector3.up * (this.transform.GetComponent<RectTransform>().rect.height / 6.666f);
                    break;
                }
            case vert.MID:
                {
                    break;
                }
            case vert.BOT:
                {
                    offset -= Vector3.up * (this.transform.GetComponent<RectTransform>().rect.height / 6.666f);

                    break;
                }
            default:
                {
                    break;
                }
        }

        switch (horizontal)
        {
            case hor.LFT:
                {
                    offset -= Vector3.forward * (this.transform.GetComponent<RectTransform>().rect.width / 6.666f);

                    break;
                }
            case hor.MID:
                {
                    break;
                }
            case hor.RIT:
                {
                    offset += Vector3.forward * (this.transform.GetComponent<RectTransform>().rect.width / 6.666f);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    void Update()
    {



        LR.SetPosition(0, this.transform.position + offset);
        LR.SetPosition(1, positionto.transform.position);
    }
}
