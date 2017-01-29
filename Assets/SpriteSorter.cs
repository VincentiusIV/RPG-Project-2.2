using UnityEngine;
using System.Collections;

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
