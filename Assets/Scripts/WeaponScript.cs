using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour
{
    private string wepName;
    private SpriteRenderer sr; // change this to animator in the future

    private PlayerStats ps; // maybe not needed if dmg is calculated beforehand

	void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // ps = GetComponent
    }

    public void MeleeAttack(int dmg, int range)
    {
        /* Modify collider radius with range
         * change damage based on player stats
         * Do dmg to rigidbodies with the right tags inside own collider
         * 
         * */
    }

    public void RangedAttack(int dmg, int range, GameObject proj, float projSpeed)
    {
        /* Instantiate(proj, );
         * change damage based on player stats
         * pass range to projectile script
         * */

    }

    public void SpecialAttack()
    {
        // Something special
    }
}
