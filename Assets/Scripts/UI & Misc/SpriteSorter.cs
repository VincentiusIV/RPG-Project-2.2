using UnityEngine;
using System.Collections;
using Tiled2Unity;

// Author: Vincent Versnel
// Simple script that sorts sprites on the order based on y position
// Wrote this when I did not understand how layers worked, and is not really used in game
public class SpriteSorter : MonoBehaviour {

    public bool takeParent;

    private SpriteRenderer sr;
    private Transform player;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

    }
	// Use this for initialization
	void Update () {
        if (!takeParent)
            sr.sortingOrder = (int)(-transform.position.y * 1000);
        else
            sr.sortingOrder = (int)(-transform.parent.position.y * 1000);


	}
}
