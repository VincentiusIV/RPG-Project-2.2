using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerStats
{
    public int power;
    public int defence;
    public int hp;
    public int maxHP;
    public int luck;
    public Resistance[] resistances;

    void doDamage(int amount, ElementType ele)
    {
        Debug.Log("Raw dmg:" + amount + " & ele: " + ele.ToString());
        amount /= resistances[(int)ele].resistanceValue;
        Debug.Log("Resist %: " + resistances[(int)ele].resistanceValue+ " against "+ele.ToString());
        Debug.Log("Total dmg = " + amount);
        hp -= amount;
    }

    void doHeal(int amount)
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

