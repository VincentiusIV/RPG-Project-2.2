using UnityEngine;
using System.Collections;

public class ElementScript : MonoBehaviour
{
// Public & Serialized Fields
    [SerializeField]public Elements thisElement;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Element"))
        {
            
            // interaction between elements
        }

        if(col.CompareTag("Bullet"))
        {
            //col.GetComponent<BulletScript>().
        }
    }
}

public enum Elements
{
    fire,
    earth,
    water,
    oil,
    ice,
    aether
}
