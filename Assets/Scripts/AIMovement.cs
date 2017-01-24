using UnityEngine;
using System.Collections;

// Movement is in this script to seperate colliders from the same GameObject
public class AIMovement : MonoBehaviour {

    public Vector2 moveSpeed;

    // State bools
    private bool seesPlayer = false;
    private bool inRange = false;
    private bool isRanged;

    private GameObject player;
    private ButtonFunctionality bf;

    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player");
        bf = GameObject.FindWithTag("UI").GetComponent<ButtonFunctionality>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(bf.canPlay)
            CheckToMove();
	}

    public void CheckToMove()
    {
        if (seesPlayer && inRange && isRanged)
        {
            //shooting
        }
        if (seesPlayer && !isRanged)
        {
            //moving
            transform.parent.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed.x * Time.deltaTime);
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == player.tag)
            seesPlayer = inRange = true;
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == player.tag)
            seesPlayer = inRange = false;

        //followCounter = 4f;
    }
}
