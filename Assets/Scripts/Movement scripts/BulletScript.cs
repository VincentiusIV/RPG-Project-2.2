using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D coll){
        if (coll.gameObject.layer == 8) {
            Destroy(gameObject);
            if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "AI") {
                coll.gameObject.GetComponent<HealthScript>().doDMG(20);
            }
            if (coll.gameObject.tag == "Breakable") {
                Destroy(coll.gameObject);
                //Spawn Loot???
            }
        }
    }
}
