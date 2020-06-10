using Smrvfx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class VFXGhostEffectBuilder : MonoBehaviour
{

    public GameObject template;
    public GameObject VFXGroupFolder;

    public static readonly string PosMap = "PositionMap";
    public static readonly string VelMap = "VelocityMap";

    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] bodys = GameObject.FindGameObjectsWithTag("body");
        GameObject[] bodysNoVFX = GameObject.FindGameObjectsWithTag("bodyNoVFX");

        for (int i = 0; i < bodys.Length; i++)
        {

            GameObject tmp = Instantiate(template, Vector3.zero, Quaternion.identity);
            tmp.transform.parent = VFXGroupFolder.transform;
            tmp.AddComponent<SkinnedMeshBaker>();
            tmp.GetComponent<SkinnedMeshBaker>()._source = bodys[i].GetComponent<SkinnedMeshRenderer>();

            tmp.GetComponent<VisualEffect>().SetTexture(PosMap, tmp.GetComponent<SkinnedMeshBaker>().PositionMap);
            tmp.GetComponent<VisualEffect>().SetTexture(VelMap, tmp.GetComponent<SkinnedMeshBaker>().VelocityMap);

            bodys[i].GetComponent<SkinnedMeshRenderer>().enabled = false;
        }

        for (int i = 0; i < bodysNoVFX.Length; i++)
        {
            bodysNoVFX[i].GetComponent<SkinnedMeshRenderer>().enabled = false;
        } 

    }
}
