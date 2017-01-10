using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GeneratingDungeon : MonoBehaviour {
    [SerializeField] private int lvlBoundWidth;
    [SerializeField] private int lvlBoundHeight;
    [SerializeField] private Vector2 MinAndMaxTrees;
    [SerializeField] private Vector2 MinAndMaxCamps;
    [SerializeField] private int CampWidthHalf;
    [SerializeField] private int CampHeigthHalf;
    [SerializeField] private int amountOfItemsInCamp;
    [SerializeField] private GameObject[] groundBlockPrefabs;
    [SerializeField] private GameObject[] itemBlockPrefabs;
    [SerializeField] private GameObject[] wallBlockPrefabs;
    [SerializeField] private GameObject lvlParent;
    private GameObject usedPrefab;
    private Vector2 blockPosition;
    private int positionCounter = 0;
    private int yCounter = 0;
    private int[] maze;
    private int lvlHeightCounter = 1;
    private Vector2 NorthExit;
    private Vector2 SouthExit;
    private Vector2 EastExit;
    private Vector2 WestExit;
    public List<Vector2> insideLvl = new List<Vector2>();

    void Start(){
        GenerateLevel();
    }

    public void GenerateLevel() {
        BuildLevelArray();
        LoadInsideLevel();
        BuildLvlFromArray();
    }

    public void DestroyLevel() {
        for (int i = 0; i < lvlParent.transform.childCount; i++){
            Destroy(lvlParent.transform.GetChild(i).gameObject);
        }
        positionCounter = 0;
        yCounter = 0;
        lvlHeightCounter = 1;
    }

    void BuildLevelArray() {
        int lvlBoundSize = lvlBoundWidth * lvlBoundHeight;
        maze = new int[lvlBoundSize];
        for (int i = 0; i < lvlBoundSize; i++){
            if (i == (lvlBoundWidth * lvlHeightCounter)) {
                lvlHeightCounter++;
            }
            if (i <= lvlBoundWidth - 1 || i >= (lvlBoundSize - lvlBoundWidth)) {
                maze[i] = 1;
            } else if (i == ((lvlBoundWidth * lvlHeightCounter) - lvlBoundWidth)) {
                maze[i] = 2;
            } else if (i == (lvlBoundWidth * lvlHeightCounter) - 1) {
                maze[i] = 3;
            }
            else {
                maze[i] = 0;
            }
        }
    }

    void LoadInsideLevel() {
        //make four exits and ?paths to them?, make entrances to dungeons, random trees etc. 
        CreatingExits();

        int amountOfTrees = Random.Range(Mathf.FloorToInt(MinAndMaxTrees.x), Mathf.FloorToInt(MinAndMaxTrees.y));
        for (int i = 0; i < amountOfTrees + 1; i++){
            int random = Random.Range(2, lvlBoundHeight - 1);
            maze[(Random.Range(((lvlBoundWidth * random) - lvlBoundWidth) + 1, (lvlBoundWidth * random) - 1))] = 4;
        }
        int amountOfCamps = Random.Range(Mathf.FloorToInt(MinAndMaxCamps.x), (Mathf.FloorToInt(MinAndMaxCamps.y)) + 1);
        for (int i = 0; i < amountOfCamps; i++){
            CreateCamp();
        }
    }

    void CreateCamp() {
        int random = Random.Range(CampHeigthHalf * 2, lvlBoundHeight - (CampHeigthHalf * 2));
        int mazePos = (Random.Range(((lvlBoundWidth * random) - lvlBoundWidth) + (CampWidthHalf * 2), (lvlBoundWidth * random) - (CampWidthHalf * 2)));
        for (int i = 0; i < amountOfItemsInCamp; i++){
            maze[(Random.Range(mazePos - CampWidthHalf, mazePos + CampWidthHalf)) + (lvlBoundWidth * Random.Range(1, CampHeigthHalf))] = Random.Range(5, 8);
        }
    }

    void CreatingExits(){
        int lvlOpeningNorth = Random.Range(2, lvlBoundWidth - 2);
        int lvlOpeningSouth = Random.Range((lvlBoundWidth * lvlBoundHeight) - lvlBoundWidth + 2, (lvlBoundWidth * lvlBoundHeight) - 2);
        int lvlOpeningEast = Random.Range(2, lvlBoundHeight - 2) * lvlBoundWidth;
        int lvlOpeningWest = Random.Range(2, lvlBoundHeight - 2) * lvlBoundWidth - 1;

        maze[lvlOpeningNorth] = -1;
        maze[lvlOpeningNorth + 1] = -1;
        maze[lvlOpeningNorth - 1] = -1;

        maze[lvlOpeningSouth] = -1;
        maze[lvlOpeningSouth + 1] = -1;
        maze[lvlOpeningSouth - 1] = -1;

        maze[lvlOpeningEast] = -1;
        maze[lvlOpeningEast + lvlBoundWidth] = -1;
        maze[lvlOpeningEast - lvlBoundWidth] = -1;

        maze[lvlOpeningWest] = -1;
        maze[lvlOpeningWest + lvlBoundWidth] = -1;
        maze[lvlOpeningWest - lvlBoundWidth] = -1;
    }

    void BuildLvlFromArray(){
        for (int i = 0; i < maze.Length; i++){
            positionCounter++;
            blockPosition = new Vector2((1.33f * positionCounter) - 30, (1.33f * yCounter) + 10);
            if (positionCounter == lvlBoundWidth){
                positionCounter = 0;
                yCounter--;
            }
            if (maze[i] == 7){
                GameObject grassBlockClone = (GameObject)Instantiate(groundBlockPrefabs[0], blockPosition, Quaternion.identity);
                grassBlockClone.transform.parent = lvlParent.transform;
                usedPrefab = itemBlockPrefabs[1];
            }
            if (maze[i] == 6) {
                GameObject grassBlockClone = (GameObject)Instantiate(groundBlockPrefabs[0], blockPosition, Quaternion.identity);
                grassBlockClone.transform.parent = lvlParent.transform;
                usedPrefab = itemBlockPrefabs[0];
            }
            if (maze[i] == 5){
                GameObject grassBlockClone = (GameObject)Instantiate(groundBlockPrefabs[0], blockPosition, Quaternion.identity);
                grassBlockClone.transform.parent = lvlParent.transform;
                usedPrefab = groundBlockPrefabs[3];
            }
            if (maze[i] == 4) {
                usedPrefab = groundBlockPrefabs[0];
                GameObject grassClone = (GameObject)Instantiate(groundBlockPrefabs[1], blockPosition, Quaternion.identity);
                grassClone.transform.parent = lvlParent.transform;
            }
            else if (maze[i] == 1 || maze[i] == 2 || maze[i] == 3) {
                usedPrefab = wallBlockPrefabs[maze[i] - 1];
            }
            else if (maze[i] == -1) {
                usedPrefab = groundBlockPrefabs[2];
            } else if (maze[i] == 0) {
                usedPrefab = groundBlockPrefabs[0];
            }
            if (maze[i] != 1 && maze[i] != 2 && maze[i] != 3) {
                insideLvl.Add(blockPosition);
            }
            GameObject blockClone = (GameObject)Instantiate(usedPrefab, blockPosition, Quaternion.identity);
            blockClone.transform.parent = lvlParent.transform;
            blockClone.name = "Block" + i;
        }
    }
}
