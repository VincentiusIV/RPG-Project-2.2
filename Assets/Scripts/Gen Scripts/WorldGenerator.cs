using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour
{
    public List<TerrainPrefab> terrainType = new List<TerrainPrefab>();
    public GameObject tileGO;
    public SpriteArrays spriteArrays;
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
                        GameObject tile = Instantiate(tileGO, position, Quaternion.identity) as GameObject;
                        tile.GetComponent<SpriteRenderer>().sprite = PickRandomSprite(terrainType[i].spriteType);
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

    Sprite PickRandomSprite(SpriteType type)
    {
        switch(type)
        {
            case SpriteType.grass:
                return spriteArrays.grassTiles[(int)Random.Range(0, spriteArrays.grassTiles.Length)];
            case SpriteType.rock:
                return spriteArrays.rockTiles[(int)Random.Range(0, spriteArrays.rockTiles.Length)];
            case SpriteType.snow:
                return spriteArrays.snowTiles[(int)Random.Range(0, spriteArrays.snowTiles.Length)];
            case SpriteType.water:
                return spriteArrays.waterTiles[(int)Random.Range(0, spriteArrays.waterTiles.Length)];
        }
        return new Sprite();
    }
}

[System.Serializable]
public struct TerrainPrefab
{
    public string name;
    public SpriteType spriteType;
    public float height;
}

[System.Serializable]
public struct SpriteArrays
{
    public Sprite[] snowTiles;
    public Sprite[] grassTiles;
    public Sprite[] rockTiles;
    public Sprite[] waterTiles;
}

public enum SpriteType
{
    snow, grass, rock, water
}
