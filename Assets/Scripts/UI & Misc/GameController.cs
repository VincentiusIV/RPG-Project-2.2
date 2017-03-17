using UnityEngine;
using System.Collections;
// Author Vincent Versnel
// Controls the game by setting ambient sounds and respawning the player
// could hold future implementations 
public class GameController : MonoBehaviour {

    public AudioClip[] ambientSounds;

    // Respawning
    public Transform respawnLocation;
    
    // Objective vars
    public float totalEnergyAmount;
    public float energyDrainPerSecond;

    private ButtonFunctionality ui;
    private PlayerMovement player;
    private Inventory inv;

	// Use this for initialization
	void Start () {
        ui = GameObject.FindWithTag("UI").GetComponent<ButtonFunctionality>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        inv = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
	}
	
    // sets the ambient music
	public void SetAmbience(int id)
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = ambientSounds[id];
        GetComponent<AudioSource>().Play();
    }

    // respawns the player
    public IEnumerator RespawnPlayer()
    {
        yield return StartCoroutine(ui.FadeIn());
        SetAmbience(0);
        player.GetComponent<SpriteRenderer>().sortingOrder = 4;
        player.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
        player.transform.position = respawnLocation.position;
        Camera.main.transform.position = player.transform.position;
        player.playerStats.doHeal((int)player.playerStats.maxHP);
        yield return StartCoroutine(ui.FadeOut());
    }

    public void AddEnergy()
    {
        totalEnergyAmount += energyDrainPerSecond;
        inv.AddItem(25);
    }
}
