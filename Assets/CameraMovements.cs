using UnityEngine;
using System.Collections;

public class CameraMovements : MonoBehaviour {

    public GameObject target;
    public float camDistance;
    public float lerpIntensity;

    private Camera cam;

	void Start () {
        cam = GetComponent<Camera>();
	}
	
	void Update () {
        FollowPlayer();
	}

    void FollowPlayer() {
        Vector3 targetPosition = target.transform.position + new Vector3(0f, 0f, -camDistance);
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpIntensity);
        //transform.LookAt(target.transform);
    }
}
