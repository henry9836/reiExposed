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
    public bool Build = false;

    public static readonly string PosMap = "PositionMap";
    public static readonly string VelMap = "VelocityMap";

    // Start is called before the first frame update
    void Awake()
    {
        if (Build)
        {
            GameObject[] bodys = GameObject.FindGameObjectsWithTag("body");

            for (int i = 0; i < bodys.Length; i++)
            {

                //if ((i % 4) == 0) {

                GameObject tmp = Instantiate(template, Vector3.zero, Quaternion.identity);
                tmp.transform.parent = VFXGroupFolder.transform;
                tmp.AddComponent<SkinnedMeshBaker>();
                tmp.GetComponent<SkinnedMeshBaker>()._source = bodys[i].GetComponent<SkinnedMeshRenderer>();

                tmp.GetComponent<VisualEffect>().SetTexture(PosMap, tmp.GetComponent<SkinnedMeshBaker>().PositionMap);
                tmp.GetComponent<VisualEffect>().SetTexture(VelMap, tmp.GetComponent<SkinnedMeshBaker>().VelocityMap);

                //}

                bodys[i].GetComponent<SkinnedMeshRenderer>().enabled = false;
            }

        }

    }
}
