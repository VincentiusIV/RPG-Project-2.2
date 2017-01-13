using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class ItemData : MonoBehaviour
{
    public Item item;
    public int amount = 1;
    //public int lastSlotID;
    public int slotID;

    private Inventory inv;
    private CanvasGroup cg;

    private GameObject InfoPanel;
    private GameObject TextPanel;

    private EventSystem e;
    private bool canDrag;
    private Vector3 dragOffset = new Vector3(10f, 10f, 0f);

    void Start()
    {
        e = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        InfoPanel = transform.parent.parent.parent.FindChild("Info_Panel").gameObject;
        TextPanel = transform.parent.parent.parent.FindChild("Info_Panel").FindChild("Text_Panel").gameObject;
        InfoPanel.SetActive(false);
        inv = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        cg = GetComponent<CanvasGroup>();
        
    }

// Controller interaction
    public void OnControllerDrag()
    {
        if (item != null && canDrag == false)
        {
            //Debug.Log("Controller started dragging: "+item.Title);
            canDrag = true;
        }
    }

    public void OnControllerDrop()
    {
        if(item != null && canDrag == true)
        {
            //Debug.Log("Dropping item:" + item.Title);
            canDrag = false;
            transform.SetParent(inv.slots[slotID].transform);
            transform.position = inv.slots[slotID].transform.position;
        }
    }

    void Update()
    {
        // dragging with controller
        if (canDrag)
        {
            // drags item if selected game object is a slot
            if (e.currentSelectedGameObject.CompareTag("Slot"))
                transform.position = e.currentSelectedGameObject.transform.position + dragOffset;
            else
            {
                Debug.LogError("Cannot move item there because it is not a slot");
                Debug.Log("slotID was:" + slotID);
                inv.EndMovingItem(inv.SearchForEmptySlot());
                inv.slots[slotID].GetComponent<Slot>().containsItem = true;
                canDrag = false;
            }
        }
    }

// Public functions used by both mouse and controller
    public void UpdateInfo()
    {
        if(InfoPanel.activeInHierarchy == false)
            InfoPanel.SetActive(true);

        TextPanel.GetComponent<InfoDataVisualizer>().UpdateInfo(item);
    }

    public void HideInfo()
    {
        InfoPanel.SetActive(false);
    }

    
}
