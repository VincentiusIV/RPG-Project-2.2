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
    private float currentHP;
    private Rigidbody2D rig;
    private GameObject player;
    private bool seesPlayer;
    private bool inRange;
    private float shootTime = 0;
    private SpriteRenderer ren;

    void Start () {
        rig = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        currentHP = maxHP;
        ren = GetComponent<SpriteRenderer>();
    }

    void Update() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right);
        Debug.Log(hit.collider.gameObject.name);
        if (hit.collider.gameObject.tag == "Player") {
            seesPlayer = true;
            inRange = true;
            Debug.Log("HIttttt");
        }
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
            //moveing
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }

        //HP
        ren.color = Color.Lerp(Color.red, Color.green, currentHP / 100);
        if (currentHP < 100 && currentHP > 0){
            currentHP += 1 / 60f;
        }
        if (currentHP <= 0){
            Destroy(gameObject);
        }
    }

    void FOV() {

    }

    void Move() {
        
    }

    void OnCollisionEnter2D(Collision2D coll){
        if (!isRanged && coll.gameObject.tag == "Player"){
            StartCoroutine(Stab());
        }
    }

    void Shoot() {
        GameObject BulletClone = (GameObject)Instantiate(bulletPreFab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
        BulletClone.transform.parent = transform;
        BulletClone.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed);
        Destroy(BulletClone, 0.6f);
    }

    IEnumerator Stab(){
        transform.GetChild(0).transform.Translate(Vector2.right);
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).transform.Translate(-Vector2.right);
        yield return new WaitForSeconds(0.1f);
    }

    public void doDmg(int damage) {
        currentHP -= damage;
    }
}
