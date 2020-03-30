using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitController : MonoBehaviour
{
    public List<Vector3> Phases = new List<Vector3>();
    public List<float> PhaseDelays = new List<float>();

    private bool phaseOverrride = false;
    private bool phaseBegin = false;
    private int nextID = 0;

    public void startPhases()
    {
        phaseBegin = true;
    }

    private void Start()
    {
        transform.localScale = Phases[0];
    }

    private void Update()
    {
        if (phaseBegin)
        {
            if (!phaseOverrride)
            {
                if (nextID < (Phases.Count - 1))
                {
                    nextID++;
                    StartCoroutine(invokePhase(nextID));
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator invokePhase(int ID)
    {
        Debug.Log(ID);
        phaseOverrride = true;
        Vector3 initalScale = transform.localScale;
        float t = 0.0f;
        for (float i = 0.0f; t < PhaseDelays[ID]; i++)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(initalScale, Phases[ID], (t / PhaseDelays[ID]));

            yield return null;
        }
        phaseOverrride = false;
        yield return null;
    }


}
