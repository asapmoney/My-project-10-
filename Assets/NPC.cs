using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NPCController : MonoBehaviour
{
    public float speed = 3f;
    public float nodeSize = 1f; // Size of each cell (in Unity units)
    public Vector3 startPosition = new Vector3(0, 1, 0); // Starting position
    public Vector3 targetPosition = new Vector3(5, 1, 5); // Desired target position
    private List<Vector3> path = new List<Vector3>(); // Path to follow

    // Directions for A* movement (up, down, left, right)
    private Vector3[] directions = new Vector3[] {
        Vector3.forward, Vector3.back, Vector3.left, Vector3.right
    };

    private bool pathCalculated = false;

    void Start()
    {
        // Calculate the path when the game starts
        path = FindPath(startPosition, targetPosition);
        pathCalculated = true;
    }

    void Update()
    {
        if (pathCalculated && path.Count > 0)
        {
            MoveAlongPath();
        }
    }

    // A* Pathfinding Algorithm
    List<Vector3> FindPath(Vector3 start, Vector3 end)
    {
        // The lists of open and closed cells
        List<Vector3> openList = new List<Vector3>();
        HashSet<Vector3> closedList = new HashSet<Vector3>();

        openList.Add(start);

        // Store the gCost and hCost for each cell
        Dictionary<Vector3, int> gCosts = new Dictionary<Vector3, int>();
        Dictionary<Vector3, int> hCosts = new Dictionary<Vector3, int>();
        Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();

        gCosts[start] = 0;
        hCosts[start] = CalculateHeuristic(start, end);

        while (openList.Count > 0)
        {
            // Get cell with the lowest fCost (gCost + hCost)
            Vector3 current = openList.OrderBy(cell => gCosts[cell] + hCosts[cell]).First();

            // If the target is reached, reconstruct the path
            if (current == end)
            {
                return ReconstructPath(cameFrom, current);
            }

            openList.Remove(current);
            closedList.Add(current);

            // Explore neighbors (up, down, left, right)
            foreach (Vector3 direction in directions)
            {
                Vector3 neighbor = current + direction * nodeSize;

                // Skip if the neighbor is out of bounds or already visited
                if (!IsWalkable(neighbor) || closedList.Contains(neighbor))
                    continue;

                int tentativeGCost = gCosts[current] + 1; // assume all moves cost 1

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
                else if (tentativeGCost >= gCosts[neighbor])
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                gCosts[neighbor] = tentativeGCost;
                hCosts[neighbor] = CalculateHeuristic(neighbor, end);
            }
        }

        // No path found
        return new List<Vector3>();
    }

    // Calculate the heuristic (Manhattan distance for grid-based movement)
    int CalculateHeuristic(Vector3 a, Vector3 b)
    {
        return Mathf.Abs((int)(a.x - b.x)) + Mathf.Abs((int)(a.z - b.z));
    }

    // Reconstruct the path from end to start
    List<Vector3> ReconstructPath(Dictionary<Vector3, Vector3> cameFrom, Vector3 current)
    {
        List<Vector3> path = new List<Vector3>();

        while (cameFrom.ContainsKey(current))
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Reverse();
        return path;
    }

    // Check if the position is walkable
    bool IsWalkable(Vector3 position)
    {
        // For simplicity, assume that some positions are blocked (adjust for your maze)
        // E.g., mark positions as blocked in your maze and return false for those
        // Here we assume all positions are walkable except some blocked ones
        return !IsBlocked(position);
    }

    // Example of blocking positions (like walls in your maze)
    bool IsBlocked(Vector3 position)
    {
        // Example: positions (2, 1, 2) and (3, 1, 3) are blocked
        if (position == new Vector3(2, 1, 2) || position == new Vector3(3, 1, 3))
        {
            return true;
        }
        return false;
    }

    // Move the NPC along the path
    void MoveAlongPath()
    {
        if (path.Count > 0)
        {
            Vector3 target = path[0];
            Vector3 direction = (target - transform.position).normalized;

            // Move the NPC
            transform.Translate(direction * speed * Time.deltaTime, Space.World);

            // If close to the next position in the path, remove it
            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                path.RemoveAt(0);
            }
        }
    }
}
