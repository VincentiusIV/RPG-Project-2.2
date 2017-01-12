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

                inv.items[droppedItem.slotID] = item.GetComponent<ItemData>().item;
                inv.items[id] = droppedItem.item;
            }
            */
            // checks if this slot is empty

            if (isEquipSlot)
            {
                inv.EquipItem(droppedItem.item.ID);
            }
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

    public void OnControllerPress()
    {
        if (containsItem && isMerchantSlot == false)
        {
            inv.StartMovingItem(transform.GetChild(0).gameObject.GetComponent<ItemData>());
            inv.isMovingAnItem = true;
        }  

        if(inv.isMovingAnItem)
        {
            inv.EndMovingItem();
        }

        if(isMerchantSlot)
        {
            ItemData itemToBuy = transform.FindChild("Item").gameObject.GetComponent<ItemData>();

            if (itemToBuy.item.Value <= inv.money)
            {
                inv.AddItem(itemToBuy.item.ID);
                inv.UpdateWallet(-itemToBuy.item.Value);
            }
        }
        /*
        if (isMerchantSlot)
        {
            ItemData itemToBuy = transform.FindChild("Item").gameObject.GetComponent<ItemData>();

            if (itemToBuy.item.Value <= inv.money)
            {
                inv.AddItem(itemToBuy.item.ID);
                inv.UpdateWallet(-itemToBuy.item.Value);
            }
        }
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
                inv.movingItem.lastSlotID = inv.movingItem.slotID;
                inv.movingItem.slotID = -1;
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

        Debug.Log("On selecting this slot isMoving =" +inv.isMovingAnItem);
        if (inv.isMovingAnItem)
        {
            inv.movingItem.slotID = id;
            Debug.Log("While holding an item, a new slot was selected with id:" + id);
        }
            
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (isMerchantSlot)
            transform.FindChild("Item").gameObject.GetComponent<ItemData>().HideInfo();
        else
            transform.GetChild(0).gameObject.GetComponent<ItemData>().HideInfo();
    }
}
