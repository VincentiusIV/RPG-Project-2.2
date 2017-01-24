﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementTracker : MonoBehaviour {

    public GameObject tickObject;
    public GameObject target;
    public float tickFrequency;
    public int amountOfTicks;
    public bool track;

    // Storing positions
    public List<Node> trackedPositions;

    void Start()
    {
        if(target == null)
            target = this.gameObject;

        track = true;
        trackedPositions.Add(new Node(0, target.transform.position));
        StartCoroutine(Tracking());
    }
	
    IEnumerator Tracking()
    {
        while(track)
        {
            Node nodeToAdd = new Node(trackedPositions.Count, target.transform.position);
            trackedPositions.Add(nodeToAdd);
            if (trackedPositions.Count > amountOfTicks)
                trackedPositions.RemoveAt(0);

            Instantiate(tickObject, target.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(tickFrequency);
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
[System.Serializable]
public class Node
{
    public Vector2 position;
    private int count;

    public Node(int count, Vector2 position)
    {
        this.count = count;
        this.position = position;
    }
}
