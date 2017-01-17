using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public int id;
    public slotType type;
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
        if (containsItem && type == slotType.weaponEquip || containsItem && type == slotType.magicEquip)
            shouldUnequip = true;

        if(containsItem && type != slotType.merchant)
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

        if(type == slotType.merchant)
        {
            ItemData itemToBuy = transform.FindChild("Item").gameObject.GetComponent<ItemData>();

            if (itemToBuy.item.Value <= inv.money)
            {
                inv.AddItem(itemToBuy.item.ID);
                inv.UpdateWallet(-itemToBuy.item.Value);
            }
            return;
        }
    }


    // Called when slot is selected
    public void OnSelect(BaseEventData eventData)
    {
        if (type == slotType.merchant)
            transform.FindChild("Item").gameObject.GetComponent<ItemData>().UpdateInfo();
        else if(containsItem)
            transform.GetChild(0).gameObject.GetComponent<ItemData>().UpdateInfo();  
    }

    // Called when slot is deselected
    public void OnDeselect(BaseEventData eventData)
    {
        if (type == slotType.merchant)
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

public enum slotType
{
    regular,
    weaponEquip,
    magicEquip,
    merchant,
}
