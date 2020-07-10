using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBody : MonoBehaviour
{

    public enum BodyParts
    {
        ALL,
        HEAD,
        ARMS,
        LEGS,
        BODY,
        FULL_BODY,
        CUSTOM1,
        CUSTOM2,
        CUSTOM3,
        CUSTOM4,
        LEFTARM,
        RIGHTARM,
        LEFTLEG,
        RIGHTLEG,
        ALLCUSTOM
    }

    public List<Collider> head = new List<Collider>();
    public List<Collider> leftarms = new List<Collider>();
    public List<Collider> rightarms = new List<Collider>();
    public List<Collider> leftlegs = new List<Collider>();
    public List<Collider> rightlegs = new List<Collider>();
    public List<Collider> body = new List<Collider>();
    public List<Collider> custom1 = new List<Collider>();
    public List<Collider> custom2 = new List<Collider>();
    public List<Collider> custom3 = new List<Collider>();
    public List<Collider> custom4 = new List<Collider>();

    //Arms or disarms mutilple body parts based on what is requested
    public void updateHitBox(BodyParts parts, bool mode)
    {
        switch (parts)
        {
            case BodyParts.ALL:
                {
                    for (int i = 0; i < head.Count; i++)
                    {
                        head[i].enabled = mode;
                    }
                    for (int i = 0; i < leftarms.Count; i++)
                    {
                        leftarms[i].enabled = mode;
                    }
                    for (int i = 0; i < rightarms.Count; i++)
                    {
                        rightarms[i].enabled = mode;
                    }
                    for (int i = 0; i < leftlegs.Count; i++)
                    {
                        leftlegs[i].enabled = mode;
                    }
                    for (int i = 0; i < rightlegs.Count; i++)
                    {
                        rightlegs[i].enabled = mode;
                    }
                    for (int i = 0; i < body.Count; i++)
                    {
                        body[i].enabled = mode;
                    }
                    for (int i = 0; i < custom1.Count; i++)
                    {
                        custom1[i].enabled = mode;
                    }
                    for (int i = 0; i < custom2.Count; i++)
                    {
                        custom2[i].enabled = mode;
                    }
                    for (int i = 0; i < custom3.Count; i++)
                    {
                        custom3[i].enabled = mode;
                    }
                    for (int i = 0; i < custom4.Count; i++)
                    {
                        custom4[i].enabled = mode;
                    }

                    break;
                }
            case BodyParts.HEAD:
                {
                    for (int i = 0; i < head.Count; i++)
                    {
                        head[i].enabled = mode;
                    }
                    break;
                }
            case BodyParts.ARMS:
                {
                    for (int i = 0; i < leftarms.Count; i++)
                    {
                        leftarms[i].enabled = mode;
                    }
                    for (int i = 0; i < rightarms.Count; i++)
                    {
                        rightarms[i].enabled = mode;
                    }
                    break;
                }
            case BodyParts.LEGS:
                {
                    for (int i = 0; i < leftlegs.Count; i++)
                    {
                        leftlegs[i].enabled = mode;
                    }
                    for (int i = 0; i < rightlegs.Count; i++)
                    {
                        rightlegs[i].enabled = mode;
                    }
                    break;
                }
            case BodyParts.BODY:
                {
                    for (int i = 0; i < body.Count; i++)
                    {
                        body[i].enabled = mode;
                    }
                    break;
                }
            case BodyParts.FULL_BODY:
                {
                    for (int i = 0; i < head.Count; i++)
                    {
                        head[i].enabled = mode;
                    }
                    for (int i = 0; i < leftarms.Count; i++)
                    {
                        leftarms[i].enabled = mode;
                    }
                    for (int i = 0; i < rightarms.Count; i++)
                    {
                        rightarms[i].enabled = mode;
                    }
                    for (int i = 0; i < leftlegs.Count; i++)
                    {
                        leftlegs[i].enabled = mode;
                    }
                    for (int i = 0; i < rightlegs.Count; i++)
                    {
                        rightlegs[i].enabled = mode;
                    }
                    for (int i = 0; i < body.Count; i++)
                    {
                        body[i].enabled = mode;
                    }
                    break;
                }
            case BodyParts.CUSTOM1:
                {
                    for (int i = 0; i < custom1.Count; i++)
                    {
                        custom1[i].enabled = mode;
                    }
                    break;
                }
            case BodyParts.CUSTOM2:
                {
                    for (int i = 0; i < custom2.Count; i++)
                    {
                        custom2[i].enabled = mode;
                    }
                    break;
                }
            case BodyParts.CUSTOM3:
                {
                    for (int i = 0; i < custom3.Count; i++)
                    {
                        custom3[i].enabled = mode;
                    }
                    break;
                }
            case BodyParts.CUSTOM4:
                {
                    for (int i = 0; i < custom4.Count; i++)
                    {
                        custom4[i].enabled = mode;
                    }
                    break;
                }
            case BodyParts.ALLCUSTOM:
                {
                    for (int i = 0; i < custom1.Count; i++)
                    {
                        custom1[i].enabled = mode;
                    }
                    for (int i = 0; i < custom2.Count; i++)
                    {
                        custom2[i].enabled = mode;
                    }
                    for (int i = 0; i < custom3.Count; i++)
                    {
                        custom3[i].enabled = mode;
                    }
                    for (int i = 0; i < custom4.Count; i++)
                    {
                        custom4[i].enabled = mode;
                    }
                    break;
                }
            case BodyParts.LEFTARM:
                {
                    for (int i = 0; i < leftarms.Count; i++)
                    {
                        leftarms[i].enabled = mode;
                    }
                    break;
                }
            case BodyParts.RIGHTARM:
                {
                    for (int i = 0; i < rightarms.Count; i++)
                    {
                        rightarms[i].enabled = mode;
                    }
                    break;
                }
            case BodyParts.LEFTLEG:
                {
                    for (int i = 0; i < leftlegs.Count; i++)
                    {
                        leftlegs[i].enabled = mode;
                    }
                    break;
                }
            case BodyParts.RIGHTLEG:
                {
                    for (int i = 0; i < rightlegs.Count; i++)
                    {
                        rightlegs[i].enabled = mode;
                    }
                    break;
                }
            default:
                {
                    Debug.LogWarning($"No Hitbox Logic for bodypart {parts}");
                    break;
                }
        }
    }

}
