﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
// Public Fields
    public ItemData movingItem;
    public bool isMovingAnItem;

// Public & Hidden Fields
    [HideInInspector]public List<Item> items = new List<Item>();
    [HideInInspector]public List<GameObject> slots = new List<GameObject>();
    [HideInInspector]public int money;

// Private & Hidden Reference Fields
    private GameObject inventoryPanel;
    private GameObject slotPanel;
    private GameObject hand;

    private DatabaseHandler database;
    private PlayerMovement player;
    private EventSystem e;

// Private & Serialized Fields
    [SerializeField]private GameObject inventorySlot;
    [SerializeField]private GameObject inventoryItem;
    [SerializeField]private GameObject equipmentItem;
    [SerializeField]private GameObject equipmentSlot;

    [SerializeField]private int slotAmount;
    [SerializeField]private int equipmentSlotAmount;

    void Start()
    {
        movingItem = null;

        database = GetComponent<DatabaseHandler>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        inventoryPanel = GameObject.Find("Inventory_Panel");
        inventoryPanel.SetActive(true);

        slotPanel = inventoryPanel.transform.FindChild("Slot_Panel").gameObject;

        hand = GameObject.FindWithTag("Hand");

        UpdateWallet(1000);

        slotAmount += equipmentSlotAmount;

        for (int i = 0; i < slotAmount; i++)
        {
            items.Add(new Item());

            if (i < equipmentSlotAmount)
            {
                slots.Add(Instantiate(equipmentSlot));
            }
            else
            {
                slots.Add(Instantiate(inventorySlot));
            }
            slots[i].GetComponent<Slot>().id = i;
            slots[i].transform.SetParent(slotPanel.transform);
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
            for (int i = equipmentSlotAmount; i < items.Count; i++)
            {
                if (items[i].ID == -1)
                {
                    items[i] = itemToAdd;
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<ItemData>().item = itemToAdd;
                    itemObj.GetComponent<ItemData>().slotID = i;
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.position = slots[i].transform.position;

                    itemObj.GetComponent<Image>().sprite = FetchSpriteBySlug(itemToAdd.Type, itemToAdd.Slug);

                    itemObj.name = itemToAdd.Title;

                    slots[i].GetComponent<Slot>().containsItem = true;
                    break;
                }
            }
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
                Debug.Log("Player already holding a weapon");
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
            wepScript.melee.damage = itemToEquip.Power;
            wepScript.melee.attackSpeed = itemToEquip.MeleeAttackSpeed;
            wepScript.melee.range = itemToEquip.MeleeAttackRange;
            wepScript.melee.element = database.StringToElement(itemToEquip.MeleeElement);

            wepScript.projectile.damage = itemToEquip.Power;
            wepScript.projectile.attackSpeed = itemToEquip.RangeAttackSpeed;
            wepScript.projectile.range = itemToEquip.RangeAttackRange;
            wepScript.projectile.speed = itemToEquip.RangeBulletSpeed;
            wepScript.projectile.element = database.StringToElement(itemToEquip.RangeElement);
            player.GetWeapon(wepScript);
        }
    }

    public void StartMovingItem(ItemData itemToMove)
    {
        // Called when player clicks on an item in inv
        // empty slot the item was in
        // store lastSlotID item
        // move item with selected gameobject
        // if holding an item while slot.containsItem == true, drop held item and pick up new item

        isMovingAnItem = true;
        Debug.Log(isMovingAnItem);
        movingItem = itemToMove;
        movingItem.OnControllerPress();

    }

    public void EndMovingItem()
    {
        isMovingAnItem = false;
        slots[movingItem.item.ID].GetComponent<Slot>().containsItem = false;

        movingItem.transform.SetParent(slots[movingItem.slotID].transform);
        // Called when player clicks on a slot while holding an item
        // place held item in the selected gameobject
        // update SlotID 
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

    public Sprite FetchSpriteBySlug(string type, string slug)
    {
        Sprite spriteData = Resources.Load<Sprite>("Sprites/" + type + "/" + slug);
        return spriteData;
    }
}
