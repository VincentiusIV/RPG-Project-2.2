using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour
{
    public List<TerrainPrefab> terrainType = new List<TerrainPrefab>();
    private GameObject worldHolder;

    public int worldHeight;
    public int worldWidth;

    public float scale;
    public int AmountOfPerlinLayers;
    public float increasingAmplitudePerLayer;
    public float increasingFrequencyPerLayer;

    private float computeTime = 0f;
    private bool generating = false;

    public void GenerateWorld()
    {
        int xChunkAmount = worldWidth / 10;
        int yChunkAmount = worldHeight / 10;

        for (int y = 0; y < yChunkAmount; y++)
        {
            for (int x = 0; x < xChunkAmount; x++)
            {
                StartCoroutine(GenerateChunk(x, y));
            }
        }
    }

    void Update()
    {
        if (generating)
            computeTime += Time.deltaTime;
    }

    IEnumerator GenerateChunk(float row, float column)
    {
        generating = true;

        worldHolder = new GameObject();

        Instantiate(worldHolder, new Vector3(0f, 0f, 0f), Quaternion.identity);
        // create noise
        Debug.Log(worldHolder.transform.childCount);
        for (int i = 0; i < worldHolder.transform.childCount; i++)
        {
            DestroyImmediate(worldHolder.transform.GetChild(i).gameObject);
            Debug.Log("Destroyed");
        }

        for (int y = 0; y < worldHeight; y++)
        {
            for (int x = 0; x < worldWidth; x++)
            {
                float frequency = 1;
                float amplitude = 1;

                float height = 0;

                for (int i = 0; i < AmountOfPerlinLayers; i++)
                {
                    float perlin = Mathf.PerlinNoise((x + row * worldWidth) / scale * frequency, (y + column * worldHeight) / scale * frequency) ;
                    height += perlin * amplitude;

                    amplitude *= increasingAmplitudePerLayer;
                    frequency *= increasingFrequencyPerLayer;
                }

                height -= 0.35f;
                Debug.Log(height);

                Vector2 position = new Vector2(x + (row * worldWidth) , y + (column * worldHeight));

                for (int i = 0; i < terrainType.Count; i++)
                {
                    if (height <= terrainType[i].height)
                    {
                        Debug.Log(height);
                        GameObject tile = Instantiate(terrainType[i].go, position, Quaternion.identity) as GameObject;
                        tile.transform.SetParent(worldHolder.transform);
                        break;
                    }
                }
                
                yield return new WaitForSeconds(0f);
            }
            
        }

        Debug.Log("World Generation Finished and took: " + computeTime + " seconds");
        generating = false;
        computeTime = 0f;
        
    }
}

[System.Serializable]
public struct TerrainPrefab
{
    public string name;
    public float height;
    public GameObject go;
}
