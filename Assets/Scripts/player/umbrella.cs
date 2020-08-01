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
    private float Shotdamage = 0.0f;
    private List<string> saveddata = new List<string>() { };

    public bool inbossroom = false;
    public GameObject shotUI;

    public bool phoneLock = false;



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

            if (Shotdamage == 0 && shottoload + 1 <= 0)
            {
                shotUI.transform.GetChild(0).GetComponent<Text>().text = "No Shots Remaining";
            }
            else if (Shotdamage == 0)
            {
                shotUI.transform.GetChild(0).GetComponent<Text>().text = "Press Q To Load Picture";
            }
            else
            {
                shotUI.transform.GetChild(0).GetComponent<Text>().text = "Loaded, Click To Shoot";
            }

            shotUI.transform.GetChild(1).GetComponent<Text>().text = "Pictures left to load: " + (shottoload + 1).ToString();

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
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if (shottoload >= 0)
                {
                    if (Shotdamage == 0)
                    {
                        loadshot();
                    }
                }
            }
        }
    }

    public void bossroomtrigger()
    {
        for (int i = 0; i < 10; i++)
        {
            string filename = ("state " + (i).ToString() + ".png");
            //string picof = save.safeItem(filename, saveFile.types.STRING).tostring;
            string picof = SaveSystemController.getValue(filename);
            if (picof != "del")
            {
                saveddata.Add(picof);
            }
        }
        shottoload = saveddata.Count - 1;
        inbossroom = true;
    }

    private void loadshot()
    {
        bool pass = true;

        for (int j = 0; j < shottoload; j++)
        {
            if (saveddata[j] == saveddata[shottoload])
            {
                pass = false;
            }

            if (saveddata[shottoload] == "bad")
            {
                pass = false;
            }
        }

        if (pass == true)
        {
            Shotdamage = 100;
        }
        else
        {
            Shotdamage = -50;
        }

        shottoload -= 1;
    }

    void bang()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, enemy))
        {
            dodamage(hit.point, Shotdamage);
           
        }

        Shotdamage = 0.0f;
        movcont.strafemode = false;
        cooldown = true;
    }

    public void Hitbox(bool toggle)
    {
        umbrellaHitBox.enabled = toggle;
    }

    void dodamage(Vector3 pos, float attackingfor)
    {
        //COMMENTED OUT FOR MIGRATION NEEDS TO BE REBUILT FOR DIFFERENT ENEMYTYPES

        //float damage = boss.GetComponent<BossController>().IBeanShot(attackingfor);

        //GameObject text = Instantiate(damagedText, pos, Quaternion.identity);
        //text.transform.GetChild(0).GetComponent<Text>().text = null;
        //text.transform.GetChild(0).GetComponent<Text>().text = Mathf.RoundToInt(damage).ToString();
        //text.transform.LookAt(cam.transform.position);
        //text.transform.Rotate(new Vector3(0, 180, 0));

        GameObject tmp = GameObject.Instantiate(damagedText, pos, Quaternion.identity);
        tmp.transform.GetChild(0).GetComponent<Text>().text = attackingfor.ToString();
        boss.GetComponent<ReprisialOfFlameController>().health -= attackingfor;

        Debug.Log("attackign for " + attackingfor);
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
