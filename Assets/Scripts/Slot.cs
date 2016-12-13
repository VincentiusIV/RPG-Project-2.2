﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public int id;
    public bool isMerchantSlot;

    private Inventory inv;

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
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isMerchantSlot)
        {
            ItemData itemToBuy = transform.FindChild("Item").gameObject.GetComponent<ItemData>();

            if(itemToBuy.item.Value <= inv.money)
            {
                inv.AddItem(itemToBuy.item.ID);
                inv.UpdateWallet(-itemToBuy.item.Value);
            }
        }
    }
}
