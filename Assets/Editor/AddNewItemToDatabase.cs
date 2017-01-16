using UnityEngine;
using System.Collections;
using UnityEditor;

public class AddNewItemToDatabase : ScriptableWizard
{
    [Range(0.0f, 100.0f)] public int ID = 0;
    public string itemName = "defaultItem";
    public ItemType itemType;
    public int cost;

    public int power;
    public int defence;
    public int vitality;

    public bool hasMelee; // add to db
    public Melee melee;
    public bool hasRanged; // add to db
    public Projectile ranged;

    public string description = "defaultDescription";
    public bool stackable;
    public int rarity;
    public string slug = "with _ please";

    private int currentID = 0;
    private DatabaseHandler db; 

    [MenuItem ("Tools/Inventory/Database")]
    static void AddNewItemWizard()
    {
        DisplayWizard<AddNewItemToDatabase>("Database", "Write To Database", "Save changes");
    }

    void Awake()
    {
        db = GameObject.FindWithTag("Inventory").GetComponent<DatabaseHandler>();
    }

    void OnWizardCreate()
    {
        if(Application.isPlaying)
        {
            db.ChangeItemInDatabase(ID, itemName, itemType.ToString(),cost, power, defence, vitality, description, stackable, rarity, slug, melee.attackSpeed, melee.range, melee.element, ranged.attackSpeed, ranged.range, ranged.bulletSpeed, ranged.element);
            db.WriteToDatabase();
        }
        else
        {
            Debug.Log("You have to be in play mode to add items to the database");
        }
    }

    void OnWizardOtherButton()
    {
        if (Application.isPlaying)
        {
            db.ChangeItemInDatabase(ID, itemName, itemType.ToString(), cost, power, defence, vitality, description, stackable, rarity, slug, melee.attackSpeed, melee.range, melee.element, ranged.attackSpeed, ranged.range, ranged.bulletSpeed, ranged.element);
        }
        else
        {
            Debug.Log("You have to be in play mode to add items to the database");
        }
    }

    void OnWizardUpdate()
    {
        if(ID != currentID && ID <= db.count)
        {
            currentID = ID;
            Item itemToUpdate = db.FetchItemByID(ID);

            itemName = itemToUpdate.Title;

            if (itemToUpdate.Type == ItemType.Items.ToString())
                itemType = ItemType.Items;
            else if (itemToUpdate.Type == ItemType.NPCs.ToString())
                itemType = ItemType.NPCs;
            else if (itemToUpdate.Type == ItemType.Magic.ToString())
                itemType = ItemType.Magic;
            else if (itemToUpdate.Type == ItemType.Weapon.ToString())
                itemType = ItemType.Weapon;

            cost = itemToUpdate.Value;
            power = itemToUpdate.Power;
            defence = itemToUpdate.Defence;
            vitality = itemToUpdate.Vitality;
            melee.element = db.StringToElement(itemToUpdate.MeleeElement);
            melee.damage = itemToUpdate.Power;
            melee.attackSpeed = itemToUpdate.MeleeAttackSpeed;

            ranged.damage = itemToUpdate.Power;
            ranged.attackSpeed = itemToUpdate.RangeAttackSpeed;
            ranged.range = itemToUpdate.RangeAttackRange;
            ranged.bulletSpeed = itemToUpdate.RangeBulletSpeed;
            ranged.element = db.StringToElement(itemToUpdate.RangeElement);

            description = itemToUpdate.Description;
            stackable = itemToUpdate.Stackable;
            rarity = itemToUpdate.Rarity;
            slug = itemToUpdate.Slug;
        }
    }
}

public class AddItemToInventory : ScriptableWizard
{
    public int itemID = 0;
    public int amount = 1;

    [MenuItem("Tools/Inventory/Add Item To Inventory")]
    static void AddItemToInventoryWizard()
    {
        DisplayWizard<AddItemToInventory>("Add Item To Inventory", "Add");
    }

    void OnWizardCreate()
    {
        if (Application.isPlaying)
        {
            Inventory inv;
            inv = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();

            for (int i = 0; i < amount; i++)
            {
                inv.AddItem(itemID);
            }
        }
        else
        {
            Debug.Log("You have to be in play mode to add items to inventory");
        }
    }

}

public class EmptyDatabase : ScriptableWizard
{
    public string securityCheck = "";

    [MenuItem("Tools/Inventory/Empty Database")]
    static void EmtpyDatabaseWizard()
    {
        DisplayWizard<EmptyDatabase>("Empty Database", "I am sure");
    }
    
    void OnWizardCreate()
    {
        DatabaseHandler db = GameObject.FindWithTag("Inventory").GetComponent<DatabaseHandler>();

        db.EmptyDatabase(securityCheck);
    }
}