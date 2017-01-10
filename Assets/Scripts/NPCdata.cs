using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCdata : MonoBehaviour
{
    public Item thisNPC = new Item();
    public int id;
    public bool isMerchant;

    private DatabaseHandler db;
    private Inventory inventory;
    public GameObject merchantSlot;
    public InventoryData[] invData;

    private GameObject inventoryPanel;
    private GameObject merchantPanel;
    private GameObject slotPanel;
    private GameObject notification;

    void Start()
    {
        notification = transform.GetChild(0).gameObject;
        notification.SetActive(false);

        db = GameObject.FindWithTag("Inventory").GetComponent<DatabaseHandler>();
        thisNPC = db.FetchItemByID(id);

        Sprite spriteData = Resources.Load<Sprite>("Sprites/" + thisNPC.Type + "/" + thisNPC.Slug);
        GetComponent<SpriteRenderer>().sprite = spriteData;

        if(isMerchant)
        {
            inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
            inventoryPanel = GameObject.Find("Inventory_Panel");
            merchantPanel = GameObject.Find("Merchant_Inventory_Panel");
            slotPanel = merchantPanel.transform.FindChild("Merchant_Slot_Panel").gameObject;

            if(merchantPanel.activeSelf)
            {
                //merchantPanel.SetActive(false);
            }
        }
    }
	
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player") && isMerchant)
        {
            notification.SetActive(true);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(Input.GetButtonDown("Interact"))
        {
            if(isMerchant)
            {
                CreateMerchantInventory();
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(notification.activeInHierarchy)
        {
            notification.SetActive(false);
        }
        else if(merchantPanel.activeInHierarchy)
        {
            merchantPanel.SetActive(false);
            inventoryPanel.SetActive(false);
        }
    }

    void CreateMerchantInventory()
    {
        for (int i = 0; i < slotPanel.transform.childCount; i++)
        {
            Destroy(slotPanel.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < invData.Length; i++)
        {
            merchantPanel.SetActive(true);
            inventoryPanel.SetActive(true);

            Item itemToAdd = db.FetchItemByID(invData[i].id);

            GameObject itemObj = Instantiate(merchantSlot) as GameObject;
            itemObj.transform.FindChild("Item").GetComponent<ItemData>().item = itemToAdd;
            itemObj.transform.SetParent(slotPanel.transform);
            itemObj.transform.position = slotPanel.transform.position;

            Sprite spriteData = Resources.Load<Sprite>("Sprites/" + itemToAdd.Type + "/" + itemToAdd.Slug);
            itemObj.transform.FindChild("Item").GetComponent<Image>().sprite = spriteData;

            itemObj.name = itemToAdd.Title;
            itemObj.transform.FindChild("Title_Text").GetComponent<Text>().text = itemToAdd.Title;
            itemObj.transform.FindChild("Cost_Text").GetComponent<Text>().text = itemToAdd.Value.ToString();
        }

        slotPanel.transform.SetParent(merchantPanel.transform);
    }
}

[System.Serializable]
public struct InventoryData
{
    public int id;
    public int amount;
}
