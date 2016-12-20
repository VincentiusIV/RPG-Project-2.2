using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour
{
    private string wepName;
    private SpriteRenderer sr; // change this to animator in the future

    private PlayerStats ps; // maybe not needed if dmg is calculated beforehand
    private Transform spawnPos;

    [SerializeField]
    private Sprite projectileSprite;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private float projectileDamage;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private float projectileRange;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        spawnPos = transform.FindChild("ProjectileSpawnPoint").transform;
        // ps = GetComponent
    }

    public void MeleeAttack()
    {
        /* Modify collider radius with range
         * change damage based on player stats
         * Do dmg to rigidbodies with the right tags inside own collider
         * 
         * */
    }

    public void RangedAttack()
    {
        BulletScript proj = projectile.GetComponent<BulletScript>();
        proj.damage = projectileDamage;
        proj.speed = projectileSpeed;
        proj.range = projectileRange;

        if(projectileSprite != null)
            projectile.GetComponent<SpriteRenderer>().sprite = projectileSprite;

        Instantiate(projectile, spawnPos.position, spawnPos.rotation);
    }

    public void SpecialAttack()
    {
        // Something special
    }
}
