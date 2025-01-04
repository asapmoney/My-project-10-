
using UnityEngine;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
public GameObject floorPrefab;

    public int width = 21;  // Must be odd numbers to have walls surrounding paths
    public int height = 21;

    private Cell[,] grid;
    private List<Vector2Int> wallList;

    void Start()
    {
        InitializeGrid();
        GenerateMaze();
        DrawMaze();
    }
    void InitializeGrid()
{
    grid = new Cell[width, height];

    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            grid[x, y] = new Cell();
        }
    }
}
void GenerateMaze()
{
    wallList = new List<Vector2Int>();

    // Choose a random starting cell
    int startX = Random.Range(1, width - 1);
    int startY = Random.Range(1, height - 1);

    // Ensure starting on an odd coordinate
    startX = startX % 2 == 0 ? startX - 1 : startX;
    startY = startY % 2 == 0 ? startY - 1 : startY;

    // Mark the starting cell as a passage
    grid[startX, startY].IsVisited = true;
    grid[startX, startY].IsWall = false;

    // Add the neighboring walls to the wall list
    AddWallsToList(startX, startY);
    
    // Main loop of the algorithm
    while (wallList.Count > 0)
    {
        // Select a random wall from the list
        int randomIndex = Random.Range(0, wallList.Count);
        Vector2Int wall = wallList[randomIndex];
        wallList.RemoveAt(randomIndex);

        // Check if the wall divides a visited and unvisited cell
        ProcessWall(wall);
    }
}
void AddWallsToList(int x, int y)
{
    if (x - 2 > 0) 
wallList.Add(new Vector2Int(x - 1, y));

    if (x + 2 < width - 1) 
wallList.Add(new Vector2Int(x + 1, y));

    if (y - 2 > 0)
wallList.Add(new Vector2Int(x, y - 1));

    if (y + 2 < height - 1)
wallList.Add(new Vector2Int(x, y + 1));
}

void ProcessWall(Vector2Int wall)
{
    int x = wall.x;
    int y = wall.y;

    // Determine the cells on either side of the wall
    List<Vector2Int> neighbors = GetNeighbors(x, y);

    if (neighbors.Count == 2)
    {
        Cell cell1 = grid[neighbors[0].x, neighbors[0].y];
        Cell cell2 = grid[neighbors[1].x, neighbors[1].y];

        // If one of the cells is unvisited
        if (cell1.IsVisited != cell2.IsVisited)
        {
            // Make the wall a passage
            grid[x, y].IsWall = false;

            // Mark the unvisited cell as visited
            if (!cell1.IsVisited)
            {
                cell1.IsVisited = true;
                cell1.IsWall = false;
                AddWallsToList(neighbors[0].x, neighbors[0].y);
            }
            else
            {
                cell2.IsVisited = true;
                cell2.IsWall = false;
                AddWallsToList(neighbors[1].x, neighbors[1].y);
            }
        }
    }
}
List<Vector2Int> GetNeighbors(int x, int y)
{
    List<Vector2Int> neighbors = new List<Vector2Int>();

    if (x % 2 == 1)
    {
        // Vertical wall
        if (y - 1 >= 0) neighbors.Add(new Vector2Int(x, y - 1));
        if (y + 1 < height) neighbors.Add(new Vector2Int(x, y + 1));
    }
    else if (y % 2 == 1)
    {
        // Horizontal wall
        if (x - 1 >= 0) neighbors.Add(new Vector2Int(x - 1, y));
        if (x + 1 < width) neighbors.Add(new Vector2Int(x + 1, y));
    }

    return neighbors;
}
void DrawMaze()
{
    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            Vector3 position = new Vector3(x, 0, y);

            if (grid[x, y].IsWall)
            {
                Instantiate(wallPrefab, position, Quaternion.identity, transform);
            }
            else
            {
                Instantiate(floorPrefab, position, Quaternion.identity, transform);
            }
        }
    }
}


}
