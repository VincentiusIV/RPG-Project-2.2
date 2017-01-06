using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour
{
// Private Fields
    private string wepName;
    private SpriteRenderer sr; // change this to animator in the future

    private PlayerStats ps; // maybe not needed if dmg is calculated beforehand
    private GameObject spawnPos;

    private CircleCollider2D destroyRange;
    private GameObject meleeRange;
// Public Fields
    public Melee melee;
    public Projectile projectile;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        spawnPos = transform.FindChild("ProjectileSpawnPoint").gameObject;
        if (spawnPos != null)
            Debug.Log("Spawn position assigned succesfully");
        else
            Debug.Log("WARNING! No spawn position for projectile assigned");
        destroyRange = transform.GetChild(1).GetComponent<CircleCollider2D>();
        meleeRange = transform.GetChild(2).gameObject;
    }

    public void MeleeAttack()
    {
        Debug.Log("Melee attack with "+ gameObject.name);

        meleeRange.SetActive(true);
        StartCoroutine(MeleeAttackSpeed(melee.attackSpeed));
        // start animation

        /* Modify collider radius with range
         * change damage based on player stats
         * Do dmg to rigidbodies with the right tags inside own collider
         * 
         * */
    }

    IEnumerator MeleeAttackSpeed(double attSp)
    {
        yield return new WaitForSeconds((float)attSp);
        meleeRange.SetActive(false);
    }

    public void RangedAttack()
    {
        spawnPos = transform.FindChild("ProjectileSpawnPoint").gameObject;
        if (spawnPos != null)
            Debug.Log("Spawn position assigned succesfully");
        else
            Debug.Log("WARNING! No spawn position for projectile assigned");

        Debug.Log("Ranged attack with "+ gameObject.name);
        destroyRange.radius = projectile.range;

        BulletScript proj = projectile.go.GetComponent<BulletScript>();
        proj.damage = projectile.damage;
        proj.speed = projectile.speed;
        proj.range = projectile.range;

        if (projectile.go != null && spawnPos != null)
            Instantiate(projectile.go, spawnPos.transform.position, spawnPos.transform.rotation);
        else
            Debug.Log("Projectile Game Object is empty");
    }

    public void SpecialAttack()
    {
        // Something special
    }
}

[System.Serializable]
public struct Melee
{
    public int damage;
    public int attackSpeed;
    public int range;
}

[System.Serializable]
public struct Projectile
{
    public GameObject go;
    public int damage;
    public int attackSpeed;
    public int speed;
    public int range;
}


