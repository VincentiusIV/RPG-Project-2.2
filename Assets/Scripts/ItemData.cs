using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public int amount = 1;
    public int slotID;

    [SerializeField]private GameObject[] textFields;

    private Inventory inv;
    private CanvasGroup cg;

    private GameObject InfoPanel;
    private GameObject TextPanel;
    private Vector3 offset = new Vector3(120f,  -160f, 0f);

    private EventSystem e;
    private bool canDrag;

    void Start()
    {
        e = GameObject.Find("EventSystem").GetComponent<EventSystem>();
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
    }

    public void OnControllerPress()
    {
        if (item != null && canDrag == false)
        {
            canDrag = true;
            transform.SetParent(transform.parent.parent);
        }
        else if(canDrag)
        {
            // drop item
            canDrag = false;
        }
        inv.EquipItem(item.ID);
    }

    void Update()
    {
        // dragging with controller
        if(canDrag)
        {
            Debug.Log("Dragging with controller...");

            if (e.currentSelectedGameObject.CompareTag("Slot"))
                transform.position = e.currentSelectedGameObject.transform.position;
            else
                Debug.LogError("Cannot move item there because it is not a slot");
            
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
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        if(InfoPanel.activeInHierarchy == false)
            InfoPanel.SetActive(true);

        InfoPanel.transform.position = transform.position + offset;

        TextPanel.GetComponent<InfoDataVisualizer>().UpdateInfo(item);
    }

    public void HideInfo()
    {
        InfoPanel.SetActive(false);
    }

    void UpdateText(GameObject go, string defaultText, string text, bool forceActive)
    {
        if (text != null || text == "" || forceActive)
        {
            go.SetActive(true);
            go.GetComponent<Text>().text = defaultText + text;
        }
        else
        {
            go.SetActive(false);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (InfoPanel.activeInHierarchy == true)
            InfoPanel.SetActive(false);
    }
}
