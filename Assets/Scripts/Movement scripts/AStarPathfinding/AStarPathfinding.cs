using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// Author: Vincent Versnel
/// <summary>
/// Map for an A* demo, template from blackboard
/// the location of many functions have been moved around because 
/// originally the placement did not make sense to me
/// </summary>
public class AStarPathfinding : MonoBehaviour {

    public int maxAmountOfPaths;
    public int AmountOfPathsBeingCalculated;

    Grid grid;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }
    /* Returns a list of A* nodes which hold world positions for the path that can be traversed
     * creates a path between start and goal vector3
     * */
    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        if(AmountOfPathsBeingCalculated >= maxAmountOfPaths)
        {
            Debug.LogError("Too many paths are being calculated: "+AmountOfPathsBeingCalculated);
            return new List<Node>();
        }
        AmountOfPathsBeingCalculated += 1;

        // vector3 conversion to nodes
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        // the open list, nodes still to be checked
        List<Node> open = new List<Node>();
        // the closed list, nodes that are checked already
        List<Node> closed = new List<Node>();
        // add start node to open list
        open.Add(startNode);
        // While loop: as long as there are options left to check (open is not empty)
        while (open.Count > 0)
        {
            // add the first node to current so we have something to check at start
            Node currentNode = open[0];

            // take the cheapest node off open
            for (int i = 0; i < open.Count; i++)
                if(open[i].f < currentNode.f)
                    currentNode = open[i];

            // Add current to CLOSED
            open.Remove(currentNode);
            closed.Add(currentNode);

            // if the best option is the goal, backtrack constructing the path (the early exit)
            if (currentNode == targetNode)
            {
                AmountOfPathsBeingCalculated -= 1;
                return ReconstructPath(startNode, currentNode);
            }

            // get the neighbours of currentNode 
            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (closed.Contains(neighbour))
                    continue;

                // cost = current g cost + movementcost(current, neighbor)
                int tentative_gCost = currentNode.g + GetDistance(currentNode, neighbour);

                // add neighbor to open
                if (!open.Contains(neighbour))
                    open.Add(neighbour);
                else if (tentative_gCost >= neighbour.g)
                    continue; // this means the the neighbor is not a better path

                // set neighbors parent to current
                neighbour.parent = currentNode;
                // set the new g cost
                neighbour.g = tentative_gCost;
                // set the h cost
                neighbour.h = GetDistance(neighbour, targetNode);
            }
        }
        Debug.LogError("Could not find path");
        AmountOfPathsBeingCalculated -= 1;
        return new List<Node>();
    }
    // reconstructs the path from end to start node using the parents
    List<Node> ReconstructPath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        // If two objects are already on the same node
        if(startNode == endNode)
        {
            List<Node> neighbors = grid.GetNeighbours(startNode);

            int index = Random.Range(0, neighbors.Count);
            path.Add(grid.nodeGrid[neighbors[index].x, neighbors[index].y]);
            return path;
        }
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;            
        }

        path.Reverse();
        return path;
    }

    // Gets the diagonal distance from point a to point b
    int GetDistance(Node a, Node b)
    {
        int dX = Mathf.Abs((int)a.x - (int)b.x);
        int dY = Mathf.Abs((int)a.y - (int)b.y);

        return 10 * (dX + dY) + (14 - 2 * 10) * Mathf.Min(dX, dY);
    }
}
