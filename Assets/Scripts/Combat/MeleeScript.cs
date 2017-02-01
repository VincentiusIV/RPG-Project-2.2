using UnityEngine;
using System.Collections;

public class MeleeScript : MonoBehaviour
{
    // Public Fields
    public int weaponID;
    public int damage;
    public int attackSpeed;

    //private Melee thisData = new Melee();

    private PolygonCollider2D polyCollider;
    private Transform player;
    //private DatabaseHandler db;

    private float nextAtt;

    void Start()
    {
        //db = GameObject.FindWithTag("Inventory").GetComponent<DatabaseHandler>();
        player = GameObject.FindWithTag("Player").transform;
        polyCollider = GetComponent<PolygonCollider2D>();
        polyCollider.enabled = false;
    }

    public bool MeleeAttack()
    {
        if(Time.time > nextAtt)
        {
            Debug.Log("melee attack");
            
            nextAtt = Time.time + attackSpeed;
            StartCoroutine(CollEnabler());
            return true;
        }
        return false;
    }

    IEnumerator CollEnabler()
    {
        polyCollider.enabled = true;
        yield return new WaitForSeconds(.5f);
        polyCollider.enabled = false;
    }

	IEnumerator OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Hit with Melee on " + col.name);
        if(col.CompareTag("Enemy"))
        {
            MobScript e = col.GetComponent<MobScript>();
            e.enemyStats.doDamage(damage, ElementType.none);
            polyCollider.enabled = false;

            // Knockback
            Rigidbody2D enemyRB = col.GetComponent<Rigidbody2D>();
            if(enemyRB != null)
            {
                enemyRB.bodyType = RigidbodyType2D.Dynamic;
                Vector3 forceDirection = col.transform.position - player.position;
                enemyRB.AddForce(forceDirection.normalized * 10);
                yield return new WaitForSeconds(.5f);
                if(enemyRB != null)
                {
                    enemyRB.bodyType = RigidbodyType2D.Kinematic;
                    enemyRB.velocity = Vector3.zero;
                }
            }
        }
    }
}
