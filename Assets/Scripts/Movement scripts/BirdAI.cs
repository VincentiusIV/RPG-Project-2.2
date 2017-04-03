using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// Author: Vincent Versnel
// AI for birds to fly together
public class BirdAI : MonoBehaviour {

    public Vector3 velocity;
    private List<GameObject> nearBirds = new List<GameObject>();
    Flock flock = new Flock();

    private Rigidbody2D rb;
    private Animator anime;
    private bool visible;
    private float destroyCounter = 10f;

    void Start()
    {
        velocity = new Vector3(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
        rb = GetComponent<Rigidbody2D>();
        flock = GameObject.Find("BirdManager").GetComponent<Flock>();
        anime = GetComponent<Animator>();
        StartCoroutine(UpdateVel());
    }

    void FixedUpdate()
    {
        //UpdateVelocity();
        Anime();
    }

    IEnumerator UpdateVel()
    {
        while (true)
        {
            UpdateVelocity();
            yield return new WaitForSeconds(.4f);
        }
    }
    void UpdateVelocity()
    {
        Vector3 cohesion = new Vector3();
        Vector3 alignment = new Vector3();
        Vector3 seperation = new Vector3();

        foreach (GameObject go in nearBirds)
        {
            Debug.DrawLine(transform.position, go.transform.position);
            cohesion += go.transform.position;
            alignment += go.GetComponent<BirdAI>().velocity;
            seperation += transform.position - go.transform.position;
        }

        if (nearBirds.Count != 0)
        {
            cohesion /= nearBirds.Count;
            alignment /= nearBirds.Count;
        }

        cohesion -= transform.position;

        velocity += cohesion * flock.cohesionWeight;
        velocity += alignment * flock.alignmentWeight;
        velocity += seperation * flock.seperationWeight;

        if (velocity.magnitude > flock.maxSpeed)
        {
            velocity = velocity.normalized * flock.maxSpeed;
        }
        Anime();
        //transform.Translate(velocity * Time.deltaTime, Space.World);
        rb.velocity = velocity;
    }

    void Anime()
    {
        anime.SetFloat("X", velocity.x);
        anime.SetFloat("Y", velocity.y);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Bird"))
        {
            nearBirds.Add(col.gameObject);
            UpdateVelocity();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Bird"))
        {
            nearBirds.Remove(col.gameObject);
            UpdateVelocity();
        }
    }

    void OnBecameInvisible()
    {
        visible = false;
    }

    void OnBecameVisible()
    {
        visible = true;
    }

    IEnumerator BirdRespawner()
    {
        while (true)
        {
            if(visible)
            {
                destroyCounter = 10f;
            }
            else if(!visible)
            {

            }
        }
    }
}
