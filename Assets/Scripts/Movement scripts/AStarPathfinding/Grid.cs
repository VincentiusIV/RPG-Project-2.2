using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Author: Vincent Versnel
// Creates a grid for pathfinding
public class Grid : MonoBehaviour {

    public LayerMask blockingLayer;
    public Vector2 gridWorldSize;
    // space each node covers
    public float nodeRadius;
    public Node[,] nodeGrid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        // gives the amount of nodes we can fit inside worldsize
        gridSizeX = (int)(gridWorldSize.x / nodeDiameter);
        gridSizeY = (int)(gridWorldSize.y / nodeDiameter);
        GenerateMap();
    }
    // Generates the grid map, the first step
    void GenerateMap()
    {
        nodeGrid = new Node[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = new Vector3(transform.position.x, transform.position.y, 0f) + Vector3.right * (x * nodeDiameter) + Vector3.up * (y * nodeDiameter);
                //bool walkable = !(Physics2D.CircleCast(worldPoint, nodeRadius, Vector2.one, 1, unwalkableLayer));
                // if raycast did not detect the blocking layer, it remains walkable
                bool walkable = true;
                if (Physics2D.BoxCast(worldPoint, Vector2.one * .5f, 0, Vector2.zero, 1f, blockingLayer))
                    walkable = false;

                nodeGrid[x, y] = new Node(walkable, worldPoint, x, y);
            }
    }
    /* Gets the neighbors of the given node. 
      * Looks at all 8 neighbours for diagonal distance &
     *  checks if the neighbour is walkable
     * */
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int  x = -1;  x <= 1;  x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.x + x;
                int checkY = node.y + y;

                // does it exist in the grid?
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    if(nodeGrid[checkX, checkY].walkable)
                        neighbours.Add(nodeGrid[checkX, checkY]);
            }

        return neighbours;
    }


    /* Converts a given world position to the corresponding node
     * rounds off the world position to an integer
     * */
    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x - transform.position.x);
        int y = Mathf.RoundToInt(worldPos.y - transform.position.y);
        //Debug.Log("x " + x + " y " + y);
        return nodeGrid[x, y];
    }
}
