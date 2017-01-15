using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonFunctionality : MonoBehaviour
{
    PlayerMovement player;

    public GameObject[] uiPanels;

    void Start()
    {
        for (int i = 1; i < 4; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        player.canPlay = true;
    }

    public void Update()
    {
        //Menus
        if (Input.GetButtonDown("Inventory"))
        {
            SwitchActive("Inventory_Panel");
        }
        if (Input.GetButtonDown("Cancel"))
        {
            for (int i = 0; i < uiPanels.Length; i++)
            {
                if (uiPanels[i].activeInHierarchy && uiPanels[i].name != "Menu_Panel")
                {
                    SwitchActive(uiPanels[i].name);
                    return;
                }
            }
        }

        if (Input.GetButtonDown("Escape"))
        {
            SwitchActive("Menu_Panel");
        }
    }


    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SaveGame()
    {
        // TO-DO Save game functionality
    }

    public void LoadGame()
    {
        // TO-DO Load game functionality
    }

    public void SwitchActive(string objName)
    {
        GameObject obj = null;

        // Finds UI panel gameobject
        for (int i = 0; i < uiPanels.Length; i++)
        {
            if (objName == uiPanels[i].name)
            {
                obj = uiPanels[i];
                break;
            }
        }

        // If UI panel was not found, return
        if (obj == null)
        {
            Debug.Log("Panel with name " + objName + " was not found in array");
            return;
        }   

        // Depending on active state, will set object to the opposite
        if(obj.activeInHierarchy)
        {
            obj.SetActive(false);
            
            if (CheckForActivePanel() == false)
                player.canPlay = true;
        }
        else
        {
            obj.SetActive(true);

            if (CheckForActivePanel())
                player.canPlay = false;
        }
    }

    

    bool CheckForActivePanel()
    {
        for (int i = 0; i < uiPanels.Length; i++)
        {
            if (uiPanels[i].activeInHierarchy)
                return true;
        }
        return false;
    }
}
