using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
    [SerializeField] private bool isRanged;
    [SerializeField] private GameObject bulletPreFab;
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float firingSpeedPerSec;
    [SerializeField] private int maxHP;
    [SerializeField] private DatabaseHandler inventorySystem;
    [SerializeField] private Inventory inventory;
    [SerializeField] public int dmg;
    private Vector2 direction = Vector2.zero;
    private float currentHP;
    private GameObject player;
    private bool seesPlayer = false;
    private bool inRange = false;
    private float shootTime = 0;
    private SpriteRenderer ren;
    private float counter = 0;
    private float followCounter = 0;
    private float random = 0;
    [HideInInspector] public bool tooCloseToPlayer = false;

    void Start () {
        inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        player = GameObject.Find("Player");
        currentHP = maxHP;
        ren = GetComponent<SpriteRenderer>();
    }

    void Update() {
        //movement & combat
        if (seesPlayer && inRange && isRanged){
            //turning
            float angleRad = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x);
            float angleDeg = (180 / Mathf.PI) * angleRad;
            transform.rotation = Quaternion.Euler(0, 0, angleDeg);
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angleDeg);
            //shooting
            shootTime += ((1f / 60f) * 100) * firingSpeedPerSec;
            if (shootTime >= 100){
                Shoot();
                shootTime = 0f;
            }
        }
        if (seesPlayer && !isRanged){
            //turning
            float angleRad = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x);
            float angleDeg = (180 / Mathf.PI) * angleRad;
            transform.rotation = Quaternion.Euler(0, 0, angleDeg);
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angleDeg);
            //moving
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }
        if (!seesPlayer && !inRange) {
            //idle movement
            Vector2 movement = Vector2.zero;
            if (counter >= 0){
                counter -= 0.1f;
            }
            if (counter <= 0) {
                random = Random.value;
                counter = 4f;
            }
            if (random <= 0.25) {
                transform.Translate((new Vector2(-1f, 0f) * Time.deltaTime) * movementSpeed);
            }
            else if (random <= 0.50){
                transform.Translate((new Vector2(1f, 0f) * Time.deltaTime) * movementSpeed);
            }
            else if (random <= 0.75){
                transform.Translate((new Vector2(0f, 1f) * Time.deltaTime) * movementSpeed);
            }
            else if (random <= 1){
                transform.Translate((new Vector2(0f, -1f) * Time.deltaTime) * movementSpeed);
            }
        }
        //HP
        ren.color = Color.Lerp(Color.red, Color.green, currentHP / 100);
        if (currentHP < 100 && currentHP > 0){
            currentHP += 1 / 60f;
        }
        if (currentHP <= 0){
            SpawnLoot(1);
            Destroy(gameObject);
        }
        /*if (inRange && seesPlayer && isRanged && !tooCloseToPlayer) {
            direction = transform.position - player.transform.position;
            direction.Normalize();
            transform.Translate((direction * Time.deltaTime) * movementSpeed);
            Debug.Log("Enemy: (should be false)" + tooCloseToPlayer);
        }
        if (tooCloseToPlayer && isRanged) {
            direction = -(transform.position - player.transform.position);
            direction.Normalize();
            transform.Translate((direction * Time.deltaTime) * movementSpeed);
            Debug.Log("Enemy: (should be true)" + tooCloseToPlayer);
        }*/
        if (followCounter >= 0) {
            followCounter -= Time.deltaTime;
        }
        if (followCounter > 0){
            //turning
            float angleRad = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x);
            float angleDeg = (180 / Mathf.PI) * angleRad;
            transform.rotation = Quaternion.Euler(0, 0, angleDeg);
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angleDeg);
            //moving
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }
    }

    void SpawnLoot(float dropChance){
        Debug.Log("Loot should have spawned");
        Item itemToDropInfo = inventorySystem.FetchItemByID(this.GetComponent<NPCdata>().invData[0].id);
        GameObject itemToDrop = new GameObject();
        itemToDrop.AddComponent<SpriteRenderer>();
        itemToDrop.GetComponent<SpriteRenderer>().sprite = inventory.FetchSpriteBySlug(itemToDropInfo.Type, itemToDropInfo.Slug);
        GameObject droppedItem = (GameObject)Instantiate(itemToDrop, transform.position, transform.rotation);
    }

    void OnTriggerStay2D(Collider2D coll) {
        if (coll.gameObject.tag == player.tag) {
            seesPlayer = true;
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag == player.tag) {
            seesPlayer = false;
            inRange = false;
        }
        followCounter = 4f;
    }

    private void OnCollisionEnter2D(Collision2D coll){
        if (coll.gameObject.tag == "Player") {
            ChangetooCloseToPlayer();
        }
    }

    private void OnCollisionExit2D(Collision2D coll){
        if (coll.gameObject.tag == "Player"){
            ChangetooCloseToPlayer();
        }
    }

    void Shoot() {
        GameObject BulletClone = (GameObject)Instantiate(bulletPreFab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
        BulletClone.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed);
        Destroy(BulletClone, 0.6f);
    }

    public void ChangetooCloseToPlayer() {
        if (tooCloseToPlayer == false) {
            tooCloseToPlayer = true;
        } else {
            tooCloseToPlayer = false;
        }
    }

    public void doDmg(int damage) {
        currentHP -= damage;
    }
}
