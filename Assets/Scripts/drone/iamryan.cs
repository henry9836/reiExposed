using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pathfind;
using UnityEditor;

[RequireComponent(typeof(saveFile))]

public class iamryan : MonoBehaviour
{
    //variables 
    private bool reclaculatepath = false;
    [HideInInspector]
    public bool movment = false;
    [HideInInspector]
    public bool movfinished = false;
    [HideInInspector]
    public bool movfin1call = false;
    private bool movfinishedonce = true;

#if UNITY_EDITOR
    [HideInInspector]
    public window editwindow;

    public bool saveWindow = false;

#endif


    [HideInInspector]
    public IEnumerator whenFin;

    public GameObject source;
    public GameObject destination;

    public saveFile save;

#if UNITY_EDITOR

    void Awake()
    {
        editwindow = (window)EditorWindow.GetWindow(typeof(window));
        //editwindow.stign = save.safeItem("stign", saveFile.types.STRING).tostring;
        //editwindow.groupEnabled = System.Convert.ToBoolean(save.safeItem("groupEnabled", saveFile.types.STRING).tostring);
        //editwindow.groupEnabled2 = System.Convert.ToBoolean(save.safeItem("groupEnabled2", saveFile.types.STRING).tostring);
        //editwindow.movespeed = save.safeItem("movespeed", saveFile.types.FLOAT).tofloat;
        //editwindow.testbounds = getbounds();
        //editwindow.stopnextto = System.Convert.ToBoolean(save.safeItem("stopnextto", saveFile.types.STRING).tostring);
        //editwindow.recalc = System.Convert.ToBoolean(save.safeItem("recalc", saveFile.types.STRING).tostring);
        //editwindow.recalcwhenidle = save.safeItem("recalcwhenidle", saveFile.types.FLOAT).tofloat;
        //editwindow.deets = save.safeItem("deets", saveFile.types.FLOAT).tofloat;
        //editwindow.rateofAnglechange = save.safeItem("rateofAnglechange", saveFile.types.FLOAT).tofloat;
        //editwindow.dynamicedgesize = save.safeItem("dynamicedgesize", saveFile.types.FLOAT).tofloat;
        //editwindow.theMask.value = save.safeItem("themask", saveFile.types.INT).toint;

    }
#endif

    void Start()
    {

#if UNITY_EDITOR

        savedll();
#endif
        Path.themask = save.safeItem("themask", saveFile.types.INT).toint;

    }

    void Update()
    {

#if UNITY_EDITOR
        if (saveWindow == true)
        {
            savedll();
            saveWindow = false;
        }
#endif

        //source finished moving
        movfinished = Path.endofmovereached;

        if (movfinished == true)
        {

            //movfin1call is false except for the first instance of movfinished == true

            movfin1call = false;

            if (movfinishedonce == true)
            {
                movfinishedonce = false;
                movfin1call = true;
            }
        }
        else
        {
            //reset the movfin1call calc

            if (movfinishedonce == false)
            {
                movfinishedonce = true;
                movfin1call = false;
            }
        }



        //true one time when finished
        if (movfin1call == true)
        {
            movfin1call = false;
            //corotine set by the user
            StartCoroutine(whenFin);
        }

        //recalcuate
        if (Path.iamryanrecalc == true)
        {
            reclaculatepath = true;
        }

        if (reclaculatepath == true)
        {
            reclaculatepath = false;

            if ((source != null) && (destination != null))
            {
                recalculate();
            }
            else
            {
                Debug.Log("source or destination missing");
            }
        }

        //if the source should be moving 
        if (movment == true)
        {
            if ((source != null) && (destination != null))
            {
                Path.smoothDisable = !System.Convert.ToBoolean(save.safeItem("groupEnabled", saveFile.types.STRING).tostring);
                Path.directionmovespeed = save.safeItem("rateofAnglechange", saveFile.types.FLOAT).tofloat;
                Path.movespeed = save.safeItem("movespeed", saveFile.types.FLOAT).tofloat;
                Path.movement();
            }
            else
            {
                Debug.Log("source or destination missing");
            }
        }
    }

    //initilise all of the things to be used from the window, despite being called recalculate it is also used for the initial calculation aswell
    public void recalculate()
    {
        Path.recalctimer = save.safeItem("recalcwhenidle", saveFile.types.FLOAT).tofloat;
        Path.startphysical = source;
        Path.finishphysical = destination;

        Path.stopnextto = System.Convert.ToBoolean(save.safeItem("stopnextto", saveFile.types.STRING).tostring);
        Path.recalculateEachStep = System.Convert.ToBoolean(save.safeItem("recalc", saveFile.types.STRING).tostring);

        if (System.Convert.ToBoolean(save.safeItem("groupEnabled2", saveFile.types.STRING).tostring) == false) //static bounds
        {
            Path.BBbounds = getbounds();
        }
        else // dyanmic bounds
        {
            Vector3 middlepos = ((source.transform.position + destination.transform.position) / 2.0f);
            Vector3 extents = (destination.transform.position - source.transform.position);
            float edgesize = save.safeItem("dynamicedgesize", saveFile.types.FLOAT).tofloat;
            extents = new Vector3(Mathf.Abs(extents.x) + edgesize, Mathf.Abs(extents.y) + edgesize, Mathf.Abs(extents.z) + edgesize);
            Path.BBbounds = new Bounds(middlepos, extents);

#if UNITY_EDITOR

            editwindow.testbounds = Path.BBbounds;
            editwindow.testbounds.extents *= 2.0f;
#endif

        }

        Path.detail = save.safeItem("deets", saveFile.types.FLOAT).tofloat;

        Path.CalculatePath();
    }



#if UNITY_EDITOR

    //draws gizmoes when in editor mode
    void OnDrawGizmos()
    {
        //bounds
        if (editwindow == null)
        {
            editwindow = (window)EditorWindow.GetWindow(typeof(window));

            editwindow.stign = save.safeItem("stign", saveFile.types.STRING).tostring;
            editwindow.groupEnabled = System.Convert.ToBoolean(save.safeItem("groupEnabled", saveFile.types.STRING).tostring);
            editwindow.groupEnabled2 = System.Convert.ToBoolean(save.safeItem("groupEnabled2", saveFile.types.STRING).tostring);
            editwindow.movespeed = save.safeItem("movespeed", saveFile.types.FLOAT).tofloat;
            editwindow.testbounds = getbounds();
            editwindow.stopnextto = System.Convert.ToBoolean(save.safeItem("stopnextto", saveFile.types.STRING).tostring);
            editwindow.recalc = System.Convert.ToBoolean(save.safeItem("recalc", saveFile.types.STRING).tostring);
            editwindow.recalcwhenidle = save.safeItem("recalcwhenidle", saveFile.types.FLOAT).tofloat;
            editwindow.deets = save.safeItem("deets", saveFile.types.FLOAT).tofloat;
            editwindow.rateofAnglechange = save.safeItem("rateofAnglechange", saveFile.types.FLOAT).tofloat;
            editwindow.dynamicedgesize = save.safeItem("dynamicedgesize", saveFile.types.FLOAT).tofloat;
            editwindow.theMask.value = save.safeItem("themask", saveFile.types.INT).toint;
        }

        Gizmos.DrawWireCube(editwindow.testbounds.center, editwindow.testbounds.extents);

        //walkable nodes
        for (int i = 0; i < Path.nodes.Count; i++)
        {
            if (Path.nodes[i].getwalkable())
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(Path.nodes[i].returnpos(), new Vector3((Path.detail / 8.0f), (Path.detail / 8.0f), (Path.detail / 8.0f)));
            }
        }

        //path source is taking
        for (int i = 0; i < Path.pathlist.Count; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(Path.pathlist[i].returnpos(), new Vector3((Path.detail / 4.0f), (Path.detail / 4.0f), (Path.detail / 4.0f)));
        }
    }


    public void savedll()
    {
        save.saveitem("stign", editwindow.stign);
        save.saveitem("groupEnabled", editwindow.groupEnabled.ToString());
        save.saveitem("groupEnabled2", editwindow.groupEnabled2.ToString());
        save.saveitem("movespeed", editwindow.movespeed);
        save.saveitem("testbounds.center.x", editwindow.testbounds.center.x);
        save.saveitem("testbounds.center.y", editwindow.testbounds.center.y);
        save.saveitem("testbounds.center.z", editwindow.testbounds.center.z);
        save.saveitem("testbounds.size.x", editwindow.testbounds.size.x);
        save.saveitem("testbounds.size.y", editwindow.testbounds.size.y);
        save.saveitem("testbounds.size.z", editwindow.testbounds.size.z);
        save.saveitem("stopnextto", editwindow.stopnextto.ToString());
        save.saveitem("recalc", editwindow.recalc.ToString());
        save.saveitem("recalcwhenidle", editwindow.recalcwhenidle);
        save.saveitem("deets", editwindow.deets);
        save.saveitem("rateofAnglechange", editwindow.rateofAnglechange);
        save.saveitem("dynamicedgesize", editwindow.dynamicedgesize);
        save.saveitem("themask", editwindow.theMask.value);
    }

#endif

    public Bounds getbounds()
    {
        float cx = save.safeItem("testbounds.center.x", saveFile.types.FLOAT).tofloat;
        float cy = save.safeItem("testbounds.center.y", saveFile.types.FLOAT).tofloat;
        float cz = save.safeItem("testbounds.center.z", saveFile.types.FLOAT).tofloat;
        float sx = save.safeItem("testbounds.size.x", saveFile.types.FLOAT).tofloat / 2.0f;
        float sy = save.safeItem("testbounds.size.y", saveFile.types.FLOAT).tofloat / 2.0f;
        float sz = save.safeItem("testbounds.size.z", saveFile.types.FLOAT).tofloat / 2.0f;

        Bounds thebounds = new Bounds(new Vector3(cx, cy, cz), new Vector3(sx, sy, sz));

        return (thebounds);
    }


}
