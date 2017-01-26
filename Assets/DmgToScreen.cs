using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DmgToScreen : MonoBehaviour {

    [SerializeField]private GameObject text;

	public void ShowDmg(float dmg, Vector3 worldPos)
    {
        GameObject newText = Instantiate(text, Camera.main.WorldToScreenPoint(worldPos), Quaternion.identity) as GameObject;
        newText.GetComponent<Text>().text = dmg.ToString();
        newText.transform.SetParent(transform);
        Destroy(newText, .5f);
    }
}
