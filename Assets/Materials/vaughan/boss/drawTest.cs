using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            }
        }
    }

    //private void OnGUI()
    //{
    //    GUI.DrawTexture(new Rect(0, 0, 256, 256), splatmap, ScaleMode.ScaleToFit, false, 1);
    //}
}
