using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class TileManager : MonoBehaviour
{
    public int columns = 8;
    public int rows = 8;

    public GameObject[] floorTiles;

    private List<Vector3> gridPositions = new List<Vector3>();

    void Start()
    {
        InitialiseList();
        SpawnTiles();
    }

    void InitialiseList()
    {
        gridPositions.Clear();

        float xPos = transform.position.x;
        float yPos = transform.position.y;

        for (float x = xPos; x < (xPos + columns); x++)
        {
            for (float y = yPos; y < (yPos + rows); y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));

            }
        }
    }

    private void SpawnTiles()
    {
        for (int i = 0; i < gridPositions.Count; i++)
        {
            GameObject go = floorTiles[Random.Range(0, floorTiles.Length)];
            Instantiate(go, gridPositions[i], Quaternion.identity);
            
            //go.transform.SetParent(gameObject.transform);
        }
    }
}
