using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
// Movement is in this script to seperate colliders from the same GameObject
public class AIMovement : MonoBehaviour {

    // Moving & Patrolling
    public Vector2 moveSpeed;
    private Vector3 currentTarget;
    public Transform[] patrolPoint;
    public float timeToMoveToNode;
    public float waitTimeAtNode;
    public float findNewPathFrequency;

    // State bools
    private bool seesPlayer = false;
    public bool inRange = false;
    private bool isRanged;

    private Transform player;
    private ButtonFunctionality bf;
    private AStarPathfinding aStar;

    // Behaviour numerators
    public IEnumerator patrolling;
    public IEnumerator followPlayer;
    public bool isPatrollingRunning;
    public bool isFollowPlayerRunning;

    private Animator ani;
    private Rigidbody2D rb;
    private Vector3 oldPosition;
    private MobScript thisMob;

    private float attackTimer;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindWithTag("Player").transform;
        bf = GameObject.FindWithTag("UI").GetComponent<ButtonFunctionality>();
        aStar = GameObject.FindWithTag("MapBorder").GetComponent<AStarPathfinding>();
        currentTarget = patrolPoint[0].position;
        ani = transform.parent.GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        thisMob = transform.parent.GetComponent<MobScript>();

        followPlayer = FollowPlayer();
        patrolling = Patrol();
        StartCoroutine(patrolling);

        attackTimer = thisMob.attackSpeed;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if(bf.canPlay)
            CheckToMove();
	}

    public void CheckToMove()
    {
        transform.parent.position = Vector2.MoveTowards(transform.position, currentTarget, moveSpeed.x * Time.deltaTime);
        Vector3 direction = transform.position - oldPosition;
        // Animator
        bool isWalking = true;
        if (Mathf.Abs(direction.x) + Mathf.Abs(direction.y) > 0)
            isWalking = true;
        else isWalking = false;

        ani.SetBool("isWalking", isWalking);
        if (isWalking)
        {
            ani.SetFloat("X", direction.x);
            ani.SetFloat("Y", direction.y);
        }
        oldPosition = transform.position;

        if (inRange && Time.time >= attackTimer)
        {
            attackTimer = Time.time + thisMob.attackSpeed;
            player.GetComponent<PlayerMovement>().playerStats.doDamage(thisMob.enemyStats.power, thisMob.attackElement);
            ani.SetBool("isAttacking", true);
        }
    }


    public IEnumerator Patrol()
    {
        isPatrollingRunning = true;
        while(!seesPlayer)
        {
            foreach (Transform pos in patrolPoint)
            {
                List<Node> path = aStar.FindPath(transform.position, pos.position);

                if (path != null)
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
        isPatrollingRunning = false;
    }

    public IEnumerator FollowPlayer()
    {
        isFollowPlayerRunning = true;
        while (seesPlayer)
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
        isFollowPlayerRunning = false;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == player.tag)
        {
            seesPlayer = true;

            if(isPatrollingRunning)
            {
                StopCoroutine(patrolling);
                patrolling = Patrol();
                isPatrollingRunning = false;
            }
            if(!isFollowPlayerRunning)
            {
                StartCoroutine(followPlayer);
            } 
        }
    }

    IEnumerator OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == player.tag)
        {
            seesPlayer = false;

            if(isFollowPlayerRunning)
            {
                StopCoroutine(followPlayer);
                followPlayer = FollowPlayer();
                isFollowPlayerRunning = false;
            }
            if(!isPatrollingRunning)
            {
                yield return new WaitForSeconds(1f);
                StartCoroutine(patrolling);
            }
        }
    }
}
