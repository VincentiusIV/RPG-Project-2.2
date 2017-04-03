using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// Author Vincent Versnel
// visualizes damage done to enemies by showing the damage number above their heads
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
