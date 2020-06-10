using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;
public class VFXController : MonoBehaviour
{

    public Material transparent;
    public Material visible;
    public VisualEffect vs;

    public static readonly string PosMap = "PositionMap";
    public static readonly string VelMap = "VelocityMap";

    private List<GameObject> bodys = new List<GameObject>();
    private List<GameObject> bodysNoVFX = new List<GameObject>();

    void Start()
    {
        GameObject[] bodysA = GameObject.FindGameObjectsWithTag("body");
        GameObject[] bodysNoVFXA = GameObject.FindGameObjectsWithTag("bodyNoVFX");

        for (int i = 0; i < bodysA.Length; i++)
        {
            bodys[i].GetComponent<SkinnedMeshRenderer>().material = transparent;
            bodys.Add(bodysA[i]);
        }

        for (int i = 0; i < bodysNoVFXA.Length; i++)
        {
            bodysNoVFXA[i].GetComponent<SkinnedMeshRenderer>().material = transparent;
            bodysNoVFX.Add(bodysNoVFXA[i]);
        }
    }

}
