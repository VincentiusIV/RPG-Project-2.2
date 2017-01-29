using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Map for an A* demo, template from blackboard
/// </summary>
public class Map : MonoBehaviour
{
    public Transform player, target;

    public LayerMask unwalkableLayer;
    public Vector2 gridWorldSize;

    // size of each node
    public float nodeRadius;
    private float nodeDiameter;

    // size of the map
    int gridSizeX, gridSizeY;

    // the default sie of the map. Can be changed in the inspector

    // the initial map
    AStarNode[,] grid = null;


    // Use this for initialization
    void Start()
    {
        // Centers the map instead of it drawing upwards
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        // generate a new map
        GenerateMap();
    }


    /// <summary>
    /// Generates the map.
    /// </summary>
    void GenerateMap()
    {
        grid = new AStarNode[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft = new Vector2(transform.position.x, transform.position.y) - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter) + Vector2.up * (y * nodeDiameter);
                bool walkable = !(Physics2D.CircleCast(worldPoint, nodeRadius, Vector2.zero, 1, unwalkableLayer));
                grid[x, y] = new AStarNode(walkable, worldPoint, x, y);
            }
        }
    }

    /// <summary>
    /// 	Gets the neighbors of a map position. Uses von Neumann neighborhood.
    /// 	Checks the map if a neighboring tile is walkable
    /// 	Checks if a neighboring tile is already 'investigated' by the algorithm.
    /// </summary>
    /// <returns>The neighbors.</returns>
    /// <param name="current">The node of which to search its neighbors</param>
    public List<AStarNode> GetNeighbors(AStarNode current)
    {
        List<AStarNode> neighbors = new List<AStarNode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 || y == 0)
                    continue;

                int checkX = current.gridX + x;
                int checkY = current.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    neighbors.Add(grid[checkX, checkY]);
            }
        }
        return neighbors;
    }

    public AStarNode NodeFromPosition(Vector3 worldPos)
    {
        //int x = Mathf.RoundToInt(worldPos.x - transform.position.x);
        //int y = Mathf.RoundToInt(worldPos.y - transform.position.y);
        float percentX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPos.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = (int)((gridSizeX) * percentX);
        int y = (int)((gridSizeY + 1) * percentY);
        Debug.Log("Node has X = " + x + " & Y = " + y);
        return grid[x, y];
    }

    // For debugging
    public List<AStarNode> path;
    void OnDrawGizmos()
    {
        Vector3 center = transform.position + Vector3.right * gridWorldSize.x/2 + Vector3.up * gridWorldSize.y/2;
        Gizmos.DrawWireCube(center, new Vector3(gridWorldSize.x, gridWorldSize.y, 0f));

        if(grid != null)
        {
            AStarNode playerNode = NodeFromPosition(player.position);
            AStarNode targetNode = NodeFromPosition(target.position);

            foreach (AStarNode node in grid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                if (path != null)
                    if (path.Contains(node))
                        Gizmos.color = Color.black;
                if (targetNode == node)
                    Gizmos.color = Color.green;
                if (playerNode == node)
                    Gizmos.color = Color.cyan;

                Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - .05f));
            }
        }
    }
}
