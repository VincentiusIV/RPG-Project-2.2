using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
// Author Vincent Versnel
// Handles the Jackson text file to store data for items, npcs, weapons and magic
public class DatabaseHandler : MonoBehaviour
{
    public string dbName;
    [HideInInspector]public int count;

    private JsonData itemData;
    private List<Item> itemList = new List<Item>();
    //Dictionary<Int, Item> itemDictionary = new Dictionary<Int, Item>();

    void Start()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/" + dbName + ".json"));
       // CreateSpots(100); 
        CreateList();
    }

    // Creates a list from the data stored in the Json file
    private void CreateList()
    {
        count = itemData.Count;

        for (int i = 0; i < count; i++)
        {
            itemList.Add(new Item((int)itemData[i]["ID"],
                                    (string)itemData[i]["Title"],
                                    (string)itemData[i]["Type"],
                                    (int)itemData[i]["Value"],
                                    (int)itemData[i]["Power"],
                                    (int)itemData[i]["Defence"],
                                    (int)itemData[i]["Vitality"],
                                    (string)itemData[i]["Description"],
                                    (bool)itemData[i]["Stackable"],
                                    (int)itemData[i]["Rarity"],
                                    (string)itemData[i]["Slug"],
                                    (int)itemData[i]["MeleeAttackSpeed"],
                                    (int)itemData[i]["MeleeAttackRange"],
                                    StringToElement((string)itemData[i]["MeleeElement"]),
                                    (int)itemData[i]["RangeAttackSpeed"],
                                    (int)itemData[i]["RangeAttackRange"],
                                    (double)itemData[i]["RangeBulletSpeed"],
                                    StringToElement((string)itemData[i]["RangeElement"]),
                                    StringToProjType((string)itemData[i]["RangeProjType"]),
                                    (bool)itemData[i]["LifeSteal"],
                                    (bool)itemData[i]["KnockBack"],
                                    (double)itemData[i]["SlowAmount"]));
        }
    }

    // Changes item information in the json file, called from custom editor wizard
    public void ChangeItemInDatabase(int id, string title, string type,int value, int power, int defence, int vitality, string description, bool stackable, int rarity, string slug, int mAttSp, int mAttRan, ElementType mElem, int rAttSp, int rAttRan, double rBullSp, ElementType rElem, ProjectileType rProjType, bool lifeSteal, bool knockback, double slowAmount)
    {
        itemList[id].Title = title;
        itemList[id].Type = type;
        itemList[id].Value = value;
        itemList[id].Power = power;
        itemList[id].Defence = defence;
        itemList[id].Vitality = vitality;
        itemList[id].Description = description;
        itemList[id].Stackable = stackable;
        itemList[id].Rarity = rarity;
        itemList[id].Slug = slug;
        // melee
        itemList[id].MeleeAttackSpeed = mAttSp;
        itemList[id].MeleeAttackRange = mAttRan;
        itemList[id].MeleeElement = mElem.ToString();
        // range
        itemList[id].RangeAttackSpeed = rAttSp;
        itemList[id].RangeAttackRange = rAttRan;
        itemList[id].RangeBulletSpeed = rBullSp;
        itemList[id].RangeElement = rElem.ToString();
        itemList[id].RangeProjType = rProjType.ToString();
        // general
        itemList[id].LifeSteal = lifeSteal;
        itemList[id].KnockBack = knockback;
        itemList[id].SlowAmount = slowAmount;
    }

    // creates empty spots in the json file
    void CreateSpots(int amountOfSpots)
    {
        for (int i = 0; i < amountOfSpots; i++)
        {
            itemList.Add(new Item(itemList.Count));
        }
        WriteToDatabase();
    }

    // writes to the json file
    public void WriteToDatabase()
    {
        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);
        writer.PrettyPrint = true;
        writer.IndentValue = 1;

        JsonMapper.ToJson(itemList, writer);
        Debug.Log(sb.ToString());
        File.WriteAllText(Application.dataPath + "/StreamingAssets/" + dbName + ".json", sb.ToString());
    }

    // converts strings to element type
    public ElementType StringToElement(string str)
    {
        switch(str)
        {
            case "fire":
                return ElementType.fire;
            case "earth":
                return ElementType.earth;
            case "water":
                return ElementType.water;
            case "oil":
                return ElementType.oil;
            case "electricity":
                return ElementType.electricity;
            case "aether":
                return ElementType.aether;
            case "ice":
                return ElementType.ice;
        }
        return ElementType.none;
    }

    // converts strings to projectile types
    public ProjectileType StringToProjType(string str)
    {
        switch (str)
        {
            case "bullet":
                return ProjectileType.bullet;
            case "beam":
                return ProjectileType.beam;
            case "flames":
                return ProjectileType.flames;
        }
        return ProjectileType.bullet;
    }

    // returns an item by giving the id
    public Item FetchItemByID(int id)
    {
        for (int i = 0; i < itemList.Count; i++)
            if (itemList[i].ID == id)
                return itemList[i]; 
        return null;
    }

    // empties the database
    public void EmptyDatabase(string safetyString)
    {
        if(safetyString == "I am sure to empty the database")
            File.WriteAllText(Application.dataPath + "/StreamingAssets/" + dbName + ".json", "");
        else
            Debug.Log("Safety string was incorrect... database was not emptied");
    }
}
// Item class, used for items, npcs, magic, weapons,
public class Item
{
    public int ID { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public int Power { get; set; }
    public int Defence { get; set; }
    public int Vitality { get; set; }
    public int MeleeAttackSpeed { get; set; }
    public int MeleeAttackRange { get; set; }
    public string MeleeElement { get; set; }
    public int RangeAttackSpeed { get; set; }
    public int RangeAttackRange { get; set; }
    public double RangeBulletSpeed { get; set; }
    public string RangeElement { get; set; }
    public string RangeProjType { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public int Rarity { get; set; }
    public string Slug { get; set; }
    public bool LifeSteal { get; set; }
    public bool KnockBack { get; set; }
    public double SlowAmount { get; set; }
    
    // bool lifesteal, bool chain ,int knockbackStrength, int slowAmount, bool mindcontrol, enum projectileType, int resistanceBoost
    public Item(int id, string title, string type, int value, int power, int defence, int vitality, string description, bool stackable, int rarity, string slug, int mAttSp, int mAttRan, ElementType mElem, int rAttSp, int rAttRan, double rBullSp, ElementType rElem, ProjectileType rProjType, bool lifeSteal, bool knockback, double slowAmount)
    {
        ID = id;
        Title = title;
        Type = type;
        Value = value;

        Power = power;
        Defence = defence;
        Vitality = vitality;

        MeleeAttackSpeed = mAttSp;
        MeleeAttackRange = mAttRan;
        MeleeElement = mElem.ToString();

        RangeAttackSpeed = rAttSp;
        RangeAttackRange = rAttRan;
        RangeBulletSpeed = rBullSp;
        RangeElement = rElem.ToString();
        RangeProjType = rProjType.ToString();

        LifeSteal = lifeSteal;
        KnockBack = knockback;
        SlowAmount = slowAmount;

        Description = description;
        Stackable = stackable;
        Rarity = rarity;
        Slug = slug;
        
    }

    public Item(int id)
    {
        ID = id;
        Title = "default";
        Type = "Items";
        Value = 0;
        Power = 0;
        Defence = 0;
        Vitality = 0;
        Description = "default";
        Stackable = false;
        Rarity = 0;
        Slug = "default_slug";
        MeleeElement = ElementType.none.ToString();
        RangeElement = ElementType.none.ToString();
        RangeProjType = ProjectileType.bullet.ToString();

    }

    public Item()
    {
        ID = -1;
    }

    public bool Equals(Item other)
    {
        if(other == null)
        {
            return false;
        }
        else
        {
            return (Title.Equals(other.Title));
        }
    }
} 

[Serializable]
public enum WeaponType
{
    Melee,
    Ranged,
}

[System.Serializable]
public enum ItemType
{
    Items, Magic, Weapon, NPCs,
}
