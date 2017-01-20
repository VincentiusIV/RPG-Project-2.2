using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public int id;
    public SlotType type;
    [HideInInspector]public bool containsItem;

    private Inventory inv;

    void Start()
    {
        inv = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        containsItem = false;
    }

    // Called when a slot is pressed
    public void OnControllerPress()
    {
        Debug.Log("slot " + id + " contains item = " + containsItem);
        bool shouldUnequip = false;
        if (containsItem && type == SlotType.weaponEquip || containsItem && type == SlotType.magicEquip)
            shouldUnequip = true;

        if (containsItem && type != SlotType.merchant)
        {
            if (inv.isMovingAnItem)
            {
                if (inv.EndMovingItem(id))
                    inv.StartMovingItem(transform.GetChild(0).gameObject.GetComponent<ItemData>(), shouldUnequip);
            }  
            else
            {
                inv.StartMovingItem(transform.GetChild(0).gameObject.GetComponent<ItemData>(), shouldUnequip);
                containsItem = false;
            }
        }
        else if(inv.isMovingAnItem && containsItem == false)
        {
            if(inv.EndMovingItem(id))
                containsItem = true;
        }

        if(type == SlotType.merchant || type == SlotType.loot)
        {
            ItemData itemToBuy = transform.FindChild("Item").gameObject.GetComponent<ItemData>();

            if (itemToBuy.item.Value <= inv.money || type == SlotType.loot)
            {
                inv.AddItem(itemToBuy.item.ID);
                if(type == SlotType.merchant)
                    inv.UpdateWallet(-itemToBuy.item.Value);
            }
            return;
        }

    }


    // Called when slot is selected
    public void OnSelect(BaseEventData eventData)
    {
        if (type == SlotType.merchant)
            transform.FindChild("Item").gameObject.GetComponent<ItemData>().UpdateInfo();
        else if(containsItem)
            transform.GetChild(0).gameObject.GetComponent<ItemData>().UpdateInfo();  
    }

    // Called when slot is deselected
    public void OnDeselect(BaseEventData eventData)
    {
        if (type == SlotType.merchant)
            transform.FindChild("Item").gameObject.GetComponent<ItemData>().HideInfo();
        else if(containsItem)
            transform.GetChild(0).gameObject.GetComponent<ItemData>().HideInfo();
    }

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
