using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateNPC : ScriptableWizard
{
    [Range(0, 100)]
    public int id;
    public string title;

    private DatabaseHandler db;

    [MenuItem("Tools/Inventory/Create NPC")]
    static void createWizard()
    {
        DisplayWizard<CreateNPC>("Create NPC", "Create");
    }

    void Awake()
    {
        db = GameObject.FindWithTag("Inventory").GetComponent<DatabaseHandler>();
    }

    void OnWizardUpdate()
    {
        Item item = db.FetchItemByID(id);
        if (item.Type == "NPCs")
            title = item.Title;
        else
            title = "not an NPC";
    }

    void OnWizardCreate()
    {

    }
}
