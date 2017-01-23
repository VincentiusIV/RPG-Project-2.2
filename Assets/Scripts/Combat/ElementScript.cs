using UnityEngine;
using System.Collections;

public class ElementScript : MonoBehaviour
{
    // Public & Serialized Fields
    public ElementType thisElement;
    public bool canSlow = false;
    public bool canDamage = false;

    // Private booleans
    [SerializeField]private Sprite[] states;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if(states.Length > 0)
            sr.sprite = states[0];
    }
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
                sr.sprite = states[1];
            }

            if(newElement == ElementType.ice && thisElement == ElementType.water)
            {
                Debug.Log("The ice has frozen "+gameObject.name);

            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") && canSlow)
            col.GetComponent<PlayerMovement>().SlowPlayer(40);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player") && canSlow)
            col.GetComponent<PlayerMovement>().SlowPlayer(100);
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
