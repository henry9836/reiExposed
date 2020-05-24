using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.TerrainAPI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using HenryIK;

public class EzyIK : MonoBehaviour
{
    public IKPlugin.BoneStructure boneStructure;
    public IKPlugin.BoneStructure.MoveType movementType = IKPlugin.BoneStructure.MoveType.LINEAR;
    //How far to search for bones if below 0 then search till we cannot find any more objects
    public int maxDepthSearch = -1;

    [Header("Targetting")]
    public Transform target;
    public Transform bendTarget;
    
    [Header("Quailty Settings")]
    public int solverIterations = 5;
    public float solvedDistanceThreshold = 0.001f;

    [Header("Movement Settings")]
    public float nodeMoveSpeed = -1.0f;
    public float rotMoveSpeed = -1.0f;

    [Header("Movement Settings")]
    public AnimationCurve MoveGraph = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
    public AnimationCurve RotGraph = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);

    [Header("Debugging")]
    public bool debugMode = false;
    [Range(0.0f, 5.0f)]
    public float visualiserScale = 0.3f;

    void Awake()
    {
        if (target)
        {
            GameObject me = this.gameObject;
            boneStructure = new IKPlugin.BoneStructure(ref me, ref maxDepthSearch, ref target, ref bendTarget, ref solvedDistanceThreshold, ref solverIterations, ref nodeMoveSpeed, ref rotMoveSpeed, ref movementType, ref MoveGraph, ref RotGraph);
            me = null;
        }
        else
        {
            Debug.LogError($"No target set for {gameObject.name} cannot start IK System");
        }
    }

    void LateUpdate()
    {
        IKPlugin.IKStep(ref boneStructure);
    }

#if UNITY_EDITOR
    //Draw In Editor
    private void OnDrawGizmos()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            if (boneStructure == null)
            {
                Gizmos.DrawIcon(transform.position, "../EzyIK/Icons/EzyIKiconError.png", true);
            }
            else if (boneStructure.rootNode != null)
            {
                Gizmos.DrawIcon(boneStructure.rootNode.position, "../EzyIK/Icons/EzyIKicon.png", true);
            }
            else
            {
                Gizmos.DrawIcon(transform.position, "../EzyIK/Icons/EzyIKiconError.png", true);
            }
        }
        else
        {
            Gizmos.DrawIcon(transform.position, "../EzyIK/Icons/EzyIKicon.png", true);
        }

        if (debugMode)
        {
            Gizmos.color = Color.green;
            if (boneStructure != null)
            {
                for (int i = 0; i < boneStructure.boneNodes.Count; i++)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(boneStructure.boneNodes[i].nodeTransform.position, visualiserScale);
                    if (i > 0)
                    {
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawLine(boneStructure.boneNodes[i].nodeTransform.position, boneStructure.boneNodes[i - 1].nodeTransform.position);
                    }
                    if (boneStructure.bendTarget != null)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(boneStructure.bendTarget.position, visualiserScale);
                    }
                    if (boneStructure.target != null)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(boneStructure.target.position, visualiserScale);
                    }
                }
            }
        }
    }
#endif
}

//Custom Editor

#if UNITY_EDITOR
[CustomEditor(typeof(EzyIK))]
[CanEditMultipleObjects]
public class EzyIKEditor : Editor {

    SerializedProperty movementmode;
    SerializedProperty depth;
    SerializedProperty targetTransform;
    SerializedProperty bendTransform;
    SerializedProperty solveAmount;
    SerializedProperty arriveDis;
    SerializedProperty nodeMove;
    SerializedProperty nodeRot;
    SerializedProperty scaleDebug;
    SerializedProperty debugFlag;
    SerializedProperty rotGraph;
    SerializedProperty movGraph;
    
    //Setup
    void OnEnable()
    {
        movementmode = serializedObject.FindProperty("movementType");
        depth = serializedObject.FindProperty("maxDepthSearch");
        targetTransform = serializedObject.FindProperty("target");
        bendTransform = serializedObject.FindProperty("bendTarget");
        solveAmount = serializedObject.FindProperty("solverIterations");
        arriveDis = serializedObject.FindProperty("solvedDistanceThreshold");
        nodeMove = serializedObject.FindProperty("nodeMoveSpeed");
        nodeRot = serializedObject.FindProperty("rotMoveSpeed");
        scaleDebug = serializedObject.FindProperty("visualiserScale");
        debugFlag = serializedObject.FindProperty("debugMode");
        movGraph = serializedObject.FindProperty("MoveGraph");
        rotGraph = serializedObject.FindProperty("RotGraph");
    }

    //When the user is inside the insepector window
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (!UnityEditor.EditorApplication.isPlaying) {

            EditorGUILayout.PropertyField(movementmode);
            EditorGUILayout.PropertyField(depth);
            EditorGUILayout.PropertyField(targetTransform);
            EditorGUILayout.PropertyField(bendTransform);

            if (targetTransform.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("No Target Set", MessageType.Error);
            }
            if (depth.intValue < 3 && depth.intValue != -1)
            {
                EditorGUILayout.HelpBox("Depth Search Must Be Greater Than 2 or Set To -1", MessageType.Error);
            }

            EditorGUILayout.PropertyField(solveAmount);
            EditorGUILayout.PropertyField(arriveDis);

            if (solveAmount.intValue < 1)
            {
                EditorGUILayout.HelpBox("Solver Iteration Cannot Be Less Than 1", MessageType.Error);
            }
            if (arriveDis.floatValue < 0.0f)
            {
                EditorGUILayout.HelpBox("Arrive Distance Cannot Be Less Than 0", MessageType.Error);
            }

            //Switch between movement modes
            switch (movementmode.enumValueIndex)
            {
                case (int)IKPlugin.BoneStructure.MoveType.LINEAR:
                    {
                        EditorGUILayout.PropertyField(nodeMove, new GUIContent("Movement Speed"));
                        EditorGUILayout.PropertyField(nodeRot, new GUIContent("Rotation Speed"));
                        if (nodeMove.floatValue < 0.0f)
                        {
                            EditorGUILayout.HelpBox("Movement Speed Will Be Ignored As It Is A Negative Value", MessageType.Warning);
                        }
                        if (nodeRot.floatValue < 0.0f)
                        {
                            EditorGUILayout.HelpBox("Rotation Speed Will Be Ignored As It Is A Negative Value", MessageType.Warning);
                        }
                        break;
                    }
                case (int)IKPlugin.BoneStructure.MoveType.CUSTOM:
                    {
                        EditorGUILayout.PropertyField(movGraph, new GUIContent("Movement Arrival Graph"));
                        EditorGUILayout.PropertyField(rotGraph, new GUIContent("Rotational Arrival Graph"));
                        break;
                    }
                default:
                    {
                        Debug.LogError($"Unknown movementmode {movementmode}|{movementmode.enumValueIndex}");
                        break;
                    }
            }
        }

        else
        {
            EditorGUILayout.HelpBox("Values cannot be changed during runtime", MessageType.Info);

            //Stop Player if unsafe values
            if (targetTransform.objectReferenceValue == null || (depth.intValue < 3 && depth.intValue != -1) || solveAmount.intValue < 1 || arriveDis.floatValue < 0.0f) {
                Debug.LogError($"Invalid Settings On EzyIK Object: {serializedObject.targetObject.name}");
            }
        }

        EditorGUILayout.PropertyField(debugFlag);

        if (debugFlag.boolValue)
        {
            EditorGUILayout.PropertyField(scaleDebug);
        }

        serializedObject.ApplyModifiedProperties();
    }

    
}
#endif