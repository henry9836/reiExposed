using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class umbrella : MonoBehaviour
{
    public bool canfire = false;
    public float blockingStamina;
    public bool cooldown = false;
    public float cooldowntime = 2.0f;
    public float cooldowntimer = 0.0f;
    public LayerMask enemy;
    public LayerMask ball;
    public GameObject damagedText;
    public bool ISBLockjing = false;
    public List<AudioClip> swishSounds = new List<AudioClip>();
    public AudioClip umbrellaActivateSFX;
    public AudioClip umbrellaShoot;
    public BoxCollider umbrellaHitBox;
    public GameObject umbeaalBone;
    public GameObject boss;
    public AudioSource audio;

    private PlayerController playercontrol;
    private GameObject cam;
    private Animator animator;
    public GameObject VFX;

    private movementController movcont;

    private bool latetest = false;

    private int shottoload = -999;
    public float Shotdamage = 15.0f;

    public bool inbossroom = false;
    public GameObject shotUI;

    public bool phoneLock = false;

    //shotty
    public float bulletSpread = 0.1f;
    public float MaxRange = 15.0f;
    public float MaxDamage = 25.0f;
    public float pellets = 8.0f;
    public GameObject xinsButthole;




    void Start()
    {
        playercontrol = GetComponent<PlayerController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        animator = playercontrol.gameObject.GetComponent<Animator>();
        if (!audio)
        {
            audio = GetComponent<AudioSource>();
        }
        umbrellaHitBox.enabled = false;

        movcont = GetComponent<movementController>();

    }

    void Update()
    {
        latetest = false;

        if (Input.GetMouseButtonDown(0) && !animator.GetBool("Blocking") && !phoneLock)
        {
            if (playercontrol.staminaAmount >= playercontrol.staminaToAttack)
            {
                movcont.attacking = true;
                playercontrol.ChangeStamina(-playercontrol.staminaToAttack);
                animator.SetTrigger("Attack");
            }
        }

        VFX.GetComponent<VisualEffect>().SetFloat("timer", 0.0f);


        if (cooldown == false && !phoneLock)
        {
            if (Input.GetAxis("Fire2") > 0.5f)
            {
                if (playercontrol.staminaAmount > blockingStamina * Time.deltaTime)
                {
                    playercontrol.ChangeStamina(-blockingStamina * Time.deltaTime);
                    blocking();

                    firemode();
                    
                }
                else
                {
                    movcont.strafemode = false;
                    shotUI.SetActive(false);

                    cooldown = true;
                }
            }
            else
            {
                movcont.strafemode = false;
                shotUI.SetActive(false);

                //animator.ResetTrigger("Block");
                animator.SetBool("Blocking", false);
                movcont.attacking = false;
            }
        }
        else
        {
            shotUI.SetActive(false);

            animator.SetBool("Blocking", false);
            movcont.attacking = false;
            cooldowntimer += Time.deltaTime;
            if (cooldowntimer > cooldowntime)
            {
                //animator.ResetTrigger("Block");
                cooldown = false;
                cooldowntimer = 0.0f;
            }
        }

    }

    void blocking()
    {
        movcont.attacking = true;
        movcont.strafemode = true;

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, ball))
        {
            hit.point = new Vector3(hit.point.x, movcont.charcterModel.transform.position.y, hit.point.z); //look forwards
            movcont.charcterModel.transform.LookAt(hit.point);
        }

        if (!animator.GetBool("Blocking"))
        {
            animator.SetTrigger("Block");
            animator.SetBool("Blocking", true);
        }
    }

    void firemode()
    {
        movcont.attacking = true;
        latetest = true;
        VFX.GetComponent<VisualEffect>().SetFloat("timer", 1.0f);

        if (inbossroom == true)
        {
            shotUI.SetActive(true);
            shotUI.transform.GetChild(0).GetComponent<Text>().text = "E to take photo";


            if (Input.GetAxis("Fire1") > 0.5f)
            {
                animator.SetTrigger("Shoot");
                bang();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {

                cooldown = true;
                VFXController vfx = boss.GetComponent<VFXController>();
                for (int i = 0; i < boss.GetComponent<VFXController>().bodysNoVFX.Count; i++)
                {
                    if (vfx.bodysNoVFX[i].GetComponent<BossRevealSurfaceController>())
                    {
                        if (vfx.bodysNoVFX[i].GetComponent<BossRevealSurfaceController>().isPlayerLookingAtMe())
                        {
                            vfx.bodysNoVFX[i].GetComponent<BossRevealSurfaceController>().EnableSurface();
                            vfx.bodysNoVFX.RemoveAt(i);
                        }
                    }
                    else
                    {
                        vfx.bodysNoVFX.RemoveAt(i);
                    }

                }
            }
        }
    }

    void bang()
    {
        //RaycastHit hit;
        //if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, enemy))
        //{
        //    dodamage(hit, Shotdamage);
        //}

        Transform brella = this.transform.GetChild(1).GetChild(6);
        var cameraThingTransform = cam.transform.parent.parent.transform;
        for (int j = 0; j < pellets; j++)
        {
            RaycastHit[] Hits;

            float yrand = Random.Range(bulletSpread, -bulletSpread);
            float xrand = Random.Range(Mathf.Sqrt(Mathf.Pow(bulletSpread, 2) - Mathf.Pow(yrand, 2)), -Mathf.Sqrt(Mathf.Pow(bulletSpread, 2) - Mathf.Pow(yrand, 2)));
            Vector3 vec3dir = new Vector3(xrand, yrand, 1);

            // ELIJAH
            //float angleDegFromCentreOutwards = Random.Range(0, 10);
            //float rollAngleDeg = Random.Range(0, 360);
            //Quaternion fromCentreOutwards = Quaternion.Euler(angleDegFromCentreOutwards, 0, 0); // this one pointing straight or downwards
            //Quaternion rollQuat = Quaternion.Euler(0, 0, rollAngleDeg);
            //Quaternion combined = rollQuat * fromCentreOutwards; // first change pitch, then change roll around forwards axis
            //Vector3 alongUmbrellaLocal = combined * Vector3.forward;
            //float cameraPitch = cameraThingTransform.localEulerAngles.x;
            //Vector3 localDirection = Quaternion.Euler(cameraPitch, 0, 0) * alongUmbrellaLocal;
            //Quaternion correction = Quaternion.Euler(0, -90, 0);
            //Vector3 worldDirection = brella.TransformDirection(correction * localDirection);
            //Debug.DrawRay(brella.position, worldDirection * 5.0f, Color.magenta, 1.0f);
            // ELIJAH END

            //Quaternion fromCentreOutwards = Quaternion.Euler(xrand, yrand, 0); // this one pointing straight or downwards
            //Vector3 alongUmbrellaLocal = vec3dir + Vector3.forward;
            Vector3 localDirection = Quaternion.Euler(cameraThingTransform.localEulerAngles.x, 0, 0) *  vec3dir;
            Quaternion correction = Quaternion.Euler(0, -90, 0);
            Vector3 worldDirection = brella.TransformDirection(correction * localDirection);



            Hits = Physics.RaycastAll(brella.transform.position, worldDirection, MaxRange);

            for (int k = 0; k < Hits.Length; k++)
            {
                Debug.DrawLine(brella.transform.position, Hits[k].point, Color.white, 10.0f);
            }

            for (int i = 0; i < Hits.Length; i++)
            {
                RaycastHit Hit = Hits[i];
                var go = Hit.collider.gameObject;
                if (go.CompareTag("Myth") || go.CompareTag("Boss"))
                {
                    GameObject enemy = Hit.collider.gameObject;
                    Vector3 hitposition = Hit.point + (Hit.normal * 0.4f);

                    float dist = Vector3.Distance(this.gameObject.transform.position, Hit.point);
                    float falloff = Mathf.Clamp(1.5f * Mathf.Cos(Mathf.Pow(dist / MaxRange, 0.3f) * (Mathf.PI / 2)), 0.0f, 1.0f);
                    float damage = falloff * (MaxDamage / pellets);

                    if (Hit.collider.GetComponent<AIObject>())
                    {
                        GameObject tmp = GameObject.Instantiate(damagedText, Hit.point, Quaternion.identity);
                        tmp.transform.SetParent(Hit.collider.gameObject.transform, true);
                        tmp.transform.GetChild(0).GetComponent<Text>().text = "-" + damage.ToString("F0");
                        Hit.collider.GetComponent<AIObject>().health -= damage;

                        Debug.Log("attackign for " + damage);
                    }
                    else if (Hit.collider.GetComponent<EnemyController>())
                    {
                        GameObject tmp = GameObject.Instantiate(damagedText, Hit.point, Quaternion.identity);
                        tmp.transform.SetParent(Hit.collider.gameObject.transform, true);
                        tmp.transform.GetChild(0).GetComponent<Text>().text = "-" + damage.ToString("F0");
                        Hit.collider.GetComponent<EnemyController>().ChangeHealth(-damage);

                        Debug.Log("attackign for " + damage);
                    }
                }
                Debug.Log("bang");

                if (Hit.collider.gameObject.layer == 0) //ground and wall
                {
                    Debug.Log("hole");

                    Quaternion hitRotation = Quaternion.FromToRotation(Vector3.forward, Hit.normal);
                    Vector3 hitposition = Hit.point + (Hit.normal * 0.01f);

                    GameObject hole = Instantiate(xinsButthole, hitposition, hitRotation);
                }
            }
        }





        movcont.strafemode = false;
        cooldown = true;
    }

    public void Hitbox(bool toggle)
    {
        umbrellaHitBox.enabled = toggle;
    }


    void LateUpdate()
    {
        if (latetest == true)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, ball))
            {
                umbeaalBone.transform.LookAt(hit.point);
                umbeaalBone.transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
                Debug.DrawLine(hit.point, cam.transform.position);
            }
        }
    }
}
