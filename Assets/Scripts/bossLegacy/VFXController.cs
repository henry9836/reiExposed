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
    public LayerMask obsctules;

    public static readonly string PosMap = "PositionMap";
    public static readonly string VelMap = "VelocityMap";

    private List<GameObject> bodys = new List<GameObject>();
    public List<GameObject> bodysNoVFX = new List<GameObject>();
    private float startAmount = 0;

    public void UnlockAll()
    {
        for (int i = 0; i < bodysNoVFX.Count; i++)
        {
            if (bodysNoVFX[i].GetComponent<BossRevealSurfaceController>())
            {
                bodysNoVFX[i].GetComponent<BossRevealSurfaceController>().EnableSurface();
            }
        }
    }

    public float Progress() { return Progress(Mathf.Infinity); }
    public float Progress(float threshold)
    {
        float amountLeft = 0;

        for (int i = 0; i < bodysNoVFX.Count; i++)
        {
            if (bodysNoVFX[i].GetComponent<BossRevealSurfaceController>())
            {
                amountLeft++;
            }
        }

        float result = (amountLeft / startAmount);

        if (result <= threshold && result > 0.0f)
        {
            UnlockAll();
            result = 0.0f;
        }

        return result;
    }

    void Start()
    {
        GameObject[] bodysA = GameObject.FindGameObjectsWithTag("body");
        GameObject[] bodysNoVFXA = GameObject.FindGameObjectsWithTag("bodyNoVFX");
        GameObject[] bones = GameObject.FindGameObjectsWithTag("bossPole");

        for (int i = 0; i < bodysA.Length; i++)
        {
            //bodysA[i].GetComponent<SkinnedMeshRenderer>().material = transparent;
            bodys.Add(bodysA[i]);
        }

        for (int i = 0; i < bodysNoVFXA.Length; i++)
        {
            bodysNoVFXA[i].GetComponent<SkinnedMeshRenderer>().enabled = false;
            bodysNoVFXA[i].AddComponent<BossRevealSurfaceController>();

            //Find direction
            Vector3 pos = bodysNoVFXA[i].GetComponent<SkinnedMeshRenderer>().bounds.center;
            int bestElement = 0;
            float bestDistance = Mathf.Infinity;
            for (int j = 0; j < bones.Length; j++)
            {
                if (Vector3.Distance(pos, bones[j].transform.position) < bestDistance)
                {
                    bestDistance = Vector3.Distance(pos, bones[j].transform.position);
                    bestElement = j;
                }
            }

            bodysNoVFXA[i].GetComponent<BossRevealSurfaceController>().outwardDir = (pos - bones[bestElement].transform.position).normalized;
            bodysNoVFXA[i].GetComponent<BossRevealSurfaceController>().obsctules = obsctules;

            bodysNoVFX.Add(bodysNoVFXA[i]);
        }

        startAmount = bodysNoVFX.Count;
    }

}
