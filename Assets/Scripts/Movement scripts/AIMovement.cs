using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
// Movement is in this script to seperate colliders from the same GameObject
public class AIMovement : MonoBehaviour {

    // Moving & Patrolling
    public AIBehaviour behaviour;
    public Transform rifleEnd;
    public Vector2 moveSpeed;
    [HideInInspector]public float speedMultiplier;

    public Transform[] patrolPoint;
    public float timeToMoveToNode;
    public float waitTimeAtNode;
    public float findNewPathFrequency;

    private Vector3 currentTarget;
    // State bools
    private bool seesPlayer = false;
    public bool inRange = false;

    private Transform player;
    private ButtonFunctionality bf;
    private AStarPathfinding aStar;
    private DatabaseHandler db;

    // Behaviour numerators
    public IEnumerator patrolling;
    public IEnumerator followPlayer;
    public bool isPatrollingRunning;
    public bool isFollowPlayerRunning;

    private Animator ani;
    private Rigidbody2D rb;
    private Vector3 oldPosition;
    public Item thisMob;

    private float attackTimer;

    // Use this for initialization
    void Start ()
    {
        db = GameObject.FindWithTag("Inventory").GetComponent<DatabaseHandler>();
        player = GameObject.FindWithTag("Player").transform;
        bf = GameObject.FindWithTag("UI").GetComponent<ButtonFunctionality>();
        aStar = GameObject.FindWithTag("MapBorder").GetComponent<AStarPathfinding>();

        ani = transform.parent.GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        thisMob = transform.parent.GetComponent<MobScript>().thisEnemy;

        followPlayer = FollowPlayer();
        patrolling = Patrol();

        if(behaviour.CanPatrol)
        {
            currentTarget = patrolPoint[0].position;
            StartCoroutine(patrolling);
        }

        if (thisMob.RangeAttackSpeed > 0)
            attackTimer = thisMob.RangeAttackSpeed;
        else if (thisMob.MeleeAttackSpeed > 0)
            attackTimer = thisMob.MeleeAttackSpeed;

        speedMultiplier = 100f;


    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if(bf.canPlay)
            CheckToMove();
	}

    public void CheckToMove()
    {
        // Attack
        if(inRange && behaviour.HasAimAttack)
        {
            if (!isAimingRunning)
                StartCoroutine(Aim());
        }
        else if (inRange && Time.time >= attackTimer)
        {
            behaviour.CanMove = false;

            if (thisMob.RangeAttackSpeed > 0)
            {
                attackTimer = thisMob.RangeAttackSpeed + Time.time;
                player.GetComponent<PlayerMovement>().playerStats.doDamage(thisMob.Power, db.StringToElement(thisMob.RangeElement));
            }
                
            else if (thisMob.MeleeAttackSpeed > 0)
            {
                attackTimer = thisMob.MeleeAttackSpeed + Time.time;
                player.GetComponent<PlayerMovement>().playerStats.doDamage(thisMob.Power, db.StringToElement(thisMob.MeleeElement));
            }

            KnockBack(player.GetComponent<Collider2D>());
            ani.SetBool("isAttacking", true);
            behaviour.CanMove = true;
        }
        else if(behaviour.CanMove)
        {
            // Move
            transform.parent.position = Vector2.MoveTowards(transform.position, currentTarget, (moveSpeed.x * Time.deltaTime) / 100f * speedMultiplier);
        }

        // Animator
        Vector3 direction = transform.position - oldPosition;

        bool isWalking = true;
        if (Mathf.Abs(direction.x) + Mathf.Abs(direction.y) > 0)
            isWalking = true;
        else isWalking = false;

        ani.SetBool("isWalking", isWalking);
        if (isWalking && !ani.GetBool("isAiming"))
        {
            ani.SetFloat("X", direction.x);
            ani.SetFloat("Y", direction.y);
        }
        oldPosition = transform.position;
    }


    public IEnumerator Patrol()
    {
        yield return new WaitForSeconds(1f);
        behaviour.CanMove = isPatrollingRunning = true;
        while(!seesPlayer)
        {
            foreach (Transform pos in patrolPoint)
            {
                List<Node> path = aStar.FindPath(transform.position, pos.position);

                if (path.Count > 0)
                {
                    foreach (Node node in path)
                    {
                        currentTarget = node.worldPosition;
                        yield return new WaitForSeconds(timeToMoveToNode);
                    }
                    yield return new WaitForSeconds(waitTimeAtNode);
                }
                else
                {
                    yield return new WaitForSeconds(waitTimeAtNode + timeToMoveToNode);
                }
            }
        }
        isPatrollingRunning = behaviour.CanMove = false;
    }

    public IEnumerator FollowPlayer()
    {
        behaviour.CanMove = isFollowPlayerRunning = true;
        while (seesPlayer && behaviour.CanMove)
        {
            List<Node> path = aStar.FindPath(transform.position, player.position);

            if (path.Count > 0)
            {
                foreach (Node node in path)
                {
                    currentTarget = node.worldPosition;
                    yield return new WaitForSeconds(timeToMoveToNode);
                }
            }
            else if(path.Count <= 1)
            {
                Debug.Log("Path is too short or empty");
                yield return new WaitForSeconds(1f);
            }
            
        }
        behaviour.CanMove = isFollowPlayerRunning = false;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            seesPlayer = true;

            if(isPatrollingRunning && behaviour.CanPatrol)
            {
                StopCoroutine(patrolling);
                patrolling = Patrol();
                isPatrollingRunning = behaviour.CanMove = false;
            }
            if(!isFollowPlayerRunning && behaviour.CanFollowPlayer)
            {
                Debug.Log("following player...");
                followPlayer = FollowPlayer();
                StartCoroutine(followPlayer);
            } 
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            seesPlayer = false;

            if(isFollowPlayerRunning && behaviour.CanFollowPlayer)
            {
                StopCoroutine(followPlayer);
                followPlayer = FollowPlayer();
                isFollowPlayerRunning = behaviour.CanMove = false;
            }
            if(!isPatrollingRunning && behaviour.CanPatrol)
            {
                patrolling = Patrol();
                StartCoroutine(patrolling);
            }
        }
    }

    void KnockBack(Collider2D col)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, col.transform.position - transform.position, 100);
        Debug.Log("trying to cast");
        //Debug.DrawRay(transform.position, (col.transform.position - transform.position) * 100);

        if (hit)
        {
            Debug.Log("adding force");
            Rigidbody2D rbPlayer = col.gameObject.GetComponent<Rigidbody2D>();
            rbPlayer.AddForce(-hit.normal * 500, ForceMode2D.Force);

            /*if(behaviour.BounceOffWhenKnockingBack)
            {
                transform.parent.GetComponent<Rigidbody2D>().AddForce(hit.normal * 10, ForceMode2D.Force);
            }*/
        }

        col.gameObject.GetComponent<PlayerMovement>().SlowPlayer((int)thisMob.SlowAmount, true);
    }

    private bool isAimingRunning;

    IEnumerator Aim()
    {
        isAimingRunning = true;
        behaviour.CanMove = false;

        while(inRange)
        {
            // aim animation
            ani.SetBool("isAiming", true);
            yield return new WaitForSeconds(thisMob.RangeAttackSpeed);
            
            // shoot
            if (inRange)
            {
                player.GetComponent<PlayerMovement>().playerStats.doDamage(thisMob.Power, db.StringToElement(thisMob.RangeElement));
                ani.SetBool("isAttacking", inRange);
            }   
            else break;
        }
        ani.SetBool("isAiming", false);
        isAimingRunning = false;
    }

    void Update()
    {
        if (isAimingRunning)
        {
            rifleEnd.GetComponent<LineRenderer>().enabled = true;
            RaycastHit2D hit = Physics2D.Raycast(rifleEnd.position, player.transform.position - transform.position, 100);
            //Debug.DrawRay(rifleEnd.position, player.transform.position - transform.position, Color.green);////////////////////////////
            rifleEnd.GetComponent<LineRenderer>().SetPosition(1, rifleEnd.position + Vector3.forward* -1);
            rifleEnd.GetComponent<LineRenderer>().SetPosition(0, player.position + Vector3.forward*-1);
        }
        if(!inRange && behaviour.HasAimAttack)
            rifleEnd.GetComponent<LineRenderer>().enabled = false;


    }
}
[System.Serializable]
public struct AIBehaviour
{
    public bool CanMove;
    public bool CanPatrol;
    public bool CanFollowPlayer;
    public bool HasAimAttack;
    public bool BounceOffWhenKnockingBack;
}
