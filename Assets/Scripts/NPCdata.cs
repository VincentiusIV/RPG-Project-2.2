using UnityEngine;
using System.Collections;

public class NPCdata : MonoBehaviour
{
    public Item thisNPC = new Item();
    public int id;
    private DatabaseHandler db;
    private GameObject notification;

    void Start()
    {
        notification = transform.FindChild("HelpText").gameObject;
        notification.SetActive(false);
        db = GameObject.FindWithTag("Inventory").GetComponent<DatabaseHandler>();
        thisNPC = db.FetchItemByID(id);

        Sprite spriteData = Resources.Load<Sprite>("Sprites/" + thisNPC.Type + "/" + thisNPC.Slug);
    }
	
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Collided with " + col.gameObject.name);
        notification.SetActive(true);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(Input.GetButtonDown("Interact"))
        {
            Debug.Log("You interacted with NPC: " + thisNPC.Title);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        notification.SetActive(false);
    }
    public void OpenTrade()
    {

    }
}
