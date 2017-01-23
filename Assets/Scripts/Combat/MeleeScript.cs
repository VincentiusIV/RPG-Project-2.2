using UnityEngine;
using System.Collections;

public class MeleeScript : MonoBehaviour
{
    // Public Fields
    public Melee thisData;

    private PolygonCollider2D polyCollider;
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        polyCollider = GetComponent<PolygonCollider2D>();
        polyCollider.enabled = false;
    }

    public void MeleeAttack()
    {
        StartCoroutine(CollEnabler());
    }

    IEnumerator CollEnabler()
    {
        polyCollider.enabled = true;
        yield return new WaitForSeconds(1f);
        polyCollider.enabled = false;
    }

	IEnumerator OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Hit with Melee on " + col.name);
        if(col.CompareTag("Enemy"))
        {
            MobScript e = col.GetComponent<MobScript>();
            e.enemyStats.doDamage(thisData.damage, thisData.element);
            polyCollider.enabled = false;

            // Knockback
            Rigidbody2D enemyRB = col.GetComponent<Rigidbody2D>();
            enemyRB.isKinematic = false;
            Vector3 forceDirection = (player.position + col.transform.position) / 2;
            enemyRB.velocity = -forceDirection * 5;
            yield return new WaitForSeconds(.5f);
            enemyRB.velocity = Vector3.zero;
            enemyRB.isKinematic = true;
        }
    }
}
