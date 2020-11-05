using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballmaster : MonoBehaviour
{
    [Tooltip("add a bunch of gameobjects as childern to the master and their transforms will be used for positions")]


    [HideInInspector]
    public List<Vector3> positions = new List<Vector3>() { };
    public GameObject ball;
    public float spawnDistance = 1.0f;
    public float speed = 1.0f;
    [HideInInspector]
    public bool stop = false;

    private Vector3 initPos;

    void Start()
    {
        initPos = transform.position;
        int count = this.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            positions.Add(transform.GetChild(i).transform.position);
        }

        for (int i = 0; i < count; i++)
        {
            Destroy(transform.GetChild(i).transform.gameObject);
        }

        StartCoroutine(running());
    }

    private void LateUpdate()
    {
        transform.position = initPos;
    }

    public IEnumerator running()
    {
        GameObject tmp = GameObject.Instantiate(ball, this.transform.position, Quaternion.identity);
        tmp.transform.parent = this.transform;

        while (Vector3.Distance(transform.position, tmp.transform.position) < spawnDistance)
        {
            yield return null;
        } 

        if (stop == false)
        {
            StartCoroutine(running());
        }
        yield return null;

    }
}
