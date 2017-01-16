﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    // Public Fields


    // Public & Hidden Fields
    [HideInInspector]public ItemData movingItem;
    [HideInInspector]public bool isMovingAnItem;
    [HideInInspector]public List<Item> items = new List<Item>();
    [HideInInspector]public List<GameObject> slots = new List<GameObject>();
    [HideInInspector]public int money;

    // Private & Hidden Reference Fields
    private GameObject hand;

    private DatabaseHandler database;
    private PlayerMovement player;
    private EventSystem e;

    // Private & Serialized Fields
    [SerializeField]private GameObject inventoryPanel;
    [SerializeField]private GameObject slotPanel;

    [SerializeField]private GameObject inventorySlot;
    [SerializeField]private GameObject inventoryItem;
    [SerializeField]private GameObject equipmentItem;
    [SerializeField]private GameObject[] equipmentSlots;

    [SerializeField]private int slotAmount;

    void Start()
    {
        database = GetComponent<DatabaseHandler>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        hand = GameObject.FindWithTag("Hand");

        //inventoryPanel = GameObject.Find("Inventory_Panel");
        inventoryPanel.SetActive(true);

        //slotPanel = inventoryPanel.transform.FindChild("Slot_Panel").gameObject;

        UpdateWallet(1000);

        // Creating basic slots
        for (int i = 0; i < slotAmount; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<Slot>().id = i;
            slots[i].transform.SetParent(slotPanel.transform);
        }
        // Updating slot id's from equipment slots;
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            items.Add(new Item());
            slots.Add(equipmentSlots[i]);
            equipmentSlots[i].GetComponent<Slot>().id = i + slotAmount;
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

        // Checks if item of the same type is in inventory, if it is stackable it will be stacked
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
        // else it will be dropped without stacking
        else
        {
            int emptySlot = SearchForEmptySlot();

            if(emptySlot == -1)
                return;

            items[emptySlot] = itemToAdd;
            GameObject itemObj = Instantiate(inventoryItem);
            itemObj.GetComponent<ItemData>().item = itemToAdd;
            itemObj.GetComponent<ItemData>().slotID = emptySlot;
            itemObj.transform.SetParent(slots[emptySlot].transform);
            itemObj.transform.position = slots[emptySlot].transform.position;

            itemObj.GetComponent<Image>().sprite = FetchSpriteBySlug(itemToAdd.Type, itemToAdd.Slug);
            itemObj.name = itemToAdd.Title;

            slots[emptySlot].GetComponent<Slot>().containsItem = true;
        }
    }

    public void RemoveItem(int id)
    {
        Item itemToRemove = database.FetchItemByID(id);
        // TO-DO write code to remove item from inventory
    }

    public void EquipItem(int id)
    {
        Item itemToEquip = database.FetchItemByID(id);
        Debug.Log(itemToEquip.Title + " " + itemToEquip.Slug);

        if (itemToEquip == null || itemToEquip.Type != "Items")
        {
            Debug.Log("Item with ID: " + id + " does not exist");
            return;
        }

        if(hand.transform.childCount > 0)
        {
            if (hand.transform.GetChild(0).CompareTag("Weapon"))
            {
                Destroy(hand.transform.GetChild(0).gameObject);
            }
        }

        if (itemToEquip.Stackable == false)
        {
            equipmentItem.GetComponent<SpriteRenderer>().sprite = FetchSpriteBySlug(itemToEquip.Type, itemToEquip.Slug);
            GameObject weapon = Instantiate(equipmentItem, hand.transform.position, hand.transform.rotation) as GameObject;
            weapon.transform.Rotate(new Vector3(0f, 0f, -90f));
            weapon.transform.SetParent(hand.transform);
            weapon.name = itemToEquip.Title;
            
            WeaponScript wepScript = weapon.GetComponent<WeaponScript>();

            wepScript.melee = new Melee(database.StringToElement(itemToEquip.MeleeElement),
                                         itemToEquip.Power,
                                         itemToEquip.MeleeAttackSpeed,
                                         itemToEquip.MeleeAttackRange);

            wepScript.projectile = new Projectile(database.StringToElement(itemToEquip.RangeElement),
                                            itemToEquip.Power,
                                            itemToEquip.RangeAttackSpeed,
                                            itemToEquip.RangeBulletSpeed,
                                            itemToEquip.RangeAttackRange);
            player.GetWeapon(wepScript);
        }
    }

    public void StartMovingItem(ItemData itemToMove)
    {
        // Setting state & position for moving
        isMovingAnItem = true;
        Debug.Log("Started moving an item: " + isMovingAnItem);

        movingItem = itemToMove;
        items[movingItem.slotID] = new Item();
        movingItem.transform.SetParent(movingItem.transform.parent.parent);
        movingItem.OnControllerDrag();
    }

    public void EndMovingItem(int new_slotID)
    {
        // Equips item on character if slot is equip slot
        if (slots[new_slotID].GetComponent<Slot>().type == slotType.weaponEquip)
        {
            Debug.Log("Equipping item...");
            EquipItem(movingItem.item.ID);
        }
        if(slots[new_slotID].GetComponent<Slot>().type == slotType.magicEquip)
        {
            // equip magic ability
        }

        // TO-DO Equip magic ability in specified slot

        // Resetting Old data
        isMovingAnItem = false;

        // Move item in array
        items[movingItem.slotID] = new Item();
        items[new_slotID] = movingItem.item;
        movingItem.slotID = new_slotID;

        // Placing held item onto slot
        movingItem.OnControllerDrop();
    }

    public void UpdateWallet(int change)
    {
        money += change;
        inventoryPanel.transform.FindChild("Wallet").GetComponent<Text>().text = "Wallet: " + money;
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

    public int SearchForEmptySlot()
    {
        int emptySlotID;

        for (int i = 0; i < items.Count; i++)
        {
            if (slots[i].GetComponent<Slot>().containsItem == false && slots[i].GetComponent<Slot>().type == slotType.regular)
            {
                emptySlotID = i;
                Debug.Log("First empty slot is: " + emptySlotID);
                return emptySlotID;
            }
        }
        Debug.LogError("Inventory is full");
        return emptySlotID = -1;
    }

    public Sprite FetchSpriteBySlug(string type, string slug)
    {
        Sprite spriteData = Resources.Load<Sprite>("Sprites/" + type + "/" + slug);
        return spriteData;
    }
}