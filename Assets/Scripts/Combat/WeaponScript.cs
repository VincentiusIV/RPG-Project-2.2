using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour
{
    // Public Fields
    public LayerMask LayersToHit;
    public float hitForce;

    [HideInInspector]public Item thisWeapon;
    public bool hasWeaponEquipped;

    public Projectile[] projectiles;

    [SerializeField]private GameObject gunSmoke;
    [SerializeField]private GameObject bulletTrail;
    [SerializeField]private GameObject projectileGO;
    [SerializeField]private Sprite[] projectileSprites;
    // Private Fields
    private string wepName;
    private SpriteRenderer sr; // change this to animator in the future

    private PlayerMovement playerStats; // maybe not needed if dmg is calculated beforehand
    private Transform spawnPos;
    private Transform aim;
    private LineRenderer beamLine;
    private CircleCollider2D destroyRange;
    private float nextShot;
    private AudioSource weaponAudio;

    private bool isShotFXRunning;
    /* If you want a weapon to melee attack, attackSpeed should be above 0
     * same counts for range attack except that bulletSpeed needs to be above 0
     */
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        beamLine = GetComponent<LineRenderer>();
        beamLine.enabled = false;
        aim = transform.parent.parent.FindChild("Aim");
        spawnPos = transform.GetChild(0);

        weaponAudio = GetComponent<AudioSource>();
        
        playerStats = transform.parent.parent.GetComponent<PlayerMovement>();

        for (int i = 0; i < 4; i++)
        {
            projectiles[i] = new Projectile(i);
        }
    }

    public void RangedAttack(int ammo)
    {
        if (Time.time > nextShot && hasWeaponEquipped)
        {
            weaponAudio.Play();

            nextShot = Time.time + (float)thisWeapon.RangeAttackSpeed / 10;
            
            ShootFX(ammo);

            if (projectiles[ammo].attackSpeed > 0)
            {
                RangeBeamAttack(ammo);

            }  
        }
    }

    void RangeBeamAttack(int ammo)
    {
        Debug.Log("range beam attack with ammo "+ projectiles[ammo].element);

        Vector2 end = new Vector2(aim.position.x, aim.position.y);
        Vector2 start = new Vector2(spawnPos.position.x, spawnPos.position.y);

        RaycastHit2D hit = Physics2D.Raycast(start, end - start, 1000, LayersToHit);

        if (!isShotFXRunning)
            StartCoroutine(ShotEffect(.25f));

        if (hit)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                MobScript hitEnemy = hit.collider.GetComponent<MobScript>();

                // WATER + ELECTRICITY = EXTRA DMG
                if(thisWeapon.RangeElement == ElementType.water.ToString() && projectiles[ammo].element == ElementType.electricity || thisWeapon.RangeElement == ElementType.electricity.ToString() && projectiles[ammo].element == ElementType.water)
                {
                    Debug.Log("water + electricity");
                }

                // WATER + AETHER = DECOY; all enemies aggro it for 1 second; leaves shadowy copy of the player 
                else if (thisWeapon.RangeElement == ElementType.water.ToString() && projectiles[ammo].element == ElementType.aether || thisWeapon.RangeElement == ElementType.aether.ToString() && projectiles[ammo].element == ElementType.water)
                {
                    Debug.Log("WATER + AETHER");
                }

                // FIRE + ELECTRICITY = 2s beam that does 1 dmg per tick; penetrates enemies
                else if (thisWeapon.RangeElement == ElementType.fire.ToString() && projectiles[ammo].element == ElementType.electricity || thisWeapon.RangeElement == ElementType.electricity.ToString() && projectiles[ammo].element == ElementType.fire)
                {
                    Debug.Log("FIRE + ELECTRICITY");
                }

                // FIRE + AETHER = Mindcontrol; enemy fights for you for a few seconds
                else if (thisWeapon.RangeElement == ElementType.fire.ToString() && projectiles[ammo].element == ElementType.aether || thisWeapon.RangeElement == ElementType.aether.ToString() && projectiles[ammo].element == ElementType.fire)
                {
                    Debug.Log(" FIRE + AETHER");
                }

                // FIRE + WATER = Steam; slows enemies
                else if (thisWeapon.RangeElement == ElementType.water.ToString() && projectiles[ammo].element == ElementType.fire || thisWeapon.RangeElement == ElementType.fire.ToString() && projectiles[ammo].element == ElementType.water)
                {
                    Debug.Log("FIRE + WATER");
                    hitEnemy.moveScript.speedMultiplier = (float)thisWeapon.SlowAmount;
                    Debug.Log("Slowing enemy for " + thisWeapon.SlowAmount + "%");
                }

                // ELECTRICITY + AETHER = Chain enemies
                else if (thisWeapon.RangeElement == ElementType.electricity.ToString() && projectiles[ammo].element == ElementType.aether || thisWeapon.RangeElement == ElementType.aether.ToString() && projectiles[ammo].element == ElementType.electricity)
                {
                    Debug.Log("ELECTRICITY + AETHER");
                }

                hitEnemy.enemyStats.doDamage(projectiles[ammo].damage + thisWeapon.Power, projectiles[ammo].element);
            }

            if (hit.collider.CompareTag("Element"))
            {
                Debug.Log("hit element");
                ElementScript triggerEle = hit.collider.GetComponent<ElementScript>();

                if (triggerEle.triggerElement == projectiles[ammo].element)
                    triggerEle.Triggered();
            }
        }
        else
        {
            Debug.Log("Fire beam nananana");
        }
    }

    void Update()
    {
        if(isShotFXRunning)
        {
            beamLine.SetPosition(0, spawnPos.position);
            beamLine.SetPosition(1, (aim.position - spawnPos.position) * thisWeapon.RangeAttackRange);
        }
    }

    private IEnumerator ShotEffect(float duration)
    {
        isShotFXRunning = true;
        weaponAudio.Play();

        beamLine.enabled = true;
        yield return new WaitForSeconds(duration);
        beamLine.enabled = false;

        isShotFXRunning = false;
    }

    void KnockBack(Collider2D col)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, col.transform.position - transform.position, 100);
        Debug.Log("trying to cast");
        Debug.DrawRay(transform.position, (col.transform.position - transform.position) * 100);

        if (hit)
        {

            Debug.Log("adding force");
            Rigidbody2D rbPlayer = col.gameObject.GetComponent<Rigidbody2D>();
            rbPlayer.AddForce(-hit.normal * 100, ForceMode2D.Force);
        }

        col.transform.FindChild("AIMovement").GetComponent<AIMovement>().speedMultiplier = (int)thisWeapon.SlowAmount;
    }

    Sprite ChooseSprite(ElementType ele)
    {
        switch (ele)
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

    void ShootFX(int ammo)
    {
        Instantiate(gunSmoke, spawnPos.position, spawnPos.rotation);

        bulletTrail.GetComponent<BulletScript>().ele = projectiles[ammo].element;
        Instantiate(bulletTrail, spawnPos.position, spawnPos.rotation);
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
    public ProjectileType projectileType;
    public int damage;
    public int attackSpeed;
    public double bulletSpeed;
    public int range;

    public Projectile(ElementType ele, ProjectileType projType, int dmg, int attSp, double bulletSp, int ran)
    {
        element = ele;
        projectileType = projType;
        damage = dmg;
        attackSpeed = attSp;
        bulletSpeed = bulletSp;
        range = ran;
    }

    public Projectile(int id)
    {
        element = ElementType.none;
        projectileType = ProjectileType.beam;
        damage = 0;
        attackSpeed = 0;
        bulletSpeed = 0;
        range = 0;
    }
}

[System.Serializable]
public enum ProjectileType
{
    bullet,
    beam,
    flames,
}


