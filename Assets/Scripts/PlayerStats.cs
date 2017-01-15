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

    void doDamage(int amount)
    {
        hp -= amount;
    }

    void doHeal(int amount)
    {
        hp += amount;
    }
}

