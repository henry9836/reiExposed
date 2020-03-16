using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControler : MonoBehaviour
{
    private GameObject camPivot;
    private GameObject camRoot;
    private Camera mainCam;

    private string mouseXInputName = "Mouse X", mouseYInputName = "Mouse Y";
    [SerializeField] private float mouseSensitivity = 180f;

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
    private Vector3 startCam;
    private Vector3 aimcam;
    public float ADStimer = 0.0f;
    private float zOffsetColl;
    private float oldfov;
    private bool FOVonce = true;

    
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        camPivot = transform.GetChild(0).gameObject;
        camRoot = transform.GetChild(0).GetChild(0).gameObject;
        mainCam = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Camera>();

        startCam = transform.localPosition;
        aimcam = startCam + new Vector3(0.4f, 0.1f, 0.0f);
    }

    void Update()
    {
        float fov; 
        pitchValueAdj = Mathf.DeltaAngle(camPivot.transform.localRotation.eulerAngles.x, 360.0f - maxPitchUp) / -(maxPitchUp + maxPitchDown);
        zOffset = Mathf.Lerp(2.0f, maxDistance, distCurve.Evaluate(pitchValueAdj));

        if (Input.GetAxis("Fire2") > 0.5f)
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
        Time.timeScale = Mathf.Lerp(1.0f, 0.3f, ADStimer);
        mainCam.fieldOfView = fov;
        CameraRotation();
    }

    private void CameraRotation()
    {
        float mouseX = Mathf.Clamp(Input.GetAxisRaw(mouseXInputName) * mouseSensitivity * Time.unscaledDeltaTime, -50f, 50f);
        float mouseY = Mathf.Clamp(Input.GetAxisRaw(mouseYInputName) * mouseSensitivity * Time.unscaledDeltaTime, -50f, 50f);

        // Clamp and smooth vertical rotation
        xAxisRot += mouseY;

        if (xAxisRot > maxPitchUp)
        {
            xAxisRot = maxPitchUp;
            mouseY = 0f;
            ClampXaxisRotationToValue(360f - maxPitchUp);
        }
        else if (xAxisRot < -maxPitchDown)
        {
            xAxisRot = -maxPitchDown;
            mouseY = 0f;
            ClampXaxisRotationToValue(maxPitchDown);
        }

        camPivot.transform.Rotate(Vector3.left * mouseY);
        transform.Rotate(Vector3.up * mouseX);

        // Camera roataion axis failsafe
        Quaternion q1 = transform.rotation;
        q1.eulerAngles = new Vector3(0, q1.eulerAngles.y, 0);
        transform.rotation = q1;

        Quaternion q2 = camPivot.transform.localRotation;
        q2.eulerAngles = new Vector3(q2.eulerAngles.x, 0, 0);
        camPivot.transform.localRotation = q2;
    }

    private void ClampXaxisRotationToValue(float value)
    {
        Vector3 eulerRotation = camPivot.transform.eulerAngles;
        eulerRotation.x = Mathf.Lerp(eulerRotation.x, value, Time.unscaledDeltaTime * lerpSpeed);
        camPivot.transform.eulerAngles = eulerRotation;
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
                hitDistance = hit.distance;
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