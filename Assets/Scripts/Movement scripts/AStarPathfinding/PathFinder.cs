using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour {

    public Transform seeker, target;

    Map grid;

	// Use this for initialization
	void Awake () {
        grid = GetComponent<Map>();
	}
	
	// Update is called once per frame
	void Update () {
        FindPath(seeker.position, target.position);
	}

    public void FindPath(Vector3 start, Vector3 goal)
    {
        // set the indices in map to indicate start and goal
        AStarNode startNode = grid.NodeFromPosition(start);
        AStarNode targetNode = grid.NodeFromPosition(goal);

        // the open list, nodes still to be checked
        List<AStarNode> open = new List<AStarNode>();
        // the closed list, nodes that are checked already
        HashSet<AStarNode> closed = new HashSet<AStarNode>();

        // set 'A* variables' (G and F) on the start node
        // add start node to open list
        open.Add(startNode);

        // While loop: as long as there are options left to check (open is not empty)
        while (open.Count > 0)
        {
            AStarNode currentNode = open[0];

            for (int i = 0; i < open.Count; i++)
            {
                // take the best option off of open, save it in currentNode
                if (open[i].f < currentNode.f || open[i].f == currentNode.f && open[i].h < currentNode.h)
                {
                    currentNode = open[i];
                }
            }

            // remove the node from open
            open.Remove(currentNode);
            // add it to closed (never to be inspected again)
            closed.Add(currentNode);

            // if the best option is the goal, backtrack constructing the path (the early exit)
            // construct the path in a separate function to keep this loop small
            if (currentNode == targetNode)
            {
                Debug.Log("reconstructing path");
                ReconstructPath(startNode, targetNode);
                return;
            }

            // for each neighbour
            // get the neighbours of currentNode (in a separate function to keep this loop small)
            foreach (AStarNode neighbor in grid.GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closed.Contains(neighbor))
                    continue;
                // calculate tentative G 
                int newGToNeighbor = currentNode.g + GetDistance(currentNode, neighbor);

                // check if the neighbour is already in open (store in variable maybe)
                if (newGToNeighbor < neighbor.g || !open.Contains(neighbor))
                {
                    neighbor.g = newGToNeighbor;
                    neighbor.h = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!open.Contains(neighbor))
                    {
                        open.Add(neighbor);
                    }
                }
            }
        }
        // if we get here, there is no path. return empty list to satisfy the path drawing function
        //return new List<AStarNode>();
    }

    /// <summary>
    ///  Finds the path from the start to goal AStarNode.
    /// </summary>
    /// <returns>The path.</returns>
    /// <param name="start">The start AStarNode</param>
    /// <param name="goal">the goal AStarNode</param>
    int GetDistance(AStarNode a, AStarNode b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
    /// <summary>
    /// Inserts the AstarNode into sorted list.
    /// </summary>
    /// <param name="neighbor">Neighbor.</param>

    void ReconstructPath(AStarNode startNode, AStarNode endNode)
    {
        List<AStarNode> newPath = new List<AStarNode>();
        AStarNode currentNode = endNode;

        while (currentNode != startNode)
        {
            newPath.Add(currentNode);
            currentNode = currentNode.parent;
        }
        newPath.Reverse();
        grid.path = newPath;
    }
}
