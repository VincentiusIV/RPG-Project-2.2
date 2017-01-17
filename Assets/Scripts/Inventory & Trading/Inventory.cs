using UnityEngine;
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
    [SerializeField]private WeaponScript emptyHand;

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

        if(itemToAdd == null)
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

    public void EquipItem(int id, int slotID)
    {
        Item itemToEquip = database.FetchItemByID(id);
        Debug.Log("equipping... "+itemToEquip.Title + " " + itemToEquip.Slug);

        if (itemToEquip == null || itemToEquip.Type == "Items")
        {
            Debug.Log("Item with ID: " + id + " does not exist");
            return;
        }

        if (itemToEquip.Stackable == false)
        {
            int equipSlotID = slotID - slotAmount;
            GameObject weapon = hand.transform.GetChild(equipSlotID).gameObject;
            weapon.GetComponent<SpriteRenderer>().sprite = FetchSpriteBySlug(itemToEquip.Type, itemToEquip.Slug);
            weapon.tag = itemToEquip.Type;
            
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
            wepScript.CheckWeaponType();
            if (weapon.tag == "Weapon")
                player.GetWeapon();
            else if (weapon.tag == "Magic")
                player.GetMagic(equipSlotID, FetchSpriteBySlug(itemToEquip.Type, itemToEquip.Slug));
        }
    }

    public void StartMovingItem(ItemData itemToMove, bool shouldUnequip)
    {
        // Setting state & position for moving
        isMovingAnItem = true;

        movingItem = itemToMove;
        items[movingItem.slotID] = new Item();

        // Unequips old item if there was one equipped
        if (shouldUnequip && itemToMove.item.Type == "Weapon")
            EquipItem(99, movingItem.slotID);
        else if (shouldUnequip && itemToMove.item.Type == "Magic")
            EquipItem(98, movingItem.slotID);

        movingItem.transform.SetParent(movingItem.transform.parent.parent);
        movingItem.OnControllerDrag();
    }

    // Returns true if the item was dropped
    public bool EndMovingItem(int new_slotID)
    {
        // Equips item on character if slot is equip slot
        if (slots[new_slotID].GetComponent<Slot>().type == slotType.weaponEquip && movingItem.item.Type == ItemType.Weapon.ToString())
        {
            Debug.Log("Equipping weapon...");
            EquipItem(movingItem.item.ID, new_slotID);
            MoveItem(new_slotID);
            return true;
        }
        if (slots[new_slotID].GetComponent<Slot>().type == slotType.magicEquip && movingItem.item.Type == ItemType.Magic.ToString())
        {
            Debug.Log("Equipping magic...");
            EquipItem(movingItem.item.ID, new_slotID);
            MoveItem(new_slotID);
            return true;
        }
        if(slots[new_slotID].GetComponent<Slot>().type == slotType.regular)
        {
            MoveItem(new_slotID);
            return true;
        }
        return false; 
    }

    void MoveItem(int new_slotID)
    {
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
        // Play small animation so the player sees how much 
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
