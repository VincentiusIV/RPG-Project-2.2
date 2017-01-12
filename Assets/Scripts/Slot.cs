using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IDropHandler, IPointerClickHandler, IPointerEnterHandler, ISelectHandler, IDeselectHandler
{
    public int id;
    public bool isMerchantSlot;
    public bool isEquipSlot;

    private Inventory inv;
    private EventSystem e;

    void Start()
    {
        inv = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(isMerchantSlot == false)
        {
            ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();
            if (inv.items[id].ID == -1)
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

            if(isEquipSlot)
            {
                inv.EquipItem(droppedItem.item.ID);
            }
        }
    }

    public void ControllerDropItem()
    {

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
            ItemData itemToMove = transform.GetChild(0).gameObject.GetComponent<ItemData>();

            if(itemToMove != null)
            {
                itemToMove.OnControllerPress();
            }
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(isMerchantSlot)
            transform.FindChild("Item").gameObject.GetComponent<ItemData>().UpdateInfo();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (isMerchantSlot)
            transform.FindChild("Item").gameObject.GetComponent<ItemData>().HideInfo();
    }
}
