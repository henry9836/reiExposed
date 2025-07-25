﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TraceController : MonoBehaviour
{

    public GameObject toDestory;
    public ClueController clueCtrl;
    public List<Renderer> renderers = new List<Renderer>();

    private float timer = 0.0f;
    private float TimeTillEnd = 1.0f;
    private bool started = false;
    private List<Material> shaderList = new List<Material>();

    private void Start()
    {
        Debug.Log(name);

        if (renderers.Count == 0)
        {
            if (GetComponent<MeshFilter>())
            {
                shaderList.Add(GetComponent<MeshRenderer>().material);
            }
            else
            {
                shaderList.Add(GetComponent<SkinnedMeshRenderer>().material);
            }
        }
        else
        {
            for (int i = 0; i < renderers.Count; i++)
            {
                shaderList.Add(renderers[i].material);
            }
        }
        

        if (toDestory == null)
        {
            toDestory = transform.root.gameObject;
        }

        clueCtrl = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ClueController>();
    }

    private void FixedUpdate()
    {
        if (started)
        {
            for (int i = 0; i < shaderList.Count; i++)
            {
                shaderList[i].SetFloat("Vector1_AD597E9B", timer);
            }
            timer += Time.deltaTime;

            if (timer > TimeTillEnd)
            {
                Destroy(toDestory);
            }
        }
    }

    public void Trigger()
    {
        started = true;

        if (!clueCtrl)
        {
            clueCtrl = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ClueController>();
        }

        clueCtrl.notificationEvent();
    }
}
