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

        if(containsItem && type != slotType.merchant)
        {
            if (inv.isMovingAnItem)
            {
                if (inv.EndMovingItem(id))
                    inv.StartMovingItem(transform.GetChild(0).gameObject.GetComponent<ItemData>());
            }  
            else
            {
                inv.StartMovingItem(transform.GetChild(0).gameObject.GetComponent<ItemData>());
                containsItem = false;
            }
        }
        else if(inv.isMovingAnItem && containsItem == false)
        {
            inv.EndMovingItem(id);
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
}

public enum slotType
{
    regular,
    weaponEquip,
    magicEquip,
    merchant,
}
