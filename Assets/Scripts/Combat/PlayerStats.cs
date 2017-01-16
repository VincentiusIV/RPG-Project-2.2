using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class PlayerStats
{
    public GameObject playerStatsUIPanel;
    public GameObject hpBar;

    public int power;
    public int defence;
    public float hp;
    public float maxHP;
    public int luck;
    public Resistance[] resistances;

    private float barLength = 0;

    public void doDamage(float amount, ElementType ele)
    {
        Debug.Log("Raw dmg:" + amount + " & ele: " + ele.ToString());

        float resist = resistances[(int)ele].resistanceValue / 100;
        float dmg = amount - (amount * resist);
        
        Debug.Log("Resist %: " + resistances[(int)ele].resistanceValue+ " against "+ele.ToString());
        Debug.Log("new Dmg= " + dmg);
        hp -= dmg;
        UpdateHealth();
    }

    public void doHeal(int amount)
    {
        hp += amount;
        UpdateHealth();
    }

    void UpdateHealth()
    {
        RectTransform bar = hpBar.GetComponent<RectTransform>();
        if (barLength == 0)
            barLength = bar.anchoredPosition.x;
        // Scale
        float xValue = hp / maxHP;
        if (xValue > 1f)
            xValue = 1f;
        Vector3 scale = new Vector3(xValue, 1f, 1f);
        bar.localScale = scale;
        // Position
        float xPos = barLength * xValue;
        Vector3 pos = new Vector3(xPos, bar.anchoredPosition.y);
        bar.GetComponent<RectTransform>().anchoredPosition = pos;
        // Text
        bar.parent.FindChild("Text").GetComponent<Text>().text = hp + " / " + maxHP;
    }
}

[System.Serializable]
public struct Resistance
{
    public string resistanceName;
    public float resistanceValue;
}

