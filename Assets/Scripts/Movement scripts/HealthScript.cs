using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthScript : MonoBehaviour {
    [SerializeField] private bool isAI;
    [SerializeField] private float maxHP;
    [SerializeField] public float dmg;
    private SpriteRenderer hPBar;
    [SerializeField] private float hP;
    private bool isDead = false;
	
	void Start () {
        hP = maxHP;
        hPBar = GetComponent<SpriteRenderer>();
    }
	
	void Update () {
        hPBar.color = Color.Lerp(Color.red, Color.green, hP/100);
        if (hP < 100 && hP > 0) {
            hP += 1/60f;
        }
        if (hP <= 0) {
            isDead = true;
            hPBar.color = Color.red;
        }
        if (isDead && isAI){
            //Depending on isAI: disableControls, dying animation, respawning etc.
            Destroy(gameObject);
        }
        else {

        }
	}

    public void doDMG(float damage) {
        hP -= damage;
    }
}
