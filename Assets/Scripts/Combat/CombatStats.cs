using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class CombatStats
{
    public bool worldSpaceBar;
    public GameObject hpBar;
    public DmgToScreen dmgText;

    public int power;
    public int defence;
    public float hp;
    public float maxHP;
    public int luck;
    public Resistance[] resistances;

    private float barLength = 0;

    void Awake()
    {
        dmgText = GameObject.Find("DamageNumber").GetComponent<DmgToScreen>();
    }

    public void doDamage(float amount, ElementType ele)
    {
        Debug.Log("Raw dmg:" + amount + " & ele: " + ele.ToString());

        float resist = resistances[(int)ele].resistanceValue / 100;
        float dmg = amount - (amount * resist);
        
        Debug.Log("Resist %: " + resistances[(int)ele].resistanceValue+ " against "+ele.ToString());
        Debug.Log("new Dmg= " + dmg);
        hp -= dmg;
        if (hp <= 0)
        {
            hp = 0;
        }
            

        if (!worldSpaceBar)
            UpdateHealth();
        else if (worldSpaceBar)
            UpdateWorldSpaceHealth();

        // show damage in game
        dmgText.ShowDmg(dmg, hpBar.transform.position);
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

    void UpdateWorldSpaceHealth()
    {
        Transform bar = hpBar.GetComponent<Transform>();
        if (barLength == 0)
            barLength = bar.localScale.x;

        // Scale
        float xValue = hp / maxHP;
        if (xValue > 1f)
            xValue = 1f;
        Vector3 scale = new Vector3(xValue, bar.localScale.y, 1f);
        bar.localScale = scale;

    }
}

[System.Serializable]
public struct Resistance
{
    public string resistanceName;
    public float resistanceValue;
}

