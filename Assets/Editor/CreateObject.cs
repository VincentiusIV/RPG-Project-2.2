using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateObject : ScriptableWizard
{
    [MenuItem("Tools/Create Object")]
    static void CreateObjectWizard()
    {
        DisplayWizard<AddNewItemToDatabase>("Add New Item to Database", "Create New Item");
    }
}
