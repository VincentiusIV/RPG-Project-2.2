using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour
{
    private string wepName;
    private SpriteRenderer sr;

	void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void MeleeAttack(int dmg, int range)
    {
        // modify collider and do dmg to 
    }

    public void RangedAttack(int dmg, int range, GameObject proj)
    {
        Instantiate(proj, );
    }

    public void SpecialAttack()
    {
        // Something special
    }
}
