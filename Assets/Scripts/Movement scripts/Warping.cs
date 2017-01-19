using UnityEngine;
using System.Collections;

public class Warping : MonoBehaviour {

    [SerializeField]private Transform targetWarp;
	
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            col.transform.position = targetWarp.position;
        }
    }
}
