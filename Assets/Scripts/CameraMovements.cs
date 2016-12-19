using UnityEngine;
using System.Collections;

public class CameraMovements : MonoBehaviour{
    [SerializeField]
    private Vector2 speed;
    public GameObject target;
    public float camDistance;
    public float lerpIntensity;

    private Camera cam;
    private Vector2 movement;

    void Start(){
        cam = GetComponent<Camera>();
    }

    void Update(){
        //FollowPlayer();
    }

    void FixedUpdate(){
        float movX = Input.GetAxis("Horizontal");
        float movY = Input.GetAxis("Vertical");
        movement = new Vector2(speed.x * movX, speed.y * movY);
        movement *= Time.deltaTime;
        //if (canPlay){
        transform.Translate(movement);
        // }
        //else{
        // return;
        // }
    }

    void FollowPlayer() { 
        Vector3 targetPosition = target.transform.position + new Vector3(0f, 0f, -camDistance);

        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpIntensity);
        //transform.LookAt(target.transform);
    }
}
