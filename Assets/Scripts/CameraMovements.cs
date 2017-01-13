using UnityEngine;
using System.Collections;

public class CameraMovements : MonoBehaviour
{
    [SerializeField]private GameObject minimap;

    public GameObject target;
    public float camDistance;
    public float lerpIntensity;

    private Vector2 movement;

    void FixedUpdate()
    {
        FollowPlayer();
    }

    void Update()
    {
        minimap.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, minimap.transform.position.z);
    }

    void FollowPlayer()
    { 
        Vector3 targetPosition = target.transform.position + new Vector3(0f, 0f, -camDistance);

        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpIntensity);
    }
}
