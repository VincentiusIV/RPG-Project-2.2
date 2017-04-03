using UnityEngine;
using UnityEngine.EventSystems;

// Author: Vincent Versnel
// Holds functionality for a slot so it can do things upon select, deselect and press
public class Slot : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public int id;
    public SlotType type;
    public bool containsItem;

    private Inventory inv;

    void Start()
    {
        inv = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        containsItem = false;
    }

    // Called when a slot is pressed
    public void OnControllerPress()
    {
        //Debug.Log("slot " + id + " contains item = " + containsItem);
        bool shouldUnequip = false;
        if (containsItem && type == SlotType.weaponEquip || containsItem && type == SlotType.magicEquip)
            shouldUnequip = true;

        // If contains an item
        if (containsItem && type != SlotType.merchant)
            // If inventory is moving an item, it should be dropped
            if (inv.isMovingAnItem)
            {
                // If it was dropped, the inventory can pickup the item that was already there
                if (inv.EndMovingItem(id))
                    inv.StartMovingItem(transform.GetChild(0).gameObject.GetComponent<ItemData>(), shouldUnequip);
            }  
            // If the inventory was not moving an item, it can just pick the item up
            else
            {
                inv.StartMovingItem(transform.GetChild(0).gameObject.GetComponent<ItemData>(), shouldUnequip);
                containsItem = false;
            }
        // If the slot does not contain an item it can just be dropped
        else if (inv.isMovingAnItem && !containsItem)
            if (inv.EndMovingItem(id))
                containsItem = true;

        // Slot functionality for merchant and loot slots
        // Loot slots ignore the value because it should cost anything to pick up loot
        if (type == SlotType.merchant || type == SlotType.loot)
        {
            ItemData itemToBuy = transform.FindChild("Item").gameObject.GetComponent<ItemData>();

            if (itemToBuy.item.Value <= inv.money || type == SlotType.loot)
            {
                inv.AddItem(itemToBuy.item.ID);
                if (type == SlotType.merchant)
                    inv.UpdateWallet(-itemToBuy.item.Value);
                else if (type == SlotType.loot)
                    Destroy(gameObject);
            }
            return;
        }

    }

    // Called when slot is selected
    public void OnSelect(BaseEventData eventData)
    {
        if (type == SlotType.merchant)
            transform.FindChild("Item").gameObject.GetComponent<ItemData>().UpdateInfo();
        else if (containsItem)
            transform.GetChild(0).gameObject.GetComponent<ItemData>().UpdateInfo();
    }

    // Called when slot is deselected
    public void OnDeselect(BaseEventData eventData)
    {
        if (type == SlotType.merchant)
            transform.FindChild("Item").gameObject.GetComponent<ItemData>().HideInfo();
        else if (containsItem)
            transform.GetChild(0).gameObject.GetComponent<ItemData>().HideInfo();
    }

    // Prevents items from getting stuck forever
    void Update()
    {
        if (transform.childCount > 0)
            containsItem = true;
        else if (transform.childCount == 0)
            containsItem = false;
    }
}

public enum SlotType
{
    regular,
    weaponEquip,
    magicEquip,
    merchant,
    loot,
}
