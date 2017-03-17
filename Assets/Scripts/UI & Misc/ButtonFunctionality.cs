using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;

public class ButtonFunctionality : MonoBehaviour
{
    private Cutscene cut;
    private PlayerMovement player;
    [SerializeField]private Animator ani;
    public GameObject[] uiPanels;

    [HideInInspector]public bool canPlay;
    public bool isMainMenu;

    private EventSystem e;

    void Start()
    {
        cut = GetComponent<Cutscene>();
        e = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        if(!isMainMenu)
        {
            foreach (GameObject item in uiPanels)
            {
                item.SetActive(false);
            }
            player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        }
            
        canPlay = true;
    }

    public void Update()
    {
        //Menus
        if (Input.GetButtonDown("Y_1"))
        {
            SwitchActive(UI_Panels.Inventory);
            e.SetSelectedGameObject(uiPanels[0].transform.GetChild(0).GetChild(0).gameObject);
        }
        if (Input.GetButtonDown("B_1"))
        {
            for (int i = 0; i < uiPanels.Length; i++)
            {
                if (uiPanels[i].activeInHierarchy && uiPanels[i].name != UI_Panels.Menu.ToString())
                {
                    SwitchActive((UI_Panels)i);
                    return;
                }
            }
        }

        if (Input.GetButtonDown("Start_1"))
        {
            SwitchActive(UI_Panels.Menu);
        }
    }


    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void PlayGame()
    {
        StartCoroutine(cut.ShowCutScene());
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

    public void SwitchActive(UI_Panels panel)
    {
        GameObject obj = null;

        // Finds UI panel gameobject
        for (int i = 0; i < uiPanels.Length; i++)
            if (panel.ToString() == uiPanels[i].name)
            {
                obj = uiPanels[i];
                break;
            }

        // If UI panel was not found, return
        if (obj == null)
        {
            Debug.Log("Panel with name " + panel.ToString() + " was not found in array");
            return;
        }   

        // Depending on active state, will set object to the opposite
        if(obj.activeInHierarchy)
        {
            obj.SetActive(false);

            if (!CheckForActivePanel())
            {
                canPlay = true;
                if(!isMainMenu)Camera.main.GetComponent<CameraMovements>().SetTarget(true);
            }
        }
        else
        {
            obj.SetActive(true);
            e.SetSelectedGameObject(obj.transform.GetChild(0).gameObject);
            if (CheckForActivePanel())
                canPlay = false;
            
        }
    }

    public GameObject FetchGOByName(UI_Panels panelName)
    {
        for (int i = 0; i < uiPanels.Length; i++)
            if (panelName.ToString() == uiPanels[i].name)
                return uiPanels[i];
        return null;
    }

    public bool IsPanelActive(UI_Panels panelName)
    {
        bool isActive = false;

        for (int i = 0; i < uiPanels.Length; i++)
            if (panelName.ToString() == uiPanels[i].name)
            {
                isActive = uiPanels[i].activeInHierarchy;
                break;
            }

        return isActive;
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

public enum UI_Panels
{
    Inventory,
    Merchant_Inventory,
    Menu,
    Dialogue,
    Interaction_Menu
}
