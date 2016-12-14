using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector2 speed;
    [SerializeField] private GameObject bulletPreFab;
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float firingSpeedPerSec;
    [SerializeField] private ButtonFunctionality buttonScript;
    [SerializeField] public int dmg;
    [SerializeField] private int maxHP;
    private float currentHP;
    private Rigidbody2D rig;
    private Vector2 movement;
    private float shootTime = 0;
    private SpriteRenderer ren;
    private GameObject invRef;
    private GameObject menuRef;

    void Start(){
        rig = transform.parent.GetComponent<Rigidbody2D>();
        currentHP = maxHP;
        ren = GetComponent<SpriteRenderer>();
        invRef = GameObject.Find("Inventory_Panel");
        menuRef = GameObject.Find("Menu_Panel");
        menuRef.SetActive(false);
        invRef.SetActive(false);
        GameObject.Find("Merchant_Inventory_Panel").SetActive(false);
    }

    void Update() {
        //Movement
        float movX = Input.GetAxis("Horizontal");
        float movY = Input.GetAxis("Vertical");
        movement = new Vector2(speed.x * movX, speed.y * movY);
        movement *= Time.deltaTime;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        float angleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
        float angleDeg = (180 / Mathf.PI) * angleRad;
        transform.rotation = Quaternion.Euler(0, 0, angleDeg);
        transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angleDeg);
        
        //Shooting
        shootTime += ((1f / 60f) * 100) * firingSpeedPerSec;
        if (Input.GetMouseButtonDown(0) && invRef.activeInHierarchy == false && menuRef.activeInHierarchy == false) {
            if (shootTime >= 100) {
                Shoot();
                shootTime = 0f;
            }
        }

        //Menus
        if (Input.GetKeyDown(KeyCode.I)) {
            buttonScript.SwitchActive(invRef.name);
        }
        if (Input.GetKeyDown(KeyCode.Escape)){
            buttonScript.SwitchActive(menuRef.name);
        }

        //HP
        ren.color = Color.Lerp(Color.red, Color.green, currentHP / 100);
        if (currentHP < 100 && currentHP > 0){
            currentHP += 1 / 60f;
        }
        if (currentHP <= 0){
            //Respawn?? End Game?? Lifes??
        }
    }

    void FixedUpdate() {
        if (menuRef != null && menuRef.activeInHierarchy == false){
            rig.velocity = movement;
        }
        else {
            rig.velocity = Vector2.zero;
        }
    }

    void Shoot() {
        GameObject BulletClone = (GameObject)Instantiate(bulletPreFab, bulletSpawnPoint.transform.position, transform.rotation);
        BulletClone.transform.parent = transform;
        BulletClone.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed);
        Destroy(BulletClone, 0.6f);
    }

    public void doDmg(int damage){
        currentHP -= damage;
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.transform.parent != null && coll.transform.parent.name == "MeleeEnemy(Clone)") {
            currentHP -= coll.transform.parent.GetComponent<EnemyScript>().dmg;
        }
        if (coll.transform.name == "Bullet(Clone)") {
            currentHP -= coll.gameObject.GetComponent<BulletScript>().enemyScript.dmg;
        }
        if (coll.gameObject.tag == "Teleporter") {
            //reload lvl and move player to opposite side of the lvl
            GeneratingDungeon gameManagerScript = FindObjectOfType<GeneratingDungeon>();
            gameManagerScript.DestroyLevel();
            gameManagerScript.GenerateLevel();
            transform.parent.transform.position = new Vector2(0f, 0f);
        }
    }
}
