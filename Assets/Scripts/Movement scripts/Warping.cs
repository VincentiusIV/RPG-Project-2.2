using UnityEngine;
using System.Collections;

public class Warping : MonoBehaviour {

    [SerializeField]private Transform targetWarp;
    private ButtonFunctionality sf;

    void Start()
    {
        sf = GameObject.Find("UI").GetComponent<ButtonFunctionality>();
    }

    IEnumerator OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            yield return StartCoroutine(sf.FadeIn());
            col.transform.position = targetWarp.position;
            Camera.main.transform.position = targetWarp.position;
            yield return StartCoroutine(sf.FadeOut());
            
        }
    }
}
