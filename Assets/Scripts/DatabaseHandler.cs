using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;

public class DatabaseHandler : MonoBehaviour
{
    public string dbName;
    [HideInInspector]public int count;

    private JsonData itemData;
    private List<Item> itemList = new List<Item>();

    void Start()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/" + dbName + ".json"));
        //CreateSpots(100); 
        CreateList();
    }

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
                                    StringToElement((string)itemData[i]["RangeElement"])));
        }
    }

    public void ChangeItemInDatabase(int id, string title, string type,int value, int power, int defence, int vitality, string description, bool stackable, int rarity, string slug, int mAttSp, int mAttRan, Elements mElem, int rAttSp, int rAttRan, double rBullSp, Elements rElem)
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
        itemList[id].MeleeAttackSpeed = mAttSp;
        itemList[id].MeleeAttackRange = mAttRan;
        itemList[id].MeleeElement = mElem.ToString();

        itemList[id].RangeAttackSpeed = rAttSp;
        itemList[id].RangeAttackRange = rAttRan;
        itemList[id].RangeBulletSpeed = rBullSp;
        itemList[id].RangeElement = rElem.ToString();
    }

    void CreateSpots(int amountOfSpots)
    {
        for (int i = 0; i < amountOfSpots; i++)
        {
            itemList.Add(new Item(itemList.Count));
        }
        WriteToDatabase();
    }

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

    public Elements StringToElement(string str)
    {
        if (str == "fire")
        {
            Debug.Log("given string returns as fire");
            return Elements.fire;
        }
            
        if (str == "earth")
            return Elements.earth;
        if (str == "water")
            return Elements.water;
        if (str == "oil")
            return Elements.oil;
        if (str == "ice")
            return Elements.ice;
        if (str == "aether")
            return Elements.aether;
        else return Elements.none;
    }
    public Item FetchItemByID(int id)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].ID == id)
            {
                return itemList[i];
            }  
        }
        return null;
    }

    public void EmptyDatabase(string safetyString)
    {
        if(safetyString == "I am sure to empty the database")
        {
            File.WriteAllText(Application.dataPath + "/StreamingAssets/" + dbName + ".json", "");
        }
        else
        {
            Debug.Log("Safety string was incorrect... database was not emptied");
        }
    }
}

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
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public int Rarity { get; set; }
    public string Slug { get; set; }
    

    public Item(int id, string title, string type, int value, int power, int defence, int vitality, string description, bool stackable, int rarity, string slug, int mAttSp, int mAttRan, Elements mElem, int rAttSp, int rAttRan, double rBullSp, Elements rElem)
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
        MeleeElement = Elements.none.ToString();
        RangeElement = Elements.none.ToString();

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
/*
[Serializable]
public enum MagicType
{
    Fire,
    Earth,
    Water,
    Ice,
}

[Serializable]
public struct Magic
{
    MagicType thisType;
    MagicType[] goodAgainst;
}*/