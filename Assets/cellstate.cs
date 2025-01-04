using UnityEngine;

public class CellState : MonoBehaviour
{
    public int x;  // X-coordinate in the maze grid
    public int y;  // Y-coordinate in the maze grid
    public float reward = 0f;  // Initial reward value, will be modified later
    public bool isWalkable = true; // Indicates if this cell is walkable (no walls)
}
