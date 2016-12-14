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
    
    public void GenerateWorld()
    {
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
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
                    float perlin = Mathf.PerlinNoise(x / scale * frequency, y / scale * frequency);
                    height += perlin * amplitude;

                    amplitude *= increasingAmplitudePerLayer;
                    frequency *= increasingFrequencyPerLayer;
                }

                height -= 0.3f;
                Debug.Log(height);
                Vector2 position = new Vector2(x, y);

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
    }
}

[System.Serializable]
public struct TerrainPrefab
{
    public string name;
    public float height;
    public GameObject go;
}
