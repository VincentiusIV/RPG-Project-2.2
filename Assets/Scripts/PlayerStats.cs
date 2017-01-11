using UnityEngine;
using System.Collections;

public class PlayerStats
{
    private int Power { get; set; }
    private int Defence { get; set; }
    private int HP { get; set; }
    private int Luck { get; set; }

    public PlayerStats(int pow, int def, int hp, int luck)
    {
        this.Power = pow;
        this.Defence = def;
        this.HP = hp;
        this.Luck = luck;
    }

    void doDamage(int amount)
    {

    }

    void doHeal(int amount)
    {
        HP += amount;
    }
}

