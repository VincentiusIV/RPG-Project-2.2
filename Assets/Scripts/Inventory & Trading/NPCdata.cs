using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

// Author: Vincent Versnel
// Holds functionality for an NPC
// (also used for lootboxes)
public class NPCdata : MonoBehaviour
{
    // Public & Hidden Variables
    [HideInInspector]
    public Item thisNPC = new Item();


    // Public Variables
    public int id;
    public bool isTalking;
    public bool isMerchant;
    public bool isLoot;
    public GameObject merchantSlot;
    public InventoryData[] invData;
    public DialogueSection[] dialogue;

    // Private Variables
    private DatabaseHandler db;
    private ButtonFunctionality ui;

    // Private UI objects
    private GameObject merchantPanel;
    private GameObject slotPanel;
    private GameObject notification;
    [SerializeField]
    private GameObject dialoguePanel;
    private int currentSelection;

    [SerializeField]
    private bool canTalk;

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
            //Sprite spriteData = Resources.Load<Sprite>("Sprites/" + thisNPC.Type + "/" + thisNPC.Slug);
            //GetComponent<SpriteRenderer>().sprite = spriteData;
        }

        ui = GameObject.Find("UI").GetComponent<ButtonFunctionality>();

        if (isMerchant)
        {
            merchantPanel = GameObject.Find("UI").GetComponent<ButtonFunctionality>().uiPanels[1];
            slotPanel = merchantPanel.transform.FindChild("Merchant_Slot_Panel").gameObject;
        }
        if (isTalking)
        {
            dialoguePanel = GameObject.Find("Dialogue_Panel");
        }
    }

    void Update()
    {
        if (canTalk && Input.GetButtonDown("A_1") && !ui.IsPanelActive(UI_Panels.Interaction_Menu))
            ShowInteractionMenu();

        if (ui.IsPanelActive(UI_Panels.Dialogue) && Input.GetButtonDown("A_1"))
            NextSection();

    }

    private void ShowInteractionMenu()
    {
        // focus camera
        Camera.main.GetComponent<CameraMovements>().SetTarget(false, gameObject);
        // show menu
        ui.SwitchActive(UI_Panels.Interaction_Menu);
        Debug.Log(ui.FetchGOByName(UI_Panels.Interaction_Menu).name);
        ui.FetchGOByName(UI_Panels.Interaction_Menu).GetComponent<NPC_Interaction>().SetCurrentNPC(this);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            canTalk = true;

            if (!notification.activeInHierarchy)
                notification.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            canTalk = false;

            if (notification.activeInHierarchy)
                notification.SetActive(false);
        }
    }

    public void CreateMerchantInventory()
    {
        ui.SwitchActive(UI_Panels.Interaction_Menu);

        for (int i = 0; i < slotPanel.transform.childCount; i++)
        {
            Destroy(slotPanel.transform.GetChild(i).gameObject);
        }
        ui.SwitchActive(UI_Panels.Merchant_Inventory);
        ui.SwitchActive(UI_Panels.Inventory);

        for (int i = 0; i < invData.Length; i++)
        {
            if (merchantPanel.activeInHierarchy)
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

    public void Engage_Dialogue()
    {
        ui.SwitchActive(UI_Panels.Dialogue);
        NextSection();
    }

    public void NextSection()
    {
        if (currentSelection > dialogue.Length - 1)
        {
            currentSelection = 0;
            ui.FetchGOByName(UI_Panels.Dialogue).SetActive(false);
        }

        ui.FetchGOByName(UI_Panels.Dialogue).transform.GetChild(1).GetChild(0).GetComponent<Text>().text = dialogue[currentSelection].npcText;
        ui.FetchGOByName(UI_Panels.Dialogue).transform.GetChild(2).GetChild(0).GetComponent<Text>().text = dialogue[currentSelection].respondText;
        currentSelection += 1;
    }
}

[System.Serializable]
public struct InventoryData
{
    public int id;
    public int amount;
}

[System.Serializable]
public struct DialogueSection
{
    public string npcText;
    public string respondText;
}
