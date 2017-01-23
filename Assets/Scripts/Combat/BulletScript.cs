using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    public Projectile thisData;

    private Vector2 endPos;

    void Start()
    {
        Destroy(gameObject, thisData.range / 100);
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
}
