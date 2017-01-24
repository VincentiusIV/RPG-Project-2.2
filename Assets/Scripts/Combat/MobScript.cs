using UnityEngine;
using System.Collections;

public class MobScript : MonoBehaviour {

    // Public Variables
    public PlayerStats enemyStats;

    // Serialized Private Variables
    [SerializeField]private GameObject lootBox;
    [SerializeField]private InventoryData[] lootItemIDs;

	// Use this for initialization
	void Start () {
        // Setting up loot
        lootBox.GetComponent<NPCdata>().invData = lootItemIDs;
	}

    // Update is called once per frame
    void Update()
    {
        if(enemyStats.hp <= 0)
        {
            OnDeath();
            Destroy(gameObject);
        }
    }

    public void OnDeath()
    {
        Instantiate(lootBox, transform, true);
    }


}
