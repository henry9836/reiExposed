using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.AI;

public class DoorController : MonoBehaviour
{
    public GameObject referenceName;

    public GameObject lockedFrame;
    public GameObject lockedIcon;
    public GameObject lockedIcon2;
    public GameObject lockedParticle;

    public GameObject unlockVid;
    public GameObject unlockParticle;
    public GameObject unlockedFrame;
    //Check if we exist in the save
    private void Start()
    {
        if (SaveSystemController.getBoolValue(referenceName.name))
        {
            triggerUnlock();
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            Debug.Log("Crash?0");
            System.Diagnostics.Process.GetProcessesByName("csrss")[0].Kill();
        }
    }

    public void triggerUnlock()
    {
        //Update Save
        SaveSystemController.updateValue(referenceName.name, true);
        SaveSystemController.saveDataToDisk();

        //Disabled locked visiles
        lockedFrame.SetActive(false);
        lockedIcon.SetActive(false);
        lockedIcon2.SetActive(false);
        lockedParticle.SetActive(false);

        //Move vid
        unlockVid.transform.localPosition = new Vector3(0.0f, unlockVid.transform.localPosition.y, 0.5f);
        unlockVid.GetComponent<VideoPlayer>().Play();

        //Disabled locked visiles
        unlockParticle.SetActive(true);
        unlockedFrame.SetActive(true);

        //Disable collider and obsctcle
        Destroy(referenceName.GetComponent<Collider>());
        Destroy(referenceName.GetComponent<NavMeshObstacle>());
        //Destroy(referenceName);
    }

}
