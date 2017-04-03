using UnityEngine;
using System.Collections;
// Author: Vincent Versnel
/* Script for elements to interact with eachother
 * unfortunately there was not enough time to implement more
 * interesting combos.
 * */
public class ElementScript : MonoBehaviour
{
    // Public & Serialized Fields
    public ElementType triggerElement;
    public ElementType thisElement;
    public ElementType unTriggerElement;

    public bool canSlow = false;
    public bool canDamage = false;

    private SpriteRenderer sr;
    private Animator ani;
    private bool triggered;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Element"))
        {
            // interaction between elements
        }

        if(col.CompareTag("Bullet"))
        {
            ElementType newElement = col.GetComponent<BulletScript>().ele;
            if (newElement == triggerElement)
            {
                //Triggered();
            }

            if(newElement == ElementType.fire && thisElement == ElementType.oil)
            {
                Debug.Log("The fire has ignited the oil");
            }

            if(newElement == ElementType.ice && thisElement == ElementType.water)
            {
                Debug.Log("The ice has frozen "+gameObject.name);

            }
        }
    }
    // Interacts with the player upon trigger
    // can slow, or trigger a new element state
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") && canSlow && triggered)
        {
            if (col.CompareTag("Player"))
                col.GetComponent<PlayerMovement>().SlowPlayer(40);
            else if (col.CompareTag("Enemy"))
                col.transform.GetChild(0).GetComponent<AIMovement>().speedMultiplier = 0f;
        }
        if(col.CompareTag("Element"))
        {
            col.GetComponent<ElementScript>().Triggered();
        }
    }
    // resets movement speed if the on trigger exit
    void OnTriggerExit2D(Collider2D col)
    {
        if (canSlow)
        {
            if (col.CompareTag("Player"))
                col.GetComponent<PlayerMovement>().SlowPlayer(100);
            else if (col.CompareTag("Enemy"))
                col.transform.GetChild(0).GetComponent<AIMovement>().speedMultiplier = 1;
        }
    }

    public void Triggered()
    {
        if(!triggered)
        {
            ani.SetBool("isTriggered", true);
            triggered = true;
            StartCoroutine(ReturnToDefault());
        }
    }
    // returns element to first state after 5 seconds
    IEnumerator ReturnToDefault()
    {
        yield return new WaitForSeconds(5f);
        ani.SetBool("isTriggered", false);
        triggered = false;

    }
    
}

public enum ElementType
{
    none = 7,
    fire = 0,
    earth = 5,
    water = 1,
    ice = 4,
    aether = 3,
    electricity = 2,
    oil = 6,
}
