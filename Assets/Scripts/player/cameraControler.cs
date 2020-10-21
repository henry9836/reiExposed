using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraControler : MonoBehaviour
{
    private GameObject camPivot;
    private GameObject camRoot;
    private Camera mainCam;

    private string mouseXInputName = "Mouse X", mouseYInputName = "Mouse Y";
    public float mouseSensitivity = 1.0f;

    [SerializeField] private float lerpSpeed = 12f;
    public AnimationCurve distCurve;
    public AnimationCurve FOVCurve;
    public float pitchValueAdj, zOffset, hitDistance;
    public float maxDistance = 5f;
    public float maxPitchDown = 60f;
    public float maxPitchUp = 50f;
    public float minFOV = 60f;
    public float maxFOV = 75f;
    private float xAxisRot;

    [SerializeField] LayerMask obstacleLayers;

    private bool isAiming;
    public float aimFOV = 55f;
    public float aimDistance = 3f;

    private float fovLerp, zOffsetLerp;
    private float ADStimer = 0.0f;
    private float zOffsetColl;
    private float oldfov;
    private float mouseX;
    private float mouseY;
    private bool FOVonce = true;
    public GameObject umbrella;

    private bool cooldownlock;
    private float fov;
    private GameObject pausemenu;

    public GameObject rei;

    private GameObject targetgo;
    public bool camtargetlock = false;
	public GameObject targetSphere;
	public float maxlockdistance = 15.0f;
    public float maxlockdistanceBoss = 30.0f;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        camPivot = transform.GetChild(0).gameObject;
        camRoot = transform.GetChild(0).GetChild(0).gameObject;
        mainCam = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Camera>();
        pausemenu = GameObject.Find("pauseMenu");
        rei = this.transform.root.gameObject;
    }

    void Update()
    {
        cooldownlock = transform.parent.GetComponent<umbrella>().cooldown;

        pitchValueAdj = Mathf.DeltaAngle(camPivot.transform.localRotation.eulerAngles.x, 360.0f - maxPitchUp) / -(maxPitchUp + maxPitchDown);
        zOffset = Mathf.Lerp(2.0f, maxDistance, distCurve.Evaluate(pitchValueAdj));

        if ((Input.GetAxis("Fire2") > 0.5f) && (cooldownlock == false) && !rei.GetComponent<umbrella>().phoneLock && (rei.GetComponent<umbrella>().phoneTimer > rei.GetComponent<umbrella>().phoneThreshold))
        {
            if (FOVonce == true)
            {
                FOVonce = false;
                oldfov = mainCam.fieldOfView;
            }

            if (ADStimer < 1.0f)
            {
                ADStimer += Time.unscaledDeltaTime * lerpSpeed;
            }
            else
            {
                ADStimer = 1.0f;
            }

            fov = Mathf.Lerp(oldfov, aimFOV, ADStimer);
            ObstacleCheck(true);
            zOffsetColl = -(Mathf.Clamp(zOffset, 1.0f, aimDistance));

        }
        else
        {
            if (FOVonce == false)
            {
                FOVonce = true;
                oldfov = mainCam.fieldOfView;
            }

            if (ADStimer > 0.0f)
            {
                ADStimer -= Time.unscaledDeltaTime * lerpSpeed;
            }
            else
            {
                ADStimer = 0.0f;
            }

            fov = Mathf.Lerp(minFOV, oldfov, ADStimer);
            ObstacleCheck(false);
            zOffsetColl = -(Mathf.Clamp(zOffset, 1.0f, hitDistance));

        }


        camRoot.transform.localPosition = new Vector3(Mathf.Lerp(0.0f, 0.4f, ADStimer), 0.0f, zOffsetColl);

        mainCam.fieldOfView = fov;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (camtargetlock == false)
            {
                GameObject[] tmp1 = GameObject.FindGameObjectsWithTag("Myth");
                GameObject[] tmp2 = GameObject.FindGameObjectsWithTag("Dummy");
                GameObject[] tmp3 = GameObject.FindGameObjectsWithTag("Boss");

                List<GameObject> targets = new List<GameObject>() { };

                for (int i = 0; i < tmp1.Length; i++)
                {
                    targets.Add(tmp1[i]);
                }
                for (int i = 0; i < tmp2.Length; i++)
                {
                    targets.Add(tmp2[i]);
                }
                for (int i = 0; i < tmp3.Length; i++)
                {
                    targets.Add(tmp3[i]);
                }

                float closest = maxlockdistance;
                int closestobj = -1;

                for (int i = 0; i < targets.Count; i++)
                {
                    float dist = Vector3.Distance(targets[i].transform.position, rei.transform.position);

                    if (targets[i].tag == "Boss") //if checking the boss
                    {
                        if (dist < maxlockdistanceBoss) //if within larger boss range
                        {
                            if (closestobj == -1) // and nothign is aggigned then assign
                            {
                                Debug.Log("boss1");
                                closest = dist;
                                closestobj = i;
                            }
                            else if (dist < closest) //somthing else is already assigned them compare
                            {
                                Debug.Log("boss2");

                                closest = dist;
                                closestobj = i;
                            }
                        }
                    }
                    else
                    {
                        if (dist < closest)
                        {
                            closest = dist;
                            closestobj = i;
                        }
                    }


                }

                if (closestobj != -1)
                {
                    targetgo = targets[closestobj];
                    camtargetlock = true;
					targetSphere.SetActive(true);
                }
				
            }
            else
            {
                camtargetlock = false;
				targetSphere.SetActive(false);

            }

        }

        if (camtargetlock == false)
        {
            CameraRotation();
        }
        else
        {
            if (targetgo == null) 
            {
                camtargetlock = false;
				targetSphere.SetActive(false);
                CameraRotation();
            }
            else if (targetgo.tag == "Boss")
            {
                if (Vector3.Distance(rei.transform.position, targetgo.transform.position) > maxlockdistanceBoss)
                {
                    camtargetlock = false;
                    targetSphere.SetActive(false);
                    CameraRotation();
                }
                else
                {
                    cameraLockOn(targetgo);
                }
            }
            else
            {
                if (Vector3.Distance(rei.transform.position, targetgo.transform.position) > maxlockdistance)
                {
                    camtargetlock = false;
                    targetSphere.SetActive(false);
                    CameraRotation();
                }
                else
                {
                    cameraLockOn(targetgo);
                }
            }
        }
    }

    private void cameraLockOn(GameObject target)
    {
        Vector3 targetpos = target.transform.position;
        if (target.tag == "Boss")
        {
            targetpos += new Vector3(0.0f, 2.0f, 0.0f);
        }
        else
        {
            targetpos += new Vector3(0.0f, 1.0f, 0.0f);

        }

        targetSphere.transform.position = targetpos;
        Quaternion targetRotation = Quaternion.LookRotation(targetpos - mainCam.transform.position);
        Vector3 tmp = targetRotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0.0f, tmp.y, 0.0f);
        camPivot.transform.localRotation = Quaternion.Euler(tmp.x, 0.0f, 0.0f);

		
    }
    private void CameraRotation()
    {
        mouseX = Input.GetAxisRaw(mouseXInputName) * mouseSensitivity * 100.0f * Time.deltaTime;
        mouseY += (Input.GetAxisRaw(mouseYInputName) * -1.0f) * mouseSensitivity * 100.0f * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        mouseY = Mathf.Clamp(mouseY, -maxPitchDown, maxPitchDown);
        camPivot.transform.localRotation = Quaternion.Euler(mouseY, 0, 0);
    }

    private void ObstacleCheck(bool ADS)
    {
        Vector3 origin = camPivot.transform.position + camPivot.transform.right * 0.0f + camPivot.transform.up * 0.0f;
        Vector3 direction = -camPivot.transform.forward;
        RaycastHit hit;


        if (Physics.Raycast(origin, direction, out hit, zOffset, obstacleLayers))
        {
            Debug.DrawLine(origin, hit.point, Color.yellow);

            if (ADS == true)
            {
                aimDistance = hit.distance;
            }
            else
            {
                hitDistance =  hit.distance;
            }
        }
        else
        {
            if (ADS == true)
            {
                aimDistance = zOffset - 1.0f;
            }
            else
            {
                hitDistance = zOffset;
            }
        }
    }
}