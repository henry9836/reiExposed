using Smrvfx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ApplyNewInfoVFX : MonoBehaviour
{
    public static readonly string PosMap = "PositionMap";
    public static readonly string VelMap = "VelocityMap";

    public int count;
     
    public SkinnedMeshBaker sb;
    public VisualEffect vfx;

    private void Start()
    {
        sb = GetComponent<SkinnedMeshBaker>();
        vfx = GetComponent<VisualEffect>();
    }

    void FixedUpdate()
    {
        //if (count < 60) {
            GetComponent<VisualEffect>().SetTexture(PosMap, sb.PositionMap);
            GetComponent<VisualEffect>().SetTexture(VelMap, sb.VelocityMap);

            //count++;
            //Debug.Log("updated");
        //}
    }
}
