using UnityEngine;
using System.Collections;

public class EnergyProbe : MonoBehaviour {

    public Transform droneTf;
    public GameObject notification;

    private float energyAmount;
    private float energyPerSecond;
    private GameController gc;
    private IEnumerator sapping;
    private bool isSappingRunning;

    void Start()
    {
        gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        sapping = DrainEnergy();
        energyAmount = Random.Range(50 , 100);
        energyPerSecond = gc.energyDrainPerSecond;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.CompareTag("Player") && Input.GetButton("Interact"))
        {
            Debug.Log("in da zone");
            if(!isSappingRunning)
            {
                StartCoroutine(sapping);
                isSappingRunning = true;
            } 
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            Debug.Log("out da zone");
            if(isSappingRunning)
            {
                StopCoroutine(sapping);
                isSappingRunning = false;
            }
                
        }
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
            
            Debug.Log("energy amount " + energyAmount);
            yield return new WaitForSeconds(1f);
        }
    }
}
