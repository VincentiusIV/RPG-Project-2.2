using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Item item;
    public int amount = 1;
    public int slotID;

    private Inventory inv;
    private CanvasGroup cg;

    private GameObject InfoPanel;
    private GameObject TextPanel;
    private Vector3 offset = new Vector3(40f, -80f, 0f);

    void Start()
    {
        if(item == null)
            item = new Item();
        InfoPanel = transform.parent.parent.parent.FindChild("Info_Panel").gameObject;
        TextPanel = transform.parent.parent.parent.FindChild("Info_Panel").FindChild("Text_Panel").gameObject;
        InfoPanel.SetActive(false);
        inv = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        cg = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            if (InfoPanel.activeInHierarchy == true)
                InfoPanel.SetActive(false);

            transform.SetParent(transform.parent.parent);
            transform.position = eventData.position;
            cg.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            transform.position = eventData.position;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inv.EquipItem(item.ID);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(inv.slots[slotID].transform);
        transform.position = inv.slots[slotID].transform.position;

        cg.blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateInfo(eventData);
    }

    public void UpdateInfo(PointerEventData eventData)
    {
        if(InfoPanel.activeInHierarchy == false)
            InfoPanel.SetActive(true);

        InfoPanel.transform.position = transform.position + offset;
        TextPanel.transform.FindChild("Name").GetComponent<Text>().text = "Name: " + item.Title;
        TextPanel.transform.FindChild("Value").GetComponent<Text>().text = "Value: " + item.Value;
        TextPanel.transform.FindChild("Power").GetComponent<Text>().text = "Power: " + item.Power;
        TextPanel.transform.FindChild("Defence").GetComponent<Text>().text = "Defence: " + item.Defence;
        TextPanel.transform.FindChild("Vitality").GetComponent<Text>().text = "Vitality: " + item.Vitality;
        TextPanel.transform.FindChild("Description").GetComponent<Text>().text = "Description: " + item.Description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (InfoPanel.activeInHierarchy == true)
            InfoPanel.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
