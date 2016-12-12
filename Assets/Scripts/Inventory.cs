using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    GameObject inventoryPanel;
    GameObject slotPanel;
    DatabaseHandler database;
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    [SerializeField]private int slotAmount;
    public List<Item> items = new List<Item>();
    [HideInInspector]public List<GameObject> slots = new List<GameObject>();

    void Start()
    {
        database = GetComponent<DatabaseHandler>();
        inventoryPanel = GameObject.Find("Inventory_Panel");
        slotPanel = inventoryPanel.transform.FindChild("Slot_Panel").gameObject;
        inventoryPanel.SetActive(true);
        for (int i = 0; i < slotAmount; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<Slot>().id = i;
            slots[i].transform.SetParent(slotPanel.transform);
        }

        AddItem(0);
        AddItem(1);
        inventoryPanel.SetActive(false);
    }
	
    public void AddItem(int id)
    {
        Item itemToAdd = database.FetchItemByID(id);
        if(itemToAdd == null || itemToAdd.Type != "Items")
        {
            Debug.Log("Item with ID: " + id + " does not exist");
            return;
        }

        if(itemToAdd.Stackable && CheckIfItemIsInInventory(itemToAdd))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if(items[i].ID == id)
                {
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == -1)
                {
                    items[i] = itemToAdd;
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<ItemData>().item = itemToAdd;
                    itemObj.GetComponent<ItemData>().slotID = i;
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.position = slots[i].transform.position;

                    Sprite spriteData = Resources.Load<Sprite>("Sprites/" + itemToAdd.Type + "/" + itemToAdd.Slug);
                    itemObj.GetComponent<Image>().sprite = spriteData;

                    itemObj.name = itemToAdd.Title;
                    break;
                }
            }
        }
    }

    bool CheckIfItemIsInInventory(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == item.ID)
                return true;
        }
        return false;
    }
}
