using UnityEngine;
using System.Collections;

public class MeleeScript : MonoBehaviour
{
    // Public Fields
    public Melee thisData;

    private PolygonCollider2D collider;

    void Start()
    {
        collider = GetComponent<PolygonCollider2D>();
        collider.enabled = false;
    }

    public void MeleeAttack()
    {
        StartCoroutine(CollEnabler());
    }

    IEnumerator CollEnabler()
    {
        collider.enabled = true;
        yield return new WaitForFixedUpdate();
        collider.enabled = false;
    }
	void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Hit with Melee on " + col.name);
        if(col.CompareTag("Enemy"))
        {
            MobScript e = col.GetComponent<MobScript>();
            e.enemyStats.doDamage(thisData.damage, thisData.element);
        }
    }
}
