using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarPathfinding : MonoBehaviour {

    public int maxAmountOfPaths;
    public int AmountOfPathsBeingCalculated;

    Grid grid;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        if(AmountOfPathsBeingCalculated >= maxAmountOfPaths)
        {
            Debug.LogError("Too many paths are being calculated: "+AmountOfPathsBeingCalculated);
            return new List<Node>();
        }
        AmountOfPathsBeingCalculated += 1;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if(currentNode == targetNode)
            {
                AmountOfPathsBeingCalculated -= 1;
                return RetracePath(startNode, currentNode);
            }

            foreach(Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                int newMoveCostG = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMoveCostG < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMoveCostG;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        Debug.LogError("Could not find path");
        AmountOfPathsBeingCalculated -= 1;
        return new List<Node>();
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        // If two objects are already on the same node
        if(startNode == endNode)
        {
            List<Node> neighbors = grid.GetNeighbours(startNode);

            int index = Random.Range(0, neighbors.Count);
            path.Add(grid.nodeGrid[neighbors[index].gridX, neighbors[index].gridY]);
            return path;
        }
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;            
        }

        path.Reverse();
        grid.path = path;
        return path;
    }

    int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
