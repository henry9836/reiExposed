using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class drawTest : MonoBehaviour
{
    public Camera phoneCam;
    public Shader shader;
    [HideInInspector]
    public RenderTexture splatmap;
    [HideInInspector]
    public Material fromMat;
    [HideInInspector]
    public Material toMat;

    private RaycastHit hit;
    public LayerMask tohit;

    public Vector4 topass;

    public float blackpersent = 1.0f;

    public Texture splatmapColored;
    void Start()
    {
        toMat = new Material(shader);
        toMat.SetColor("_Color", Color.red);
        fromMat = GetComponent<SkinnedMeshRenderer>().material;
        splatmap = new RenderTexture(32, 32, 0, RenderTextureFormat.ARGBFloat);
        fromMat.SetTexture("_Splat", splatmap);
    }
    public void toScanBoss()
    {
        if (Physics.Raycast(phoneCam.transform.position, phoneCam.transform.forward, out hit, Mathf.Infinity, tohit))
        {
            topass = new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0);
            toMat.SetVector("_Coordinate", topass); //_Coordinate
            RenderTexture tmp = RenderTexture.GetTemporary(splatmap.width, splatmap.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(splatmap, tmp);
            Graphics.Blit(tmp, splatmap, toMat);
            RenderTexture.ReleaseTemporary(tmp);
            fromMat.SetTexture("Texture2D_DB299D9F", splatmap);

            blackpersent = getblackPixels();
        }
    }


    float getblackPixels()
    {

        int blackcount = 0;
        int totalcount = 0;

        Texture2D totest2 = new Texture2D(splatmap.width, splatmap.height);

        RenderTexture.active = splatmap;
        totest2.ReadPixels(new Rect(0, 0, splatmap.width, splatmap.height), 0, 0);
        totest2.Apply();

        for (int i = 0; i < splatmap.width; i++)
        {
            for (int j = 0; j < splatmap.width; j++)
            {
                if (i < splatmap.width && j < splatmap.width)
                {
                    Color tmp = totest2.GetPixel(i, j);
                    totalcount++;
                    if (tmp.r <= 0.1f)
                    {
                        blackcount++;
                    }
                }
            }
        }

        return ((float)blackcount / (float)totalcount);
    }

    //private void OnGUI()
    //{
    //    GUI.DrawTexture(new Rect(0, 0, 256, 256), splatmap, ScaleMode.ScaleToFit, false, 1);
    //}
}
