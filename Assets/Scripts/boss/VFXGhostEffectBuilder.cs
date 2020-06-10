using Smrvfx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class VFXGhostEffectBuilder : MonoBehaviour
{
    public Material transparent;
    public Material visible;
    public GameObject template;
    public GameObject VFXGroupFolder;
    public bool build = false;

    public static readonly string PosMap = "PositionMap";
    public static readonly string VelMap = "VelocityMap";

    void Awake()
    {
        GameObject[] bodys = GameObject.FindGameObjectsWithTag("body");
        GameObject[] bodysNoVFX = GameObject.FindGameObjectsWithTag("bodyNoVFX");
        for (int i = 0; i < bodys.Length; i++)
        {
            if (build)
            {
                GameObject tmp = Instantiate(template, Vector3.zero, Quaternion.identity);
                tmp.transform.parent = VFXGroupFolder.transform;
                tmp.AddComponent<SkinnedMeshBaker>();
                tmp.GetComponent<SkinnedMeshBaker>()._source = bodys[i].GetComponent<SkinnedMeshRenderer>();

                Debug.Log($"Skin is enabled: {bodys[i].GetComponent<SkinnedMeshRenderer>().enabled}");
                Debug.Log($"VisualEffect is enabled: {bodys[i].GetComponent<VisualEffect>().enabled}");

                tmp.GetComponent<VisualEffect>().SetTexture(PosMap, tmp.GetComponent<SkinnedMeshBaker>().PositionMap);
                tmp.GetComponent<VisualEffect>().SetTexture(VelMap, tmp.GetComponent<SkinnedMeshBaker>().VelocityMap);
            }
            bodys[i].GetComponent<SkinnedMeshRenderer>().material = transparent;
        }

        for (int i = 0; i < bodysNoVFX.Length; i++)
        {
            bodys[i].GetComponent<SkinnedMeshRenderer>().material = transparent;
        }

    }

}
