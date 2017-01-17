using UnityEngine;
using System.Collections;

public class CameraMovements : MonoBehaviour
{
    [SerializeField]private GameObject minimap;

    public GameObject target;
    public float camDistance;
    public float lerpIntensity;
    public float pixelToUnits;
    private Vector2 movement;

    void Start()
    {
        GetComponent<Camera>().orthographicSize = (Screen.height / pixelToUnits) / 2;
    }
    void FixedUpdate()
    {
        //Vector3 targetPosition = new Vector3(RoundToNearestPixel(target.transform.position.x), RoundToNearestPixel(target.transform.position.y), 0f )+ new Vector3(0f, 0f, -camDistance);
        Vector3 targetPosition = target.transform.position + new Vector3(0f, 0f, -camDistance);
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpIntensity);
    }

    void Update()
    {
        minimap.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, minimap.transform.position.z);
    }

    public float RoundToNearestPixel(float unityUnits)
    {
        float valueInPixels = unityUnits * pixelToUnits;
        valueInPixels = Mathf.Round(valueInPixels);
        float roundedUnityUnits = valueInPixels * (1 / pixelToUnits);
        return roundedUnityUnits;
    }
}
