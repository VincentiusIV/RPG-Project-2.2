using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour
{
// Private Fields
    private string wepName;
    private SpriteRenderer sr; // change this to animator in the future

    private PlayerStats ps; // maybe not needed if dmg is calculated beforehand
    private Transform spawnPos;

// Private & Serialized Fields
    [SerializeField]
    private Melee melee;
    [SerializeField]
    private Projectile projectile;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        spawnPos = transform.FindChild("ProjectileSpawnPoint").transform;
        // ps = GetComponent

        transform.GetChild(1).GetComponent<CircleCollider2D>().radius = projectile.range;
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
        BulletScript proj = projectile.go.GetComponent<BulletScript>();
        proj.damage = projectile.damage;
        proj.speed = projectile.speed;
        proj.range = projectile.range;

        if(projectile.sprite != null)
            projectile.go.GetComponent<SpriteRenderer>().sprite = projectile.sprite;

        Instantiate(projectile.go, spawnPos.position, spawnPos.rotation);
    }

    public void SpecialAttack()
    {
        // Something special
    }
}

[System.Serializable]
public struct Melee
{
    public Sprite sprite;
    public float damage;
    public float attackSpeed;
    public float range;
}

[System.Serializable]
public struct Projectile
{
    public Sprite sprite;
    public GameObject go;
    public float damage;
    public float attackSpeed;
    public float speed;
    public float range;
}


