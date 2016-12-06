using UnityEngine;
using System.Collections;

public class enemySpawnerScript : MonoBehaviour {
    [SerializeField] private GameObject enemyPreFab;
    [SerializeField] private GameObject lvlParent;

    void Start () {
        lvlParent = GameObject.Find("lvlParent");
        GameObject enemy = (GameObject)Instantiate(enemyPreFab, transform.position, enemyPreFab.transform.rotation);
        enemy.transform.parent = lvlParent.transform;
	}
	
	void Update () {
	    //write code for continously spawning enemies
	}
}
