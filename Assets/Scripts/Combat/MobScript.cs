using UnityEngine;
using System.Collections;
// Author: Vincent Versnel
// Mobscript holds enemy combat data
public class MobScript : MonoBehaviour {

    // Public Variables
    public int npcID;
    public CombatStats enemyStats;
    public ElementType attackElement;

    // Public & Hidden
    [HideInInspector]public Item thisEnemy;
    [HideInInspector]public AIMovement moveScript;

    // Serialized Private Variables
    [SerializeField]private GameObject lootBox;
    [SerializeField]private InventoryData[] lootItemIDs;

    private DatabaseHandler db;
    private GameObject player;
    private ButtonFunctionality bf;

    private bool inCombat;
    private bool isAlive;
    private float nextAttack;

    void Start ()
    {
        isAlive = true;
        bf = GameObject.FindWithTag("UI").GetComponent<ButtonFunctionality>();
        db = GameObject.FindWithTag("Inventory").GetComponent<DatabaseHandler>();
        player = GameObject.FindWithTag("Player");
        moveScript = transform.GetChild(0).GetComponent<AIMovement>();
        thisEnemy = db.FetchItemByID(npcID);

        // Setting up loot
        lootBox.GetComponent<NPCdata>().invData = lootItemIDs;
	}

    // Update is called once per frame
    void Update()
    {
        if (bf.canPlay)
            HealthCheck();
    }

    // public stop attack animation is called on the last frame of the attacking animation
    public void StopAttackAnimation()
    {
        GetComponent<Animator>().SetBool("isAttacking", false);
    }

    // drops loot on death
    public void HealthCheck()
    {
        if (enemyStats.hp <= 0 && isAlive)
        {
            isAlive = false;
            Instantiate(lootBox, transform.position, Quaternion.identity);
            lootBox.GetComponent<NPCdata>().Initialise();
            Destroy(gameObject);
        }
    }
}
