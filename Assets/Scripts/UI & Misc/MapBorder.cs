using UnityEngine;
using System.Collections;

// Spawns a border around a map
public class MapBorder : MonoBehaviour {

    [SerializeField]private GameObject border;
    [SerializeField]private int rows;
    [SerializeField]private int columns;
    void Start ()
    {
        for (int y = 0; y < rows; y++)
            for (int x = 0; x < columns; x++)
            {
                if (x == 0 || x == columns -1 || y == 0 || y == rows -1)
                {
                    float xPos = transform.position.x + x;
                    float yPos = transform.position.y + y;
                    GameObject go = Instantiate(border, new Vector3(xPos, yPos, 0f), Quaternion.identity) as GameObject;
                    go.transform.SetParent(transform);
                }   
            }
	}
}
