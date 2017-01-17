using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    public Projectile thisData;

    private Vector2 endPos;

    void Start()
    {
        //transform.parent = null;
        //range += transform.position.z;
        StartCoroutine(DestroyTime());
        Debug.Log("bullet speed:" + thisData.bulletSpeed);
    }

    void Update()
    {
        transform.Translate(new Vector2(0f,(float)thisData.bulletSpeed));
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
                coll.gameObject.GetComponent<EnemyScript>().doDmg(thisData.damage);
                Destroy(gameObject);
                return;
            }
            if (coll.gameObject.CompareTag("Breakable"))
            {
                Destroy(coll.gameObject);
                return;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        // If bullet leaves DestroyRange trigger, bullet is destroyed
        //if (col.CompareTag("DestroyRange"))
        //    Destroy(gameObject);
    }

    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(thisData.range/10);
        Destroy(this.gameObject);
    }
}
