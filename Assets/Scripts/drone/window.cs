using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

#if UNITY_EDITOR

public class window : EditorWindow
{
    //variables
    public string stign = "im a text box for fun :D";
    public bool groupEnabled = true;
    public bool groupEnabled2 = true;

    public float movespeed = 3.0f;
    public Bounds testbounds = new Bounds(new Vector3(0.0f, 20.0f, 0.0f), new Vector3(80.0f, 20.0f, 80.0f));
    public bool stopnextto = false;
    public bool recalc = true;
    public float recalcwhenidle = 2.0f;

    public float deets = 2.0f;
    public float rateofAnglechange = 3.0f;
    public float dynamicedgesize = 10.0f;

    public LayerMask theMask;
    //window inilisation
    [MenuItem("Window/PathFinding")]


    //all elements in the window, the elemets apply to variables that can be set or read from other code 
    void OnGUI()
    {

        GUILayout.Label("base settings", EditorStyles.boldLabel);
        stign = EditorGUILayout.TextField("text fild", stign);

        testbounds = EditorGUILayout.BoundsField("bounds box", testbounds);

        GUILayout.Label("if true it will stop next to the destination if false it will get as close as possible", EditorStyles.helpBox);
        stopnextto = EditorGUILayout.Toggle("stop next to", stopnextto);

        GUILayout.Label("recalculates each step, my be coputationaly intensive", EditorStyles.helpBox);
        recalc = EditorGUILayout.Toggle("recalc", recalc);

        movespeed = EditorGUILayout.FloatField("movespeed", movespeed);

        GUILayout.Label("how long between recalculates when source has already reached destination (set to 0 to disable)", EditorStyles.helpBox);
        recalcwhenidle = EditorGUILayout.FloatField("recalc timer", recalcwhenidle);

        GUILayout.Label("how large the nodes are so bigger is less detailed, massive computational effect", EditorStyles.helpBox);
        deets = EditorGUILayout.FloatField("deets", deets);


        groupEnabled = EditorGUILayout.BeginToggleGroup("smooth movment", groupEnabled);
        GUILayout.Label("if too small it may get stuck as it cant turn sharp enough to reach the next node", EditorStyles.helpBox);
        rateofAnglechange = EditorGUILayout.FloatField("rateofAnglechange", rateofAnglechange);
        EditorGUILayout.EndToggleGroup();

        GUILayout.Label("enabling this group will ignor the box bounds", EditorStyles.helpBox);
        groupEnabled2 = EditorGUILayout.BeginToggleGroup("dynamic bounds box", groupEnabled2);
        dynamicedgesize = EditorGUILayout.FloatField("edge buffer", dynamicedgesize);
        EditorGUILayout.EndToggleGroup();

        GUILayout.Label("set this layermask to somthing if you want it to be ignored when finding a path if it have a collider on it. some examples could be if the source had a collider or a wall that it can walk through, they layermask will need to be but on thoes gameobjects", EditorStyles.helpBox);
        theMask = EditorGUILayout.LayerField("the mask", theMask);

    }



}
#endif
