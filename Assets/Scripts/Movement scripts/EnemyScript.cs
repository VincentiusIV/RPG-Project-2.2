using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
    [SerializeField] private int fOVRange;
    [SerializeField] private bool isRanged;
    [SerializeField] private GameObject bulletPreFab;
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float firingSpeedPerSec;
    [SerializeField] public int dmg;
    [SerializeField] private int maxHP;
    [SerializeField] private DatabaseHandler inventorySystem;
    [SerializeField] private Inventory inventory;
    private float currentHP;
    private float timer;
    private GameObject player;
    private bool seesPlayer = false;
    private bool inRange = false;
    private float shootTime = 0;
    private SpriteRenderer ren;
    private int counter = 0;

    void Start () {
        inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        player = GameObject.Find("Player");
        currentHP = maxHP;
        ren = GetComponent<SpriteRenderer>();
        SpawnLoot(1);
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
            if (counter == 0) {
                IdleMovement();
                counter++;
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
    }

    IEnumerator IdleMovement(){
        float random = Random.value;
        Vector2 movement;
        if (random >= 0.75){
            movement = new Vector2(-1f, 0f) * Time.deltaTime;
            movement *= movementSpeed;
            transform.Translate(movement);
        }
        else if (random >= 0.50){
            movement = new Vector2(1f, 0f) * Time.deltaTime;
            movement *= movementSpeed;
            transform.Translate(movement);
        }
        else if (random >= 0.25){
            movement = new Vector2(0f, 1f) * Time.deltaTime;
            movement *= movementSpeed;
            transform.Translate(movement);
        }
        else if (random >= 0){
            movement = new Vector2(0f, -1f) * Time.deltaTime;
            movement *= movementSpeed;
            transform.Translate(movement);
        }
        yield return new WaitForSeconds(5f);
        if (!seesPlayer && !inRange) {
            IdleMovement();
        }
    }

    void SpawnLoot(float dropChance)
    {
        Item itemToDropInfo = inventorySystem.FetchItemByID(GetComponent<NPCdata>().invData[0].id);
        GameObject itemToDrop = new GameObject();
        itemToDrop.GetComponent<SpriteRenderer>().sprite = inventory.FetchSpriteBySlug(itemToDropInfo.Type, itemToDropInfo.Slug);
        GameObject droppedItem = (GameObject)Instantiate(itemToDrop, transform);
    }

    void OnTriggerStay2D(Collider2D coll) {
        if (coll.gameObject.tag == player.tag) {
            //seesPlayer = true;
            //inRange = true;
            int layerMask = 1 << 1;
            layerMask = ~layerMask;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, layerMask);
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == player.tag){
                seesPlayer = true;
                Debug.Log("HIttttt");
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag == player.tag) {
            seesPlayer = false;
            inRange = false;
        }
    }

    void OnCollisionEnter2D(Collision2D coll){
        if (isRanged && coll.gameObject.tag == player.tag){
            //transform.Translate(-(transform.TransformDirection(player.transform.position).normalized * Time.deltaTime));
        }
    }

    void Shoot() {
        GameObject BulletClone = (GameObject)Instantiate(bulletPreFab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
        BulletClone.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed);
        Destroy(BulletClone, 0.6f);
    }

    public void doDmg(int damage) {
        currentHP -= damage;
    }
}
