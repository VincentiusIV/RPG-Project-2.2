using UnityEngine;
using System.Collections;
// Author: Vincent Versnel
// Warps the player from one position to another
public class Warping : MonoBehaviour {

    [SerializeField]private Transform airshipWarp;
    [SerializeField]
    private Transform worldWarp;

    public bool AirshipOrWorld;

    private ButtonFunctionality sf;
    private GameController gc;

    void Start()
    {
        sf = GameObject.Find("UI").GetComponent<ButtonFunctionality>();
        gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    // fades in on enter, sets the player still,
    // warps the player, fades out, player can play again
    IEnumerator OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            yield return StartCoroutine(sf.FadeIn());

            if(AirshipOrWorld)
            {
                col.transform.position = airshipWarp.position;
                gc.SetAmbience(0);
                col.GetComponent<SpriteRenderer>().sortingOrder = 4;
                col.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
                Camera.main.transform.position = airshipWarp.position;
            }
            else if(!AirshipOrWorld)
            {
                col.transform.position = worldWarp.position;
                gc.SetAmbience(1);
                col.GetComponent<SpriteRenderer>().sortingOrder = 19;
                col.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 20;
                Camera.main.transform.position = worldWarp.position;
            }
            
            
            
            yield return StartCoroutine(sf.FadeOut());
            
        }
    }

}
