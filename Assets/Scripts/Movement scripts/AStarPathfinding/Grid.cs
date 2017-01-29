using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Grid : MonoBehaviour {

    public Transform player, target;
    public LayerMask unwalkableLayer;
    public Vector2 gridWorldSize;
    // space each node covers
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        // gives the amount of nodes we can fit inside worldsize
        gridSizeX = (int)(gridWorldSize.x / nodeDiameter);
        gridSizeY = (int)(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = new Vector3(transform.position.x, transform.position.y, 0f) + Vector3.right * (x * nodeDiameter) + Vector3.up * (y * nodeDiameter);
                bool walkable = !(Physics2D.CircleCast(worldPoint, nodeRadius, Vector2.one, 1, unwalkableLayer));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int  x = -1;  x <= 1;  x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }

        return neighbours;
    }


    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        /*float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = (int)((gridSizeX) * percentX);
        int y = (int)((gridSizeY + 1) * percentY);
        Debug.Log("Node has X = " + x + " & Y = " + y);
        */
        int x = Mathf.RoundToInt(worldPos.x - transform.position.x);
        int y = Mathf.RoundToInt(worldPos.y - transform.position.y);
        return grid[x, y];
    }

    public List<Node> path;
    void OnDrawGizmos()
    {
        Vector3 center = transform.position + Vector3.right * gridWorldSize.x / 2 + Vector3.up  * gridWorldSize.y / 2 + -Vector3.one * nodeRadius;
        Gizmos.DrawWireCube(center, new Vector3(gridWorldSize.x, gridWorldSize.y,0));

        if (grid != null)
        {
            Node playerNode = NodeFromWorldPoint(player.position);
            Node targetNode = NodeFromWorldPoint(target.position);

            foreach (Node node in grid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                if (path != null)
                    if (path.Contains(node))
                        Gizmos.color = Color.black;
                if (targetNode == node)
                    Gizmos.color = Color.green;
                if (playerNode == node)
                    Gizmos.color = Color.cyan;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
            
                
    }
}
