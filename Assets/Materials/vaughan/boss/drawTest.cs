using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TextureExtentions
{
    public static Texture2D ToTexture2D(this Texture texture)
    {
        return Texture2D.CreateExternalTexture(
            texture.width,
            texture.height,
            TextureFormat.RGB24,
            false, false,
            texture.GetNativeTexturePtr());
    }
}
public class drawTest : MonoBehaviour
{

    private Camera cam;
    public Shader shader;
    private RenderTexture splatmap;
    private Material fromMat;
    private Material toMat;

    private RaycastHit hit;
    public LayerMask tohit;

    public Vector4 topass;

    public float blackpersent = 0.0f;
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        toMat = new Material(shader);
        toMat.SetColor("_Color", Color.red);

        fromMat = GetComponent<SkinnedMeshRenderer>().material;
        splatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        fromMat.SetTexture("_Splat", splatmap);
    }

    void Update()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, tohit))
            {
                topass = new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0 , 0);
                toMat.SetVector("_Coordinate", topass); //_Coordinate
                RenderTexture tmp = RenderTexture.GetTemporary(splatmap.width, splatmap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(splatmap, tmp);
                Graphics.Blit(tmp, splatmap, toMat);
                RenderTexture.ReleaseTemporary(tmp);
                fromMat.SetTexture("Texture2D_DB299D9F", splatmap);

                blackpersent = getblackPixels();
            }
        }
    }

    ////commented bit causes lag

    float getblackPixels()
    {

        int blackcount = 0;
        int totalcount = 0;

        Texture totest = fromMat.GetTexture("Texture2D_DB299D9F");
        Texture2D totest2 = new Texture2D(1024, 1024);

        RenderTexture.active = splatmap;
        totest2.ReadPixels(new Rect(0, 0, 1024, 1024), 0, 0);
        totest2.Apply();

        //Texture2D rgbTex = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);

        //RenderTexture.active = totest;
        //rgbTex.ReadPixels(new Rect(0, 0, 1024, 1024), 0, 0);
        //rgbTex.Apply();
        //RenderTexture.active = null;



        //Color[] tmp = rgbTex.GetPixels(0,0, 1024, 1024);





        //for (int i = 0; i < totalcount; i++)
        //{
        //    if (tmp[i] == Color.black)
        //    {
        //        blackcount++;
        //    }
        //}

        for (int i = 0; i < 1024; i++)
        {
            i += 8;
            for (int j = 0; j < 1024; j++)
            {
                j += 8;
                if (i < 1024 && j < 1024)
                {
                    Color tmp = totest2.GetPixel(i, j);
                    totalcount++;
                    if (tmp.r == 0.0f)
                    {
                        blackcount++;
                    }
                }
            }
        }

        Debug.Log(blackcount + " / " + totalcount);

        return ((float)blackcount / (float)totalcount);
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 256, 256), splatmap, ScaleMode.ScaleToFit, false, 1);
    }
}
