using UnityEngine;
using System.Collections;

public class DroneAI : MonoBehaviour
{
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

    void Start ()
    {
        targetToFollow = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        follow = true;
	}

	void FixedUpdate ()
    {
        Vector3 movement = (targetToFollow.position + transform.position) / 2;
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, movement.x));

        if (follow)
        {
            transform.position = Vector3.Lerp(transform.position, targetToFollow.transform.position + targetOffset, followDelay);
        }
        if(collectEnergy)
        {

        }
	}
}
