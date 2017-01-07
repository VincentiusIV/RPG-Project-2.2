using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // To-do: move combat related code to WeaponScript
    [SerializeField] private Vector2 moveSpeed;
    

    [SerializeField] private int maxHP;
    private float currentHP;

    private WeaponScript weapon;

    private Rigidbody2D rig;
    private Vector2 movement;
    private float shootTime = 0;
    private SpriteRenderer ren;

    public bool canPlay;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        ren = GetComponent<SpriteRenderer>();

        currentHP = maxHP;
    }

    void Update()
    {
        if(canPlay)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            float angleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
            float angleDeg = (180 / Mathf.PI) * angleRad;
            transform.rotation = Quaternion.Euler(0, 0, angleDeg);
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angleDeg);

            if (weapon != null)
            {
                if (Input.GetButton("Fire1"))
                    weapon.RangedAttack();

                if (Input.GetButton("Fire2"))
                    weapon.MeleeAttack();
            }
            else
                Debug.Log("Weapon is not assigned");

            //HP
            ren.color = Color.Lerp(Color.red, Color.green, currentHP / 100);

            if (currentHP < 100 && currentHP > 0)
            {
                currentHP += 1 / 60f;
            }
            if (currentHP <= 0)
            {
                //Respawn?? End Game?? Lifes??
            }

            FollowPlayer();
        }
    }

    public GameObject target;
    public float camDistance;
    public float lerpIntensity;

    void FollowPlayer()
    {
        Vector3 targetPosition = target.transform.position + new Vector3(0f, 0f, camDistance);
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpIntensity);
        //transform.LookAt(target.transform);
    }

    /*void FixedUpdate() {
        float movX = Input.GetAxis("Horizontal");
        float movY = Input.GetAxis("Vertical");
        movement = new Vector2(speed.x * movX, speed.y * movY);
        movement *= Time.deltaTime;
        //if (canPlay){
            transform.Translate(movement);
       // }
        //else{
           // return;
       // }
    }*/
    
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
        currentHP -= damage;
    }

    void OnCollisionEnter2D(Collision2D coll){
        if (coll.transform.parent != null && coll.transform.parent.name == "MeleeEnemy(Clone)"){
            currentHP -= coll.transform.parent.GetComponent<EnemyScript>().dmg;
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
}

