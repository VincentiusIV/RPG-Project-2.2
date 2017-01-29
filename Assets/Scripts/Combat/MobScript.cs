﻿using UnityEngine;
using System.Collections;

public class MobScript : MonoBehaviour {

    // Public Variables
    public int npcID;
    public CombatStats enemyStats;
    public float moveSpeed;

    // Public & Hidden
    [HideInInspector]public Item thisEnemy;

    // Serialized Private Variables
    [SerializeField]private GameObject lootBox;
    [SerializeField]private InventoryData[] lootItemIDs;


    private DatabaseHandler db;
    private GameObject player;
    private ButtonFunctionality bf;

    private bool inCombat;
    private bool isAlive;

    void Start ()
    {
        isAlive = true;
        bf = GameObject.FindWithTag("UI").GetComponent<ButtonFunctionality>();
        db = GameObject.FindWithTag("Inventory").GetComponent<DatabaseHandler>();
        player = GameObject.FindWithTag("Player");
        thisEnemy = db.FetchItemByID(npcID);

        // Setting up loot
        lootBox.GetComponent<NPCdata>().invData = lootItemIDs;
	}

    // Update is called once per frame
    void Update()
    {
        if(bf.canPlay)
            HealthCheck();
    }

    public void HealthCheck()
    {
        // Regeneration when out of combat
        /*if (enemyStats.hp < enemyStats.maxHP && enemyStats.hp > 0 && !inCombat)
        {
            enemyStats.hp += enemyStats.maxHP / 10;
        }*/
        if (enemyStats.hp <= 0 && isAlive)
        {
            isAlive = false;
            Instantiate(lootBox, transform.position, Quaternion.identity);
            lootBox.GetComponent<NPCdata>().Initialise();
            Destroy(gameObject);
        }
    }
    


    public void CheckToAttack()
    {

    }
}
