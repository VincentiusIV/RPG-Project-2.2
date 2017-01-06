using UnityEngine;
using System.Collections;

public class MeleeScript : MonoBehaviour
{
// Public Fields
    public int dmg;

	void OnTriggerStay(Collider2D col)
    {
        if(col.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<EnemyScript>().doDmg(dmg);
            gameObject.SetActive(false);
        }
    }
}
