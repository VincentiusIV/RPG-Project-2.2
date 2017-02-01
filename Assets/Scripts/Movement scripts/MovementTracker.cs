using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementTracker : MonoBehaviour {

    public GameObject tickObject;
    public GameObject target;
    public float tickFrequency;
    public int amountOfTicks;
    public bool track;
    public Vector2 spawnOffset;

    // Storing positions
    public List<Vector2> trackedPositions;
    public AudioSource stepSound;

    void Start()
    {
        stepSound = GetComponent<AudioSource>();

        if(target == null)
            target = this.gameObject;

        track = true;
        trackedPositions.Add(target.transform.position);
        StartCoroutine(Tracking());
    }
	
    IEnumerator Tracking()
    {
        while(track)
        {
            trackedPositions.Add(target.transform.position);
            if (trackedPositions.Count > amountOfTicks)
                trackedPositions.RemoveAt(0);

            GameObject tick = Instantiate(tickObject, new Vector2(target.transform.position.x, target.transform.position.y) + spawnOffset, Quaternion.Euler(0f, 0f, 90f)) as GameObject;
            Destroy(tick, 3f);
            yield return new WaitForSeconds(tickFrequency);
        }
    }
}
