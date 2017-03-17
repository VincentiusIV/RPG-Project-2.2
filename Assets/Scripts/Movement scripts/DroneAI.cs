using UnityEngine;
using System.Collections;
// Author: Vincent Versnel
// Basic follow instructions for the drone.
// can switch target to for example target an energy probe instead when interacting
public class DroneAI : MonoBehaviour
{
    public Sprite[] bodySprites;
    public Transform targetToFollow;
    public Vector3 targetOffset;
    public float followDelay;
    public float moveSpeed;
    public float fillPercentage;

    // State bools
    public bool follow;
    public bool reachedPlayer;
    public bool collectEnergy;

    // Private variables
    private Rigidbody2D rb;
    private float tilt = 0;
    private Vector2 oldPosition;
    private SpriteRenderer sr;

    void Start ()
    {
        targetToFollow = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        follow = true;
        oldPosition = transform.position;
	}

	void FixedUpdate ()
    {
        Vector3 movement = (targetToFollow.position + transform.position) / 2;

        if (follow)
        {
            transform.position = Vector3.Lerp(transform.position, targetToFollow.transform.position + targetOffset, followDelay);
        }
        if(collectEnergy)
        {

        }
	}

    void Update()
    {
        Vector2 distance = new Vector2(transform.position.x, transform.position.y) - oldPosition;
        // right
        if (distance.x > 0 && distance.y < .5f && distance.y > -.5f)
            sr.sprite = bodySprites[0];

        //left 
        if (distance.x < 0 && distance.y < .5f && distance.y > -.5f)
            sr.sprite = bodySprites[1];

        // down
        if (distance.y < 0 && distance.x < .5f && distance.x > -.5f)
            sr.sprite = bodySprites[2];

        oldPosition = transform.position;
    }
}
