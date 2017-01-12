﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector2 moveSpeed;
    [SerializeField] private int maxHP;
    [SerializeField] private bool useController;
    [SerializeField] private Vector2 cursorMoveSpeed;
    [SerializeField] private GameObject cursorObject;
    private float currentHP;
    private WeaponScript weapon;
    private Rigidbody2D rig;
    private Vector2 movement;
    private SpriteRenderer ren;
    public bool canPlay;

    void Start(){
        rig = GetComponent<Rigidbody2D>();
        ren = GetComponent<SpriteRenderer>();
        currentHP = maxHP;
    }

    void Update()
    {
        if (true/*Input.GetButton("X360_Horizontal") || Input.GetButton("X360_Vertical")*/)
        {
            float xPos = Input.GetAxis("Horizontal") * cursorMoveSpeed.x;
            float yPos = Input.GetAxis("Vertical") * cursorMoveSpeed.y;
            Vector3 cursorMovement = new Vector3(xPos, yPos, 0f);

            cursorObject.transform.Translate(cursorMovement);
        }

        if (canPlay)
        {
            // Weapon
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
        }

        if(useController)
        {
            float xPos = Input.GetAxis("X360_Horizontal");
            float yPos = Input.GetAxis("X360_Vertical");
            Vector2 mouseMovement = new Vector2(xPos, yPos);

            //Cursor.SetCursor(cursorTexture, mouseMovement, CursorMode.Auto);
        }
    }
    
    void FixedUpdate()
    {
        if(canPlay)
        {
            // Rotation
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            float angleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
            float angleDeg = (180 / Mathf.PI) * angleRad;
            transform.rotation = Quaternion.Euler(0, 0, angleDeg);
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angleDeg);

            // Movement
            float xPos = Input.GetAxis("X360_Horizontal");
            float yPos = Input.GetAxis("X360_Vertical");

            Vector3 movement = new Vector3(xPos, yPos, 0f).normalized;

            if (Input.GetButtonUp("X360_Horizontal") || Input.GetButtonUp("X360_Vertical"))
            {
                Debug.Log("Velocity reset on player");
                rig.velocity = Vector3.zero;
            }
            else
            {
                rig.velocity = movement * moveSpeed.x;
            }
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

