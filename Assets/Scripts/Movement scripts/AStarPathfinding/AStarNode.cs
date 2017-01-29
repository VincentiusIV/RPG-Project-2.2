using UnityEngine;

/// <summary>
///  Node for the A* algorithm.
/// </summary>
public class AStarNode
{
    // position in the map
    public Vector2 worldPos;
    // whether or not this node is walkable
    public bool walkable;
	// the parent node of this position (the step in the path before this one)
	public AStarNode parent = null;

    public int gridX, gridY;

	// the A* numbers
	public int h, g;

    public int f
    {
        get { return g + h; }
    }
        
	/// <summary>
	/// Initializes a new instance of the <see cref="AStarNode"/> class.
	/// </summary>
	/// <param name="_position">The position in the map this node represents</param>
	/// <param name="_f">The sum of g and the estimate to get to the goal node from this node</param>
	/// <param name="_g">The cost to get to this node from the start of the path</param>
	public AStarNode( bool _walkable, Vector2 _worldPos, int _gridX, int _gridY)
	{
        walkable = _walkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
	}
}