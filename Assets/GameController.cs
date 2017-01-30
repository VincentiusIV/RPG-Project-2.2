﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

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
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator RespawnPlayer()
    {
        yield return StartCoroutine(ui.FadeIn());
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