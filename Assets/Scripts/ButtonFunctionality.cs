using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonFunctionality : MonoBehaviour {

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
        }
        else
        {
            obj.SetActive(true);
        }
    }
}
