using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    public float damage;
    public float speed;
    public float range;

    private Vector2 endPos;

    void Start()
    {
        //transform.parent = null;

        range += transform.position.z;
        StartCoroutine(DestroyTime());
    }

    void FixedUpdate()
    {
        transform.Translate(new Vector2(0f,speed / 10));
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 8 || coll.gameObject.layer == 9)
        {
            
            if (coll.gameObject.tag == "Player")
            {
                Debug.Log("hithihtihtihithitihtihit");
            }
            if (coll.gameObject.CompareTag("AI"))
            {
                Debug.Log("destroyed " + gameObject.name);
                Destroy(gameObject);
            }
            if (coll.gameObject.CompareTag("Breakable"))
            {
                Destroy(coll.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        // If bullet leaves DestroyRange trigger, bullet is destroyed
        if (col.CompareTag("DestroyRange"))
            Destroy(gameObject);
    }

    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(range/10);
        Destroy(this.gameObject);
    }
}
