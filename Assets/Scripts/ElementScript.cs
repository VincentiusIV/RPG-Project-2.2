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
            Elements newElement = col.GetComponent<BulletScript>().element;
            if (newElement == thisElement)
            {
                Debug.Log("The elements are identical");
            }

            if(newElement == Elements.fire && thisElement == Elements.oil)
            {
                Debug.Log("The fire has ignited the oil");
                
            }

            if(newElement == Elements.ice && thisElement == Elements.water)
            {
                Debug.Log("The ice has frozen "+gameObject.name);
            }
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
