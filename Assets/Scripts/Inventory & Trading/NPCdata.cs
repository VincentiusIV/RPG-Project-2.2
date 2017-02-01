using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCdata : MonoBehaviour
{
    // Public & Hidden Variables
    [HideInInspector]public Item thisNPC = new Item();
    

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
    [SerializeField]private GameObject dialoguePanel;
    private int currentSelection;

    [SerializeField]private bool canTrade;
    [SerializeField]private bool canTalk;

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
            canTrade = false;
        }
        if(isTalking)
        {
            dialoguePanel = GameObject.Find("Dialogue_Panel");
            dialoguePanel.SetActive(false);
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
        else if (col.CompareTag("Player") && isTalking)
            canTalk = true;
    }

    void Update()
    {
        if (Input.GetButtonDown("A_1") && canTrade && !merchantPanel.activeInHierarchy)
        {
            Camera.main.GetComponent<CameraMovements>().SetTarget(false, transform.FindChild("CamTarget").gameObject);
            CreateMerchantInventory();
        }
        else if (Input.GetButtonDown("A_1") && canTalk && !dialoguePanel.activeInHierarchy)
        {
            dialoguePanel.SetActive(true);
            NextSection();
        }
        else if (Input.GetButtonDown("A_1") && canTalk && dialoguePanel.activeInHierarchy)
            NextSection();
    }

    public void NextSection()
    {
        if (currentSelection > dialogue.Length -1)
        {
            currentSelection = 0;
            dialoguePanel.SetActive(false);
        }

        dialoguePanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = dialogue[currentSelection].npcText;
        dialoguePanel.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = dialogue[currentSelection].respondText;
        currentSelection += 1;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            canTrade = canTalk = false;

            if (notification.activeInHierarchy)
                notification.SetActive(false);
            else if (isMerchant && merchantPanel.activeInHierarchy)
            {
                ui.SwitchActive("Merchant_Inventory_Panel");
                ui.SwitchActive("Inventory_Panel");
            }
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

[System.Serializable]
public struct DialogueSection
{
    public string npcText;
    public string respondText;
}
