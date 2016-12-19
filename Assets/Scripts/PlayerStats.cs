using UnityEngine;
using System.Collections;

public class PlayerStats
{
    int Power { get; set; }
    int Defence { get; set; }
    int HP { get; set; }

    public PlayerStats(int pow, int def, int hp)
    {
        this.Power = pow;
        this.Defence = def;
        this.HP = hp;
    }
}

