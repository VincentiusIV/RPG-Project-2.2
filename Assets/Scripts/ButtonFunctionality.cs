using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonFunctionality : MonoBehaviour
{
    PlayerMovement player;

    void Start()
    {
        for (int i = 1; i < 4; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        player.canPlay = true;
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

    }

    public void SwitchActive(string objName)
    {
        GameObject obj = transform.FindChild(objName).gameObject;

        if(obj.activeInHierarchy)
        {
            obj.SetActive(false);
            player.canPlay = true;
        }
        else
        {
            obj.SetActive(true);
            player.canPlay = false;
        }
    }

    public void Update()
    {
        //Menus
        if (Input.GetKeyDown(KeyCode.I))
        {
            SwitchActive("Inventory_Panel");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchActive("Menu_Panel");
        }
    }
}
