using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
    private PlayerMovement playerScript;
    private EnemyScript enemyScript;

    void Start() {
        if (transform.GetComponentInParent<PlayerMovement>() != null) {
            playerScript = transform.GetComponentInParent<PlayerMovement>();
        }
        if (transform.GetComponentInParent<EnemyScript>() != null) {
            enemyScript = transform.GetComponentInParent<EnemyScript>();
        }
        transform.parent = null;
    }

    private void OnTriggerEnter2D(Collider2D coll){
        if (coll.gameObject.layer == 8) {
            Destroy(gameObject);
            if (coll.gameObject.tag == "Player") {
                coll.gameObject.GetComponent<PlayerMovement>().doDmg(enemyScript.dmg);
            }
            if (coll.gameObject.tag == "AI") {
                coll.gameObject.GetComponent<EnemyScript>().doDmg(playerScript.dmg);
            }
            if (coll.gameObject.tag == "Breakable") {
                Destroy(coll.gameObject);
                //Spawn Loot???
            }
        }
    }
}
