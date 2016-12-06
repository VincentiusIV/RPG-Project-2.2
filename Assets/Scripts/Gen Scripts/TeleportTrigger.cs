using UnityEngine;
using System.Collections;

public class TeleportTrigger : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            //reload lvl and move player to opposite side of the lvl
            GeneratingDungeon gameManagerScript = FindObjectOfType<GeneratingDungeon>();
            gameManagerScript.DestroyLevel();
            gameManagerScript.GenerateLevel();
            //gameManagerScript.RespawnPlayer();
            col.gameObject.transform.position = new Vector2(0f, 0f);
        }
    }
}
