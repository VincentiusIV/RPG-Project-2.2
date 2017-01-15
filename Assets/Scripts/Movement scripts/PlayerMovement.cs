using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector2 moveSpeed;
    [SerializeField] private GameObject aim;
    public PlayerStats playerStats;
    private WeaponScript weapon;
    private Rigidbody2D rig;
    private Vector2 movement;
    private SpriteRenderer ren;
    public bool canPlay;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        ren = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (canPlay)
        {
            float rStickH = Input.GetAxis("X360_RStickX");
            float rStickV = Input.GetAxis("X360_RStickY");

            aim.transform.position = new Vector3(transform.position.x + rStickH, transform.position.y + rStickV, 0f);

            // Weapon
            if (weapon != null)
            {
                if (Input.GetAxis("X360_Triggers") < 0)
                    weapon.RangedAttack();

                if (Input.GetAxis("X360_Triggers") > 0)
                    weapon.MeleeAttack();
            }
            // move hp to player stats at some point
            //HP
            ren.color = Color.Lerp(Color.red, Color.green, playerStats.hp / playerStats.maxHP);

            /*if (playerStats.hp < 100 && playerStats.hp > 0)
            {
                playerStats.hp += 1 / 60f;
            }*/
            if (playerStats.hp <= 0)
            {
                //Respawn?? End Game?? Lifes??
            }
        }
    }
    
    void FixedUpdate()
    {
        if(canPlay)
        {
            // Rotation with mouse
            float angleRad = Mathf.Atan2(aim.transform.position.y - transform.position.y, aim.transform.position.x - transform.position.x);
            float angleDeg = (180 / Mathf.PI) * angleRad;
            transform.rotation = Quaternion.Euler(0, 0, angleDeg);
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angleDeg);

            // Movement
            float xPos = Input.GetAxis("X360_LStickX");
            float yPos = Input.GetAxis("X360_LStickY");

            Vector3 movement = new Vector3(xPos, yPos, 0f).normalized;
            rig.velocity = movement * moveSpeed.x;
        }
    }

    public void GetWeapon(WeaponScript wep)
    {
        weapon = wep;
        Debug.Log("Equipped new weapon: " + weapon.gameObject.name);

        if(weapon == null)
        {
            Debug.Log("Player is not holding any weapon");
        }
    }

    public void doDmg(int damage){
        playerStats.hp -= damage;
    }

    void OnCollisionEnter2D(Collision2D coll){
        if (coll.transform.parent != null && coll.transform.parent.name == "MeleeEnemy(Clone)"){
            playerStats.hp -= coll.transform.parent.GetComponent<EnemyScript>().dmg;
        }
        if (coll.transform.name == "Bullet(Clone)"){
            //currentHP -= coll.gameObject.GetComponent<BulletScript>().enemyScript.dmg;
        }
        if (coll.gameObject.tag == "Teleporter"){
            //reload lvl and move player to opposite side of the lvl
            GeneratingDungeon gameManagerScript = FindObjectOfType<GeneratingDungeon>();
            gameManagerScript.DestroyLevel();
            gameManagerScript.GenerateLevel();
            transform.parent.transform.position = new Vector2(0f, 0f);
        }
    }

    private void OnTriggerEnter(Collider col){
        if (col.transform.gameObject.tag == "AI") {
            col.gameObject.GetComponent<EnemyScript>().ChangetooCloseToPlayer();
            Debug.Log("Player: (should be true)" + GetComponent<EnemyScript>().tooCloseToPlayer);
        }
    }

    private void OnTriggerExit(Collider col){
        if (col.transform.gameObject.tag == "AI"){
            col.gameObject.GetComponent<EnemyScript>().ChangetooCloseToPlayer();
            Debug.Log("Player: (should be false)" + GetComponent<EnemyScript>().tooCloseToPlayer);
        }
    }
}



