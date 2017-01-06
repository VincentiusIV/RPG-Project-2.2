using UnityEngine;
using System.Collections;

public class MeleeScript : MonoBehaviour
{
// Public Fields
    public int dmg;

	void OnTriggerStay2D(Collider2D col)
    {
        if(col.CompareTag("AI"))
        {
            col.gameObject.GetComponent<EnemyScript>().doDmg(dmg);
            gameObject.SetActive(false);
        }
    }
}
