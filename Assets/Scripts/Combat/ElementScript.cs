using UnityEngine;
using System.Collections;

public class ElementScript : MonoBehaviour
{
// Public & Serialized Fields
    [SerializeField]public ElementType thisElement;

    // Private booleans
    bool canSlow = false;
    bool canDamage = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Element"))
        {
            // interaction between elements
        }

        if(col.CompareTag("Bullet"))
        {
            //col.GetComponent<BulletScript>().
            ElementType newElement = col.GetComponent<BulletScript>().thisData.element;
            if (newElement == thisElement)
            {
                Debug.Log("The elements are identical");
            }

            if(newElement == ElementType.fire && thisElement == ElementType.oil)
            {
                Debug.Log("The fire has ignited the oil");
                // does dmg based on % of the hp of enemies and player and their resistances

                // look for  rigidbodies and their tags (enemy and player)
            }

            if(newElement == ElementType.ice && thisElement == ElementType.water)
            {
                Debug.Log("The ice has frozen "+gameObject.name);
                // does dmg based on % of the hp of enemies and player and their resistances

                // look for  rigidbodies and their tags (enemy and player)
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            col.GetComponent<PlayerMovement>().SlowPlayer(40, 1);
    }

    bool CheckForCollsions(Collider2D col)
    {
        return true;
    }
}

public enum ElementType
{
    none = 7,
    fire = 0,
    earth = 1,
    water = 2,
    ice = 3,
    aether = 4,
    electricity = 5,
    oil = 6,
}
