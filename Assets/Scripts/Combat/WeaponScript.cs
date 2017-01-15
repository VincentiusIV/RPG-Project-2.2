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

    void Update()
    {

    }

    Sprite ChooseSprite(Elements ele)
    {
        Sprite newSprite = new Sprite();

        switch(ele)
        {
            case Elements.fire:
                return newSprite = projectileSprites[0];
            case Elements.water:
                return newSprite = projectileSprites[1];
            case Elements.aether:
                return newSprite = projectileSprites[2];
            case Elements.electricity:
                return newSprite = projectileSprites[3];
        }
        return newSprite;
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
        if(Time.time > nextShot)
        {
            nextShot = Time.time + (float)projectile.attackSpeed / 10;

            spawnPos = transform.FindChild("ProjectileSpawnPoint").gameObject;
            if (spawnPos != null)
                Debug.Log("Spawn position assigned succesfully");
            else
                Debug.Log("WARNING! No spawn position for projectile assigned");

            Debug.Log("Ranged attack with " + gameObject.name);
            destroyRange.radius = projectile.range;

            projectileGO.GetComponent<BulletScript>().thisData = projectile;
            projectileGO.GetComponent<SpriteRenderer>().sprite = ChooseSprite(projectile.element);

            if (projectileGO != null && spawnPos != null)
                Instantiate(projectileGO, spawnPos.transform.position, spawnPos.transform.rotation);
            else
                Debug.Log("Projectile Game Object is empty");
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
    public Elements element;
    public int damage;
    public int attackSpeed;
    public int range;

    public Melee(Elements ele, int dmg, int attSp, int ran)
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
    public Elements element;
    public int damage;
    public int attackSpeed;
    public double bulletSpeed;
    public int range;

    public Projectile(Elements ele, int dmg, int attSp, double bulletSp, int ran)
    {
        element = ele;
        damage = dmg;
        attackSpeed = attSp;
        bulletSpeed = bulletSp;
        range = ran;
    }
}


