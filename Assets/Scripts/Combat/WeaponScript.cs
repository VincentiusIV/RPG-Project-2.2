using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour
{    
    // Public Fields
    public bool canMelee;
    public Melee melee;
    public bool canRange;
    public Projectile projectile;

    [SerializeField]private GameObject projectileGO;
    [SerializeField]private Sprite[] projectileSprites;
    // Private Fields
    private string wepName;
    private SpriteRenderer sr; // change this to animator in the future

    private PlayerStats ps; // maybe not needed if dmg is calculated beforehand
    private GameObject spawnPos;

    private CircleCollider2D destroyRange;
    private GameObject meleeRange;
    private float nextShot;

    /* If you want a weapon to melee attack, attackSpeed should be above 0
     * same counts for range attack except that bulletSpeed needs to be above 0
     */
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        spawnPos = transform.FindChild("ProjectileSpawnPoint").gameObject;
        destroyRange = transform.GetChild(1).GetComponent<CircleCollider2D>();
        meleeRange = transform.GetChild(2).gameObject;
    }

    public void CheckWeaponType()
    {
        if (melee.attackSpeed == 0)
            canMelee = false;
        else canMelee = true;
        if (projectile.bulletSpeed == 0)
            canRange = false;
        else canRange = true;
    }

    Sprite ChooseSprite(ElementType ele)
    {
        switch(ele)
        {
            case ElementType.fire:
                return projectileSprites[0];
            case ElementType.water:
                return projectileSprites[1];
            case ElementType.aether:
                return projectileSprites[2];
            case ElementType.electricity:
                return projectileSprites[3];
        }
        return new Sprite();
    }
    public void Attack()
    {
        if (canMelee)
            MeleeAttack();
        else if (canRange)
            RangedAttack();

    }
    void MeleeAttack()
    {
        if (Time.time > nextShot)
        {
            nextShot = Time.time + (float)projectile.attackSpeed / 10;

            Debug.Log("Melee attack with " + gameObject.name);

            meleeRange.SetActive(true);
            StartCoroutine(MeleeAttackSpeed(melee.attackSpeed));
            // start animation

            /* Modify collider radius with range
                * change damage based on player stats
                * Do dmg to rigidbodies with the right tags inside own collider
                * 
                * */
        }
    }

    IEnumerator MeleeAttackSpeed(double attSp)
    {
        yield return new WaitForSeconds((float)attSp);
        meleeRange.SetActive(false);
    }

    void RangedAttack()
    {
        Debug.Log("Ranged Attack with "+gameObject.name);
        if (Time.time > nextShot)
        {
            nextShot = Time.time + (float)projectile.attackSpeed / 10;

            spawnPos = transform.FindChild("ProjectileSpawnPoint").gameObject;

            destroyRange.radius = projectile.range;

            projectileGO.GetComponent<BulletScript>().thisData = projectile;
            projectileGO.GetComponent<SpriteRenderer>().sprite = ChooseSprite(projectile.element);

            if (projectileGO != null && spawnPos != null)
                Instantiate(projectileGO, spawnPos.transform.position, spawnPos.transform.rotation);
        }
    }
    
    public void SpecialAttack()
    {
        // Something special
    }
}

[System.Serializable]
public struct Melee
{
    public ElementType element;
    public int damage;
    public int attackSpeed;
    public int range;

    public Melee(ElementType ele, int dmg, int attSp, int ran)
    {
        element = ele;
        damage = dmg;
        attackSpeed = attSp;
        range = ran;
    }
}

[System.Serializable]
public struct Projectile
{
    public ElementType element;
    public int damage;
    public int attackSpeed;
    public double bulletSpeed;
    public int range;

    public Projectile(ElementType ele, int dmg, int attSp, double bulletSp, int ran)
    {
        element = ele;
        damage = dmg;
        attackSpeed = attSp;
        bulletSpeed = bulletSp;
        range = ran;
    }
}

[System.Serializable]
public enum ProjectileType
{
    bullet,
    beam,
    flames,
}


