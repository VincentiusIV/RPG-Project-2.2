using UnityEngine;
using System.Collections;
// Author: Vincent Versnel
// Script to trigger when the player is in range
// needs to be in a seperate script from AIMovement so the trigger colliders can be seperated
public class AICombat : MonoBehaviour
{
    AIMovement aiMovement;

    void Awake()
    {
        aiMovement = transform.parent.FindChild("AIMovement").GetComponent<AIMovement>();
    }

	void OnTriggerStay2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            aiMovement.inRange = true;

            if(aiMovement.isFollowPlayerRunning)
            {
                aiMovement.StopCoroutine(aiMovement.followPlayer);
                aiMovement.followPlayer = aiMovement.FollowPlayer();
                aiMovement.isFollowPlayerRunning = false;
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            aiMovement.inRange = false;

            if (!aiMovement.isFollowPlayerRunning)
            {
                aiMovement.StartCoroutine(aiMovement.followPlayer);
            }
        }
    }
}
