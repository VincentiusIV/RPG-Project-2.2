using UnityEngine;
using System.Collections;

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
