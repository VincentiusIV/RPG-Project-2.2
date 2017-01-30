using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flock : MonoBehaviour {

    [SerializeField]private int birdCount = 5;
    [SerializeField]private GameObject birdPrefab;

    public float spawnRadius = 3f;
    public List<GameObject> birds = new List<GameObject>();

    public Transform direction;
    public float cohesionWeight = 0.2f;
    public float alignmentWeight = 0.1f;
    public float seperationWeight = 0.6f;
    public float maxSpeed = 3f;

    void Start()
    {
        for (int i = 0; i < birdCount; i++)
        {
            Vector2 pos = Random.insideUnitCircle * spawnRadius;
            Vector2 vel = new Vector2(Random.Range(0, 1), Random.Range(0, 1));
            Vector3 pos3D = new Vector3(pos.x, pos.y, 0f);

            GameObject go = Instantiate(birdPrefab, pos3D, Quaternion.identity) as GameObject;
            go.transform.SetParent(transform);
            BirdAI bird = go.GetComponent<BirdAI>();
            bird.velocity = vel;

            birds.Add(go);
        }
    }
}
