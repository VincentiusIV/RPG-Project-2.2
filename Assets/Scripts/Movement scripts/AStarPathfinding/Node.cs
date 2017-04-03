using UnityEngine;
using System.Collections;
// Author: Vincent Versnel
/// <summary>
///  Node for the A* algorithm.
/// </summary>
public class Node {

    // position in the map
    public Vector2 worldPosition;
    // whether or not this node is walkable
    public bool walkable;
    // the parent node of this position (the step in the path before this one)
    public Node parent = null;
    // h is the estimated distance towards the end node
    public int h;
    // g is the cost of going from start to this node
    public int g;
    // f cost only needs to be retrieved and is always the sum of g and h
    public int f
    {
        get { return g + h; }
    }
    // position in the grid
    public int x, y;

    public Node(bool _walkable, Vector2 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        x = _gridX;
        y = _gridY;
    }
}
