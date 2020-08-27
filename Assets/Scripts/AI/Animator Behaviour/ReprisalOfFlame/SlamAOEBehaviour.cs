using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SlamAOEBehaviour : StateMachineBehaviour
{

    enum STAGES
    {
        SLAM,
        HIDDEN,
        FLAME,
        FINISHED
    }

    public GameObject flame;

    [Range(0.0f, 1.0f)]
    public float disapearTarget = 0.2f;
    [Range(0.0f, 1.0f)]
    public float spawnFlameTarget = 0.5f;
    [Range(0.0f, 1.0f)]
    public float reappearTarget = 0.6f;
    [Range(0.0f, 1.0f)]
    public float resumeAITarget = 0.9f;

    public float distanceBehindPlayerToLook = 10.0f;
    public float sizeOfAreaToSearch = 10.0f;

    public LayerMask obsctales;
    public LayerMask floor;

    AIObject ai;
    AIMovement movement;
    STAGES currentStage;
    float progress;
    Transform player;
    Transform playerCharTransform;
    Transform transform;
    List<Vector3> respawnLocs = new List<Vector3>();
    float colSize;

    Vector3 targetPos;
    GameObject flameInstance;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Init
        if (ai == null)
        {
            ai = animator.GetComponent<AIObject>();
            movement = ai.movement;
            player = ai.player.transform;
            playerCharTransform = ai.player.GetComponent<movementController>().charcterModel.transform;
            transform = ai.transform;
            colSize = ai.gameObject.GetComponent<CapsuleCollider>().radius;
            GameObject[] tmps = GameObject.FindGameObjectsWithTag("RespawnPoint");
            for (int i = 0; i < tmps.Length; i++)
            {
                respawnLocs.Add(tmps[i].transform.position);
            }
        }

        //Lock into state
        animator.SetBool("Attacking", true);

        //Stop movement for attack
        movement.setOverride(AIMovement.OVERRIDE.FULL_OVERRIDE);

        //Reset
        targetPos = Vector3.zero;
        currentStage = STAGES.SLAM;
        if (flameInstance != null)
        {
            Destroy(flameInstance);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        progress = stateInfo.normalizedTime % 1.0f;
        switch (currentStage)
        {
            case STAGES.SLAM:
                {
                    //Disappear Logic
                    if (progress >= disapearTarget)
                    {
                        //Spawn flame
                        flameInstance = Instantiate(flame, transform.position, Quaternion.identity);

                        //Disapear logic done in animation
                        currentStage = STAGES.HIDDEN;
                    }
                    Debug.LogWarning("Not Setup");
                    break;
                }
            case STAGES.HIDDEN:
                {
                    if (targetPos == Vector3.zero)
                    {
                        //Get position behind the player
                        Vector3 behindPlayerPos = player.position + (-playerCharTransform.forward * distanceBehindPlayerToLook);

                        Debug.DrawLine(behindPlayerPos, Vector3.up * 9999.0f, Color.cyan, 10.0f);

                        //Choose a random position behind the player
                        Vector3 rngPos = new Vector3(Random.Range(behindPlayerPos.x - sizeOfAreaToSearch, behindPlayerPos.x + sizeOfAreaToSearch), transform.position.y, Random.Range(behindPlayerPos.z - sizeOfAreaToSearch, behindPlayerPos.z + sizeOfAreaToSearch));

                        Debug.DrawLine(rngPos, Vector3.up * 9999.0f, Color.red, 10.0f);

                        //Check position for obsctcles and ground
                        //Ground is ground
                        //Boss Obstacle is a problem spot

                        if (Physics.CheckBox(rngPos, Vector3.one * colSize, Quaternion.identity))
                        {
                            Debug.Log("I git something");
                        }

                        //If we are hitting the ground
                        if (Physics.CheckBox(rngPos, Vector3.one * colSize, Quaternion.identity, floor))
                        {
                            //If we are not in a obsctacle
                            if (!(Physics.CheckBox(rngPos, Vector3.one * colSize, Quaternion.identity, obsctales)))
                            {
                                //Found valid position
                                Debug.Log("Found A Valid Pos and i'm happy boss :)");
                                Debug.DrawLine(rngPos, Vector3.up * 9999.0f, Color.green, 10.0f);
                                targetPos = rngPos;
                            }
                            else
                            {
                                Debug.Log("Found A Ground Pos but no go");
                                Debug.DrawLine(rngPos, Vector3.up * 9999.0f, Color.yellow, 10.0f);
                            }
                        }
                        else
                        {
                            Debug.Log("Found bad");
                            Debug.DrawLine(rngPos, Vector3.up * 9999.0f, Color.red, 10.0f);
                        }
                    }

                    //Flame visible logic
                    if (progress >= spawnFlameTarget)
                    {
                        //Pick random pos if it is still null
                        if (targetPos == Vector3.zero)
                        {
                            targetPos = respawnLocs[Random.Range(0, respawnLocs.Count)];
                        }

                        //Spawn flame
                        flameInstance = Instantiate(flame, targetPos, Quaternion.identity);


                        currentStage = STAGES.FLAME;
                    }
                    Debug.LogWarning("Not Setup");
                    break;
                }
            case STAGES.FLAME:
                {
                    //Reappear Logic
                    if (progress >= reappearTarget)
                    {
                        //Move the boss to flame
                        ai.transform.position = targetPos;
                        currentStage = STAGES.FINISHED;
                    }
                    Debug.LogWarning("Not Setup");
                    break;
                }
            case STAGES.FINISHED:
                {
                    //AI Resume Logic
                    if (progress >= resumeAITarget)
                    {
                        currentStage = STAGES.FINISHED;
                        //Leave state
                        animator.SetBool("Attacking", false);
                    }
                    //Debug.Break();
                    break;
                }
            default:
                {
                    Debug.LogWarning("Unknown Stage Of AOE Attack For Boss " + currentStage.ToString());
                    break;
                }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Resume movement
        movement.setOverride(AIMovement.OVERRIDE.NO_OVERRIDE);
        //Reset selected attack
        //ai.unbindAttack();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
