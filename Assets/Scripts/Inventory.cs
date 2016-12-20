using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
// Public Fields
    public List<Item> items = new List<Item>();

// Public & Hidden Fields
    [HideInInspector]public List<GameObject> slots = new List<GameObject>();
    [HideInInspector]public int money;

// Private & Hidden Reference Fields
    private GameObject inventoryPanel;
    private GameObject slotPanel;
    private GameObject hand;

    private DatabaseHandler database;

// Private & Serialized Fields
    [SerializeField]private GameObject inventorySlot;
    [SerializeField]private GameObject inventoryItem;
    [SerializeField]private GameObject equipmentItem;

    [SerializeField]private int slotAmount;

    void Start()
    {
        database = GetComponent<DatabaseHandler>();

        inventoryPanel = GameObject.Find("Inventory_Panel");
        inventoryPanel.SetActive(true);

        slotPanel = inventoryPanel.transform.FindChild("Slot_Panel").gameObject;

        hand = GameObject.FindWithTag("Hand");

        UpdateWallet(1000);

        for (int i = 0; i < slotAmount; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<Slot>().id = i;
            slots[i].transform.SetParent(slotPanel.transform);
        }
    }
	
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            EquipItem(2);
        }
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

    public void EquipItem(int id)
    {
        Item itemToEquip = database.FetchItemByID(id);

        if (itemToEquip == null || itemToEquip.Type != "Items")
        {
            Debug.Log("Item with ID: " + id + " does not exist");
            return;
        }

        if (CheckIfItemIsInInventory(itemToEquip) && itemToEquip.Stackable == false)
        {
            GameObject weapon = Instantiate(equipmentItem, hand.transform.position, Quaternion.identity) as GameObject;
            weapon.transform.SetParent(hand.transform);
            weapon.name = itemToEquip.Title;

            WeaponScript wepScript = weapon.GetComponent<WeaponScript>();

        }
    }

    public void UpdateWallet(int change)
    {
        money += change;
        inventoryPanel.transform.FindChild("Wallet").GetComponent<Text>().text = "Wallet: " + money;
    }
}
