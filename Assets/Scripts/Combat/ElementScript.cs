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
            Elements newElement = col.GetComponent<BulletScript>().thisData.element;
            if (newElement == thisElement)
            {
                Debug.Log("The elements are identical");
            }

            if(newElement == Elements.fire && thisElement == Elements.oil)
            {
                Debug.Log("The fire has ignited the oil");
                // does dmg based on % of the hp of enemies and player and their resistances

                // look for  rigidbodies and their tags (enemy and player)
                
            }

            if(newElement == Elements.ice && thisElement == Elements.water)
            {
                Debug.Log("The ice has frozen "+gameObject.name);
                // does dmg based on % of the hp of enemies and player and their resistances

                // look for  rigidbodies and their tags (enemy and player)
            }
        }
    }
}

public enum Elements
{
    none,
    fire,
    earth,
    water,
    oil,
    ice,
    aether,
    electricity,
}
