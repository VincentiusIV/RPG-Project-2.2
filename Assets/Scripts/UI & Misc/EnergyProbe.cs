using UnityEngine;
using System.Collections;

public class EnergyProbe : MonoBehaviour {

    public Transform droneTf;

    private float energyAmount;
    private float energyPerSecond;
    private GameController gc;
    private IEnumerator sapping;
    private bool isSappingRunning;

    private PlayerMovement player;

    private AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        sapping = DrainEnergy();
        energyAmount = Random.Range(50 , 100);
        energyPerSecond = gc.energyDrainPerSecond;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.CompareTag("Player") && Input.GetButton("A_1"))
        {
            if(!isSappingRunning)
            {
                player.drone.GetComponent<DroneAI>().targetToFollow = droneTf;
                StartCoroutine(sapping);
                isSappingRunning = true;
            } 
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
       /* if(col.CompareTag("Player"))
        {
            if(isSappingRunning)
            {
                StopCoroutine(sapping);
                isSappingRunning = false;
            }
                
        }*/
    }

    IEnumerator DrainEnergy()
    {
        while (energyAmount > 0)
        {
            energyAmount -= energyPerSecond;
            for (int i = 0; i < energyPerSecond; i++)
            {
                gc.AddEnergy();
            }
            audio.Play();
            Debug.Log("energy amount " + energyAmount);
            yield return new WaitForSeconds(1f);
        }
        player.drone.GetComponent<DroneAI>().targetToFollow = player.transform;
    }
}
