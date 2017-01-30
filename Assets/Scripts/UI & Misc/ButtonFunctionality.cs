﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;
public class ButtonFunctionality : MonoBehaviour
{
    private PlayerMovement player;
    [SerializeField]private Animator ani;
    public GameObject[] uiPanels;

    public bool canPlay;

    private EventSystem e;

    void Start()
    {
        e = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        for (int i = 1; i < 4; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        canPlay = true;
    }

    public void Update()
    {
        //Menus
        if (Input.GetButtonDown("Inventory"))
        {
            SwitchActive("Inventory_Panel");
            e.SetSelectedGameObject(uiPanels[0].transform.GetChild(0).GetChild(0).gameObject);
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

            if (!CheckForActivePanel())
            {
                canPlay = true;
                Camera.main.GetComponent<CameraMovements>().SetTarget(true);
            }
        }
        else
        {
            obj.SetActive(true);

            if (CheckForActivePanel())
                canPlay = false;
            
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

    public IEnumerator FadeIn()
    {
        canPlay = false;
        ani.SetBool("Fade", true);
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator FadeOut()
    {
        ani.SetBool("Fade", false);
        yield return new WaitForSeconds(1f);
        canPlay = true;
    }
}
