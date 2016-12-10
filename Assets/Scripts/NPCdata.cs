using UnityEngine;
using System.Collections;

public class NPCdata : MonoBehaviour
{
    [HideInInspector]public Item thisNPC;
    private DatabaseHandler db;

	void Awake ()
    {
	    db = GameObject.FindWithTag("Inventory").GetComponent<DatabaseHandler>();
    }
	

}
