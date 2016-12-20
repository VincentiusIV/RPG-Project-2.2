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
    }

    void FixedUpdate()
    {
        transform.Translate(new Vector2(0f,speed / 10));
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 8 || coll.gameObject.layer == 9)
        {
            Destroy(gameObject);
            if (coll.gameObject.tag == "Player")
            {
                Debug.Log("hithihtihtihithitihtihit");
            }
            if (coll.gameObject.tag == "AI")
            {

            }
            if (coll.gameObject.CompareTag("Breakable"))
            {
                Destroy(coll.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("DestroyRange"))
            Destroy(gameObject);
    }
}
