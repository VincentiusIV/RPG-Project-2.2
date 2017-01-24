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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<MobScript>().enemyStats.doDamage(thisData.damage, thisData.element);
            Destroy(gameObject);
        }
        if (col.gameObject.layer == 8 || col.gameObject.layer == 9)
        {
            if (col.gameObject.tag == "Player")
            {
                Debug.Log("hithihtihtihithitihtihit");
            }
            
            if (col.gameObject.CompareTag("Breakable"))
            {
                Destroy(col.gameObject);
                return;
            }
        }
    }
}
