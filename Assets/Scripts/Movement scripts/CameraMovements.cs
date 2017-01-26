using UnityEngine;
using System.Collections;

public class CameraMovements : MonoBehaviour
{
    [SerializeField]private GameObject minimap;
    [SerializeField]private GameObject playerTarget;
    private GameObject target;
    public float camDistance;
    public float lerpIntensity;
    public float pixelToUnits;
    private Vector2 movement;

    void Start()
    {
        //GetComponent<Camera>().orthographicSize = (Screen.height / pixelToUnits) / 2;
        target = playerTarget;
    }
    void FixedUpdate()
    {
        Vector3 targetPosition = target.transform.position + new Vector3(0f, 0f, -camDistance);
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpIntensity);
    }

    void Update()
    {
        minimap.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, minimap.transform.position.z);
    }

    public void SetTarget(bool setPlayer, GameObject newTarget = null)
    {
        if (setPlayer)
            target = playerTarget;
        else
            target = newTarget;
    }
}
