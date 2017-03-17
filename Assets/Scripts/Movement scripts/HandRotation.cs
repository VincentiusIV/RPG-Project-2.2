using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Author: Vincent Versnel
// rotates the hand around the shoulder of the player character based on where he is aiming
public class HandRotation : MonoBehaviour {

    private Transform aim;
    public float rotationOffset;
    public Sprite[] bodySprites;
    public Sprite[] armSprite;

    public bool isAiming;

    private SpriteRenderer thisSr;
    private SpriteRenderer playerSr;

    void Awake()
    {
        aim = transform.parent.FindChild("Aim");
        thisSr = GetComponent<SpriteRenderer>();
        playerSr = transform.parent.GetComponent<SpriteRenderer>();
    }
	// Update is called once per frame
	void Update ()
    {
        Vector3 direction = aim.position - transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x); // rotation angle in degrees
        float degrees = (180 / Mathf.PI) * angle;
        transform.rotation = Quaternion.Euler(0f, 0f, degrees + rotationOffset);

        // switches sprites based on where player is looking
        if (isAiming)
        {
            if (aim.localPosition.x > 0)
                thisSr.sprite = armSprite[0];
            else if (aim.localPosition.x < 0)
                thisSr.sprite = armSprite[1];
            // right
            if (aim.localPosition.x > 0 && aim.localPosition.y < .5f && aim.localPosition.y > -.5f)
                playerSr.sprite = bodySprites[0];

            //left 
            if (aim.localPosition.x < 0 && aim.localPosition.y < .5f && aim.localPosition.y > -.5f)
                playerSr.sprite = bodySprites[1];

            // up
            if (aim.localPosition.y > 0 && aim.localPosition.x < .5f && aim.localPosition.x > -.5f)
                playerSr.sprite = bodySprites[2];

            // down
            if (aim.localPosition.y < 0 && aim.localPosition.x < .5f && aim.localPosition.x > -.5f)
                playerSr.sprite = bodySprites[3];
        }
        else
            thisSr.sprite = new Sprite();
	}


}
