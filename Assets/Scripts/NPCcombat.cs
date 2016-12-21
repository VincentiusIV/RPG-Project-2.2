using UnityEngine;
using System.Collections;

public class NPCcombat : MonoBehaviour
{
    private int hp;

    public void Damage(int amount)
    {
        hp += amount;
    }
}
