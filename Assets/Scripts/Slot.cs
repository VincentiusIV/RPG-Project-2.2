using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IDropHandler, IPointerClickHandler, IPointerEnterHandler, ISelectHandler, IDeselectHandler
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

    public void OnDrop(PointerEventData eventData)
    {
        ControllerDropItem(eventData.pointerDrag.GetComponent<ItemData>());
    }

    public void ControllerDropItem(ItemData droppedItem)
    {
        if (isMerchantSlot == false)
        {
            /*if (inv.items[id].ID == -1)
            {
                inv.items[droppedItem.slotID] = new Item();
                inv.items[id] = droppedItem.item;
                droppedItem.slotID = id;
            }
            else
            {
                Transform item = this.transform.GetChild(0);
                item.GetComponent<ItemData>().slotID = droppedItem.slotID;
                item.transform.SetParent(inv.slots[droppedItem.slotID].transform);
                item.transform.position = inv.slots[droppedItem.slotID].transform.position;

                droppedItem.slotID = id;
                droppedItem.transform.SetParent(this.transform);
                droppedItem.transform.position = this.transform.position;

                inv.items[id] = item.GetComponent<ItemData>().item;
                inv.items[id] = droppedItem.item;
            }
            */
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isMerchantSlot)
        {
            Debug.Log("Pointer entered merchant slot");
            transform.FindChild("Item").gameObject.GetComponent<ItemData>().UpdateInfo();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnControllerPress();
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
/*
        else
        {
            if(inv.movingItem != null)
            {
                Debug.Log("Dropped moving item");
                ControllerDropItem(inv.movingItem);
                inv.movingItem = null;
            }
            else
            {
                Debug.Log("Picked up an item");
                inv.movingItem = transform.GetChild(0).gameObject.GetComponent<ItemData>();
                inv.items[inv.movingItem.slotID] = new Item();

                if (inv.movingItem != null)
                {
                    inv.movingItem.OnControllerPress();
                }
            }
        }*/
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (isMerchantSlot)
            transform.FindChild("Item").gameObject.GetComponent<ItemData>().UpdateInfo();
        else if(containsItem)
            transform.GetChild(0).gameObject.GetComponent<ItemData>().UpdateInfo();
            
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (isMerchantSlot)
            transform.FindChild("Item").gameObject.GetComponent<ItemData>().HideInfo();
        else if(containsItem)
            transform.GetChild(0).gameObject.GetComponent<ItemData>().HideInfo();
    }
}
