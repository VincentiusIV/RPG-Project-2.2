using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerStats
{
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
        Debug.Log("Resist = " + resist);
        amount *= resist;
        Debug.Log("Resist %: " + resistances[(int)ele].resistanceValue+ " against "+ele.ToString());
        Debug.Log("Total dmg = " + amount);
        hp -= amount;
    }

    public void doHeal(int amount)
    {
        hp += amount;
    }
}

[System.Serializable]
public struct Resistance
{
    public string resistanceName;
    public int resistanceValue;
}

