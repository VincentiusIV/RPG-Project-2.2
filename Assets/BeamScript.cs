using UnityEngine;
using System.Collections;

public class BeamScript : MonoBehaviour {

    public int gunDamage = 1;
    public float fireRate = .25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;
    public Transform aim;
    public LayerMask beamLayerMask;

    private Camera cam;
    private WaitForSeconds shotDuration = new WaitForSeconds(.07f);
    private AudioSource gunAudio;
    private LineRenderer beamLine;
    private float nextFire;

	// Use this for initialization
	void Start () {
        beamLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 end = new Vector2(aim.position.x, aim.position.y);
        Vector2 start = new Vector2(gunEnd.position.x, gunEnd.position.y);
        Debug.DrawLine(start, end, Color.green);
        if (Input.GetAxis("X360_Triggers") != 0 && Time.time > nextFire)
        {
            
            RaycastHit2D hit = Physics2D.Raycast(start, end, weaponRange, beamLayerMask);
            

            nextFire = Time.time + fireRate;

            StartCoroutine(ShotEffect());

            if(hit != null && hit.collider.CompareTag("AI"))
            {
                Debug.Log("Fire beam hitititit");
                beamLine.SetPosition(0, start);
                beamLine.SetPosition(1, hit.point);
            }
            else
            {
                Debug.Log("Fire beam nananana");
                beamLine.SetPosition(0, start);
                beamLine.SetPosition(1, start - end * weaponRange);
            }
        }
	}

    private IEnumerator ShotEffect()
    {
        gunAudio.Play();

        beamLine.enabled = true;
        yield return shotDuration;
        beamLine.enabled = false;
    }
}
