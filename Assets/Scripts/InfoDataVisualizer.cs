using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InfoDataVisualizer : MonoBehaviour
{
    [SerializeField]
    private GameObject[] textFields;
    private List<string> defaultText = new List<string>();

    void Start()
    {
        for (int i = 0; i < textFields.Length; i++)
        {
            defaultText.Add(textFields[i].GetComponent<Text>().text.ToString());
        }
    }

    public void UpdateInfo(Item itemToVisual)
    {
        UpdateText(0, itemToVisual.Title.ToString());
        UpdateText(1, itemToVisual.Value.ToString());
        UpdateText(2, itemToVisual.Power.ToString());
        UpdateText(3, itemToVisual.Defence.ToString());
        UpdateText(4, itemToVisual.Vitality.ToString());
        UpdateText(5, itemToVisual.MeleeElement.ToString());
        UpdateText(6, itemToVisual.MeleeAttackSpeed.ToString());
        UpdateText(7, itemToVisual.MeleeAttackRange.ToString());
        UpdateText(8, itemToVisual.RangeElement.ToString());
        UpdateText(9, itemToVisual.RangeAttackSpeed.ToString());
        UpdateText(10, itemToVisual.RangeBulletSpeed.ToString());
        UpdateText(11, itemToVisual.RangeAttackRange.ToString());
        UpdateText(12, itemToVisual.Rarity.ToString());
        UpdateText(13, itemToVisual.Description.ToString());
    }

// Checks if the text provided holds any value, otherwise textfield will be left invisible
    void UpdateText(int index, string text)
    {
        if (text != null && text != "" && text != "0" && text != "none")
        {
            textFields[index].SetActive(true);
            textFields[index].GetComponent<Text>().text = defaultText[index] + text;
        }
        else
        {
            textFields[index].SetActive(false);
        }
    }
}
