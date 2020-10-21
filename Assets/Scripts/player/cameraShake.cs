using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shakeOperation
{
    public enum lerpModes 
    {
        LINEAR, //string line
        OUTSINE, //gradual, sin function, slows towards the end
        INSINE, //gradual, sin function, slow to start off
        INOUTSINE, //gradual, sin function, slow to start and slow to finish but faster in the middle
        INEXPO, //expodential sharp growth, slow start fast finish
        OUTEXPO,    //expodential sharp growth, fast start slow finish
        INOUTEXPO, //expodential sharp growth, slow start slow finish, fast middle 
    }

    public Vector3 pos;
    public Vector3 rot;
    public float speedOverall;
    public float speedin;
    public float speedout;

    public lerpModes funcCurrent;
    public lerpModes funcin;
    public lerpModes funcout;

    //do not touch
    public float completion = 0.0f;
    public bool pastHalfWay = false;

}

public class cameraShake : MonoBehaviour
{
    float timer;
    public Vector3 passTargetPos;
    public Vector3 passTargetRot;
    public float passOverallSpeed;
    public shakeOperation.lerpModes funcin;
    public shakeOperation.lerpModes funcout;
    public float speedIn;
    public float speedOut;

    public List<shakeOperation> OP = new List<shakeOperation>() { };

    public bool Test = false;
    public int shakenumber;
    private int hitcycle = 0;


    public enum Modes
    { 
        NONE,
        EXPLODE,
        SAKE,
        SHOTGUN,
        WALKING,
        SPRINTING,
        WHACK,
    }

    public Modes active;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Test = true;
        }

        shakeupdate();

        switch (active)
        {
            case Modes.NONE:
                {
                    if (Test)
                    {
                        Test = false;
                        addOperation(passTargetPos, passTargetRot, passOverallSpeed, funcin, funcout, speedIn, speedOut);
                    }
                    break;
                }
            case Modes.EXPLODE:
                {
                    if (Test)
                    {
                        Test = false;
                        StartCoroutine(explode(2.0f));

                    }

                    break;
                }           
            case Modes.SAKE:
                {
                    timer += Time.deltaTime;
                    if (timer > 1.0f)
                    {
                        timer = 0.0f;
                        passTargetRot = new Vector3(Random.Range(10.0f, -10.0f), Random.Range(10.0f, -10.0f), Random.Range(10.0f, -10.0f));
                        passOverallSpeed = Random.Range(0.1f, 0.5f);
                        passTargetPos = new Vector3(Random.Range(0.2f, -0.2f), Random.Range(0.2f, -0.2f), Random.Range(0.2f, -0.2f));
                        addOperation(passTargetPos, passTargetRot, passOverallSpeed);
                    }
                    break;
                }
            case Modes.SHOTGUN:
                {
                    if (Test)
                    {
                        Test = false;

                        passTargetPos = new Vector3(0.0f, 0.1f, -0.3f);
                        passOverallSpeed = 3.0f;
                        passTargetRot = new Vector3(-3.0f, 2.0f, 0.0f);
                        funcin = shakeOperation.lerpModes.OUTEXPO;
                        funcout = shakeOperation.lerpModes.INSINE;
                        speedIn = 5000.0f;
                        speedOut = 1.0f;

                        addOperation(passTargetPos, passTargetRot, passOverallSpeed, funcin, funcout, speedIn, speedOut);
                    }

                    break;
                }
            case Modes.SPRINTING:
                {
                    timer += Time.deltaTime;
                    if (timer > 0.285)
                    {
                        timer = 0.0f;

                        if (shakenumber == 0)
                        {
                            shakenumber = 1;

                            passTargetRot = new Vector3(0.0f, 0.0f, 0.0f);
                            passOverallSpeed = 3.50877192982f;
                            passTargetPos = new Vector3(0.02f, 0.01f, 0.0f);
                            addOperation(passTargetPos, passTargetRot, passOverallSpeed, shakeOperation.lerpModes.OUTSINE, shakeOperation.lerpModes.INSINE, 1.0f, 1.0f);
                        }
                        else if (shakenumber == 1)
                        {
                            shakenumber = 0;

                            passTargetRot = new Vector3(0.0f, 0.0f, 0.0f);
                            passOverallSpeed = 3.50877192982f;
                            passTargetPos = new Vector3(-0.02f, 0.01f, 0.0f);
                            addOperation(passTargetPos, passTargetRot, passOverallSpeed, shakeOperation.lerpModes.OUTSINE, shakeOperation.lerpModes.INSINE, 1.0f, 1.0f);
                        }
                        else
                        {
                            shakenumber = 0;
                        }


                    }

                    break;
                }
            case Modes.WALKING:
                {
                    timer += Time.deltaTime;
                    if (timer > 0.36)
                    {
                        timer = 0.0f;
                        timer = 0.0f;
                        Debug.Log("cross");

                        if (shakenumber == 0)
                        {
                            shakenumber = 1;

                            passTargetRot = new Vector3(0.0f, 0.0f, 0.0f);
                            passOverallSpeed = 2.77777f;
                            passTargetPos = new Vector3(0.02f, -0.01f, 0.0f);
                            addOperation(passTargetPos, passTargetRot, passOverallSpeed, shakeOperation.lerpModes.OUTSINE, shakeOperation.lerpModes.INSINE, 1.0f, 1.0f);
                        }
                        else if (shakenumber == 1)
                        {
                            shakenumber = 0;

                            passTargetRot = new Vector3(0.0f, 0.0f, 0.0f);
                            passOverallSpeed = 2.77777f;
                            passTargetPos = new Vector3(-0.02f, -0.01f, 0.0f);
                            addOperation(passTargetPos, passTargetRot, passOverallSpeed, shakeOperation.lerpModes.OUTSINE, shakeOperation.lerpModes.INSINE, 1.0f, 1.0f);
                        }
                        else
                        {
                            shakenumber = 0;
                        }
                    }

                    break;
                }
            case Modes.WHACK:
                {
                    if (Test == true)
                    {
                        Test = false;
                        hitcycle++;

                        if (hitcycle > 2)
                        {
                            hitcycle = 0;
                        }

                        if (hitcycle == 0)
                        {
                            passTargetPos = new Vector3(-0.05f, -0.1f, 0.0f);
                            passOverallSpeed = 10.0f;
                            passTargetRot = new Vector3(2.0f, -1.0f, 0.0f);
                            funcin = shakeOperation.lerpModes.INEXPO;
                            funcout = shakeOperation.lerpModes.OUTEXPO;
                            speedIn = 1.5f;
                            speedOut = 0.1f;

                        }
                        else if (hitcycle == 1)
                        {
                            passTargetPos = new Vector3(0.1f, 0.0f, 0.0f);
                            passOverallSpeed = 10.0f;
                            passTargetRot = new Vector3(-0.5f, 2.0f, 0.0f);
                            funcin = shakeOperation.lerpModes.INEXPO;
                            funcout = shakeOperation.lerpModes.OUTEXPO;
                            speedIn = 1.5f;
                            speedOut = 0.1f;
                        }
                        else if (hitcycle == 2)
                        {
                            passTargetPos = new Vector3(0.0f, 0.0f, 0.1f);
                            passOverallSpeed = 10.0f;
                            passTargetRot = new Vector3(2.0f, 0.0f, 0.0f);
                            funcin = shakeOperation.lerpModes.INEXPO;
                            funcout = shakeOperation.lerpModes.OUTEXPO;
                            speedIn = 1.0f;
                            speedOut = 0.1f;
                        }

                        addOperation(passTargetPos, passTargetRot, passOverallSpeed, funcin, funcout, speedIn, speedOut);


                    }
                    break;
                }
            default:
                {
                    break;
                }
        }

    }

    public void shakeupdate()
    {
        Vector3 targetPos = Vector3.zero;
        Vector3 targetRot = Vector3.zero;

        for (int i = 0; i < OP.Count; i++)
        {
            if (!OP[i].pastHalfWay)
            {
                OP[i].completion += Time.deltaTime * OP[i].speedOverall * OP[i].speedin;
            }
            else
            {
                OP[i].completion += Time.deltaTime * OP[i].speedOverall * OP[i].speedout;
            }


            if (OP[i].completion > 1.0f)
            {
                if (OP[i].pastHalfWay == true)
                {

                    OP.RemoveAt(i);
                    i--;
                    continue;
                }

                OP[i].funcCurrent = OP[i].funcout;
                OP[i].pastHalfWay = true;
                OP[i].completion = 0.0f;
            }


            float x = OP[i].completion;

            switch (OP[i].funcCurrent)
            {
                case shakeOperation.lerpModes.LINEAR:
                    {
                        break;
                    }
                case shakeOperation.lerpModes.OUTSINE:
                    {
                        x = Mathf.Sin((x * Mathf.PI) / 2.0f);
                        break;
                    }
                case shakeOperation.lerpModes.INSINE:
                    {
                        x = 1.0f - Mathf.Cos((x * Mathf.PI) / 2.0f);
                        break;
                    }
                case shakeOperation.lerpModes.INOUTSINE:
                    {
                        x = -((Mathf.Cos(Mathf.PI * x) - 1.0f) / 2.0f);
                        break;
                    }
                case shakeOperation.lerpModes.INEXPO:
                    {
                        x = Mathf.Pow(2.0f, (10.0f * x) - 10.0f);
                        break;
                    }
                case shakeOperation.lerpModes.OUTEXPO:
                    {
                        x = 1.0f - Mathf.Pow(2.0f, (-10.0f * x));
                        break;
                    }
                case shakeOperation.lerpModes.INOUTEXPO:
                    {
                        if (x < 0.5f)
                        {
                            x = (Mathf.Pow(2.0f, 20.0f * x - 10.0f)) / 2.0f;
                        }
                        else
                        {
                            x = (2.0f - (Mathf.Pow(2.0f, -20.0f * x + 10.0f))) / 2.0f;
                        }
                        break;
                    }
                default:
                    {
                        Debug.LogWarning("camera shake operation does not have a valid selected function");
                        break;
                    }
            }

            if (!OP[i].pastHalfWay)
            {
                targetPos += Vector3.Lerp(Vector3.zero, OP[i].pos, x);
                targetRot += Vector3.Lerp(Vector3.zero, OP[i].rot, x);
            }
            else
            {
                targetPos += Vector3.Lerp(OP[i].pos, Vector3.zero, x);
                targetRot += Vector3.Lerp(OP[i].rot, Vector3.zero, x);
            }
        }

        transform.localPosition = targetPos;
        transform.localRotation = Quaternion.Euler(targetRot);
    }

    //adds operation to the list to be competed
    public void addOperation(Vector3 pPos, Vector3 pRot, float pSpeed)
    {
        addOperation(pPos, pRot, pSpeed, shakeOperation.lerpModes.LINEAR, shakeOperation.lerpModes.LINEAR, 1.0f, 1.5f);
    }

    public void addOperation(Vector3 targetPos, Vector3 targetRot, float overallSpeed, shakeOperation.lerpModes funcin, shakeOperation.lerpModes funcout, float speedIn, float speedOut)
    {
        shakeOperation tmp = new shakeOperation();
        tmp.pos = targetPos;
        tmp.rot = targetRot;
        tmp.speedOverall = overallSpeed;
        tmp.speedin = speedIn;
        tmp.speedout = speedOut;
        tmp.funcin = funcin;
        tmp.funcout = funcout;
        tmp.funcCurrent = tmp.funcin;

        OP.Add(tmp);
    }


    public IEnumerator explode(float range)
    {

        Debug.Log("call");
        float tmp = Random.Range(2.0f, 18.0f);
        passTargetPos = Vector3.zero;
        passOverallSpeed = 3.0f;
        passTargetRot = new Vector3(-0.75f * range, 0.5f * range, 0.0f);
        funcin = shakeOperation.lerpModes.LINEAR;
        funcout = shakeOperation.lerpModes.INSINE;
        speedIn = 20.0f - tmp;
        speedOut = 1.5f;
        addOperation(passTargetPos, passTargetRot, passOverallSpeed, funcin, funcout, speedIn, speedOut);

        yield return new WaitForSeconds(0.2f);

        passTargetRot = new Vector3(Random.Range(1.0f, -1.0f) * range, Random.Range(1.0f, -1.0f) * range, Random.Range(1.0f, -1.0f) * range);
        passTargetPos = new Vector3(Random.Range(0.03f, -0.03f) * range, Random.Range(0.03f, -0.03f) * range, Random.Range(0.03f, -0.03f) * range);

        funcin = shakeOperation.lerpModes.LINEAR;
        funcout = shakeOperation.lerpModes.LINEAR;
        speedIn = tmp;
        speedOut = 3.0f;

        addOperation(passTargetPos, passTargetRot, passOverallSpeed, funcin, funcout, speedIn, speedOut);

        
        yield return null;
    }
}
