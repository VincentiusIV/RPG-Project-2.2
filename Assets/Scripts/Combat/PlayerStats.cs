using UnityEngine;
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
    }

    void UpdateHealth()
    {
        RectTransform bar = hpBar.GetComponent<RectTransform>();
        // Scale
        float xValue = hp / maxHP;
        if (xValue > 1f)
            xValue = 1f;
        Vector3 scale = new Vector3(xValue, 1f, 1f);
        bar.localScale = scale;
        // Position
        float xPos = bar.anchoredPosition.x * xValue;
        Vector3 pos = new Vector3(xPos, bar.anchoredPosition.y);
        bar.GetComponent<RectTransform>().anchoredPosition = pos;
    }
}

[System.Serializable]
public struct Resistance
{
    public string resistanceName;
    public float resistanceValue;
}

