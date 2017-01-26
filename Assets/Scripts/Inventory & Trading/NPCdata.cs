﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCdata : MonoBehaviour
{
// Public & Hidden Variables
    [HideInInspector]public Item thisNPC = new Item();

// Public Variables
    public int id;
    public bool isMerchant;
    public bool isLoot;
    public GameObject merchantSlot;
    public InventoryData[] invData;

// Private Variables
    private DatabaseHandler db;
    private ButtonFunctionality ui;

// Private UI objects
    private GameObject merchantPanel;
    private GameObject slotPanel;
    private GameObject notification;

    [SerializeField]private bool canTrade;

    void Start()
    {
        Initialise();
    }
    // Put into a public function so when lootboxes are spawned it still acquires the references
    public void Initialise()
    {
        notification = transform.GetChild(0).gameObject;
        notification.SetActive(false);

        db = GameObject.FindWithTag("Inventory").GetComponent<DatabaseHandler>();

        if (id != -1)
        {
            thisNPC = db.FetchItemByID(id);
            Sprite spriteData = Resources.Load<Sprite>("Sprites/" + thisNPC.Type + "/" + thisNPC.Slug);
            GetComponent<SpriteRenderer>().sprite = spriteData;
        }

        ui = GameObject.Find("UI").GetComponent<ButtonFunctionality>();

        if (isMerchant)
        {
            merchantPanel = GameObject.Find("UI").GetComponent<ButtonFunctionality>().uiPanels[1];
            Debug.Log("merchant panel on " + gameObject.name + " is " + merchantPanel);
            slotPanel = merchantPanel.transform.FindChild("Merchant_Slot_Panel").gameObject;
            canTrade = false;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") && isMerchant)
        {
            canTrade = true;
            if (notification.activeInHierarchy == false)
                notification.SetActive(true);
        }
    }

    void Update()
    {
        if(Input.GetButtonDown("Interact") && canTrade)
        {
            Camera.main.GetComponent<CameraMovements>().SetTarget(false, transform.FindChild("CamTarget").gameObject);
            CreateMerchantInventory();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        canTrade = false;

        if(notification.activeInHierarchy)
            notification.SetActive(false);
        else if(merchantPanel.activeInHierarchy)
        {
            ui.SwitchActive("Merchant_Inventory_Panel");
            ui.SwitchActive("Inventory_Panel");
        }
    }

    void CreateMerchantInventory()
    {
        for (int i = 0; i < slotPanel.transform.childCount; i++)
        {
            Destroy(slotPanel.transform.GetChild(i).gameObject);
        }
        ui.SwitchActive("Merchant_Inventory_Panel");
        ui.SwitchActive("Inventory_Panel");

        for (int i = 0; i < invData.Length; i++)
        {
            if(merchantPanel.activeInHierarchy)
            {
                Item itemToAdd = db.FetchItemByID(invData[i].id);

                GameObject itemObj = Instantiate(merchantSlot) as GameObject;
                itemObj.transform.FindChild("Item").GetComponent<ItemData>().item = itemToAdd;
                itemObj.transform.SetParent(slotPanel.transform);
                itemObj.transform.position = slotPanel.transform.position;

                Sprite spriteData = Resources.Load<Sprite>("Sprites/" + itemToAdd.Type + "/" + itemToAdd.Slug);
                itemObj.transform.FindChild("Item").GetComponent<Image>().sprite = spriteData;

                itemObj.name = itemToAdd.Title;
                itemObj.transform.FindChild("Title_Text").GetComponent<Text>().text = itemToAdd.Title;

                if (!isLoot)
                    itemObj.transform.FindChild("Cost_Text").GetComponent<Text>().text = itemToAdd.Value.ToString();
                else if (isLoot)
                    itemObj.transform.FindChild("Cost_Text").GetComponent<Text>().text = "";
            }
            else
            {
                Camera.main.GetComponent<CameraMovements>().SetTarget(true);
            }
        }

        slotPanel.transform.SetParent(merchantPanel.transform);
    }
}

[System.Serializable]
public struct InventoryData
{
    public int id;
    public int amount;
}
