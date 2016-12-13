﻿using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;

public class DatabaseHandler : MonoBehaviour
{
    public string dbName;
    public int count;

    private JsonData itemData;
    private List<Item> itemList = new List<Item>();

    void Start()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/" + dbName + ".json"));
        CreateList();
    }

    private void CreateList()
    {
        count = itemData.Count;

        for (int i = 0; i < count; i++)
        {
            itemList.Add(new Item(  (int)itemData[i]["ID"],
                                    (string)itemData[i]["Title"],
                                    (string)itemData[i]["Type"],
                                    (int)itemData[i]["Value"],
                                    (int)itemData[i]["Power"],
                                    (int)itemData[i]["Defence"],
                                    (int)itemData[i]["Vitality"],
                                    (string)itemData[i]["Description"],
                                    (bool)itemData[i]["Stackable"],
                                    (int)itemData[i]["Rarity"],
                                    (string)itemData[i]["Slug"]));
        }
    }

    public void ChangeItemInDatabase(int id, string title, string type,int value, int power, int defence, int vitality, string description, bool stackable, int rarity, string slug)
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

public class Item// : IEquatable<Item>
{
    public int ID { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public int Power { get; set; }
    public int Defence { get; set; }
    public int Vitality { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public int Rarity { get; set; }
    public string Slug { get; set; }

    public Item(int id, string title, string type, int value, int power, int defence, int vitality, string description, bool stackable, int rarity, string slug)
    {
        ID = id;
        Title = title;
        Type = type;
        Value = value;
        Power = power;
        Defence = defence;
        Vitality = vitality;
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