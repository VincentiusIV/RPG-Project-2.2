using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public int id;
    public bool isMerchantSlot;
    public bool isEquipSlot;
    public bool containsItem;

    private Inventory inv;

    void Start()
    {
        inv = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
    }

    // Called when a slot is pressed
    public void OnControllerPress()
    {
        Debug.Log("On pressed this slot "+ id+" isMoving =" + inv.isMovingAnItem);
        if (inv.isMovingAnItem == false && containsItem && isMerchantSlot == false)
        {
            inv.StartMovingItem(transform.GetChild(0).gameObject.GetComponent<ItemData>());
            containsItem = false;
            return;
        }  

        if(inv.isMovingAnItem)
        {
            inv.EndMovingItem(id);

            if(containsItem)
            {
                inv.StartMovingItem(transform.GetChild(0).gameObject.GetComponent<ItemData>());
            }
            else containsItem = true;
            return;
        }

        if(isMerchantSlot)
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
        if (isMerchantSlot)
            transform.FindChild("Item").gameObject.GetComponent<ItemData>().UpdateInfo();
        else if(inv.items[id].ID != -1)
            transform.GetChild(0).gameObject.GetComponent<ItemData>().UpdateInfo();
            
    }

    // Called when slot is deselected
    public void OnDeselect(BaseEventData eventData)
    {
        if (isMerchantSlot)
            transform.FindChild("Item").gameObject.GetComponent<ItemData>().HideInfo();
        else if(inv.items[id].ID != -1)
            transform.GetChild(0).gameObject.GetComponent<ItemData>().HideInfo();
    }

    void Update()
    {
        // prevents items from getting stuck when a bug that allows two seperate items to be stacked
        if(transform.childCount > 0)
        {
            containsItem = true;
        }
    }
}
