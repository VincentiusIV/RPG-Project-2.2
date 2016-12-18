using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour
{
    public List<TerrainPrefab> terrainType = new List<TerrainPrefab>();
    private GameObject worldHolder;

    public int chunkHeight;
    public int chunkWidth;
    public int xChunkAmount;
    public int yChunkAmount;

    public float scale;
    public int AmountOfPerlinLayers;
    public float increasingAmplitudePerLayer;
    public float increasingFrequencyPerLayer;
    public float manualHeightAdjustment;

    private float computeTime = 0f;
    private bool generating = false;

    public void GenerateWorld()
    {
        //xChunkAmount = worldWidth / 10;
        //yChunkAmount = worldHeight / 10;

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
        worldHolder.name = "Chunk(" + row +"," + column + ")";

        for (int y = 0; y < chunkHeight; y++)
        {
            for (int x = 0; x < chunkWidth; x++)
            {
                float frequency = 1;
                float amplitude = 1;
                float height = 0;

                float xValue = x + row * chunkWidth;
                float yValue = y + column * chunkHeight;

                for (int i = 0; i < AmountOfPerlinLayers; i++)
                {
                    float perlin = Mathf.PerlinNoise( xValue / scale * frequency, yValue / scale * frequency) ;
                    height += perlin * amplitude;

                    amplitude *= increasingAmplitudePerLayer;
                    frequency *= increasingFrequencyPerLayer;
                }

                height -= manualHeightAdjustment;

                Vector2 position = new Vector2( xValue , yValue);

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
