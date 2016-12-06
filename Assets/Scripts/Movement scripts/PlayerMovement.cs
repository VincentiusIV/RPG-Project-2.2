using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector2 speed;
    [SerializeField] private GameObject bulletPreFab;
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float firingSpeedPerSec;
    [SerializeField] private GameObject invRef;
    [SerializeField] private GameObject menuRef;
    [SerializeField] private ButtonFunctionality buttonScript;
    private Rigidbody2D rig;
    private Vector2 movement;
    private float shootTime = 0;

    void Start(){
        rig = transform.parent.GetComponent<Rigidbody2D>();
    }

    void Update() {
        float movX = Input.GetAxis("Horizontal");
        float movY = Input.GetAxis("Vertical");
        movement = new Vector2(speed.x * movX, speed.y * movY);
        movement *= Time.deltaTime;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        float angleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
        float angleDeg = (180 / Mathf.PI) * angleRad;
        transform.rotation = Quaternion.Euler(0, 0, angleDeg);
        transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angleDeg);
        
        shootTime += 1f/60f;
        if (Input.GetMouseButtonDown(0) && invRef.activeInHierarchy == false && menuRef.activeInHierarchy == false) {
            if (shootTime >= firingSpeedPerSec) {
                Shoot();
                shootTime = 0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            buttonScript.SwitchActive("Inventory_Panel");
        }
        if (Input.GetKeyDown(KeyCode.Escape)){
            buttonScript.SwitchActive("Menu_Panel");
        }
    }

    void FixedUpdate() {
        if (menuRef.activeInHierarchy == false){
            rig.velocity = movement;
        }
        else {
            rig.velocity = Vector2.zero;
        }
    }

    void Shoot() {
        GameObject BulletClone = (GameObject)Instantiate(bulletPreFab, bulletSpawnPoint.transform.position, transform.rotation);
        BulletClone.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed);
        Destroy(BulletClone, 0.6f);
    }
}
