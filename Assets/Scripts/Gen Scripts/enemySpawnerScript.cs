using UnityEngine;
using System.Collections;

public class enemySpawnerScript : MonoBehaviour {
    [SerializeField] private GameObject[] enemyPreFabs;
    [SerializeField] private GameObject lvlParent;

    void Start () {
        int random = Random.Range(0,2);
        lvlParent = GameObject.Find("lvlParent");
        GameObject enemy = (GameObject)Instantiate(enemyPreFabs[random], transform.position, enemyPreFabs[random].transform.rotation);
        enemy.transform.parent = lvlParent.transform;
	}
	
	void Update () {
	    //write code for continously spawning enemies?
	}
}
