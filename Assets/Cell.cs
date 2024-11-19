using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell
{
    public Vector2Int position;
    public List<Tile> possibleTiles;

    public bool IsCollapsed => possibleTiles.Count == 1;
    public int Entropy => possibleTiles.Count;

    public Cell(Tile[] allTiles)
    {
        possibleTiles = new List<Tile>(allTiles);
    }

    public Cell(Vector2Int pos, List<Tile> tiles)
    {
        position = pos;
        possibleTiles = tiles;
    }

    public void Collapse(Vector2Int position, int gridWidth, int gridHeight, Tile waterTile, Tile sandTile, Cell[,] grid, int waterEdgeDistance)
    {
        // Define the potential neighboring positions (left, right, down, up)
        Vector2Int[] neighborOffsets = new Vector2Int[]
        {
            new Vector2Int(-1, 0), // Left
            new Vector2Int(1, 0),  // Right
            new Vector2Int(0, -1), // Down
            new Vector2Int(0, 1)   // Up
        };
        // 1. Water cannot be placed near the center
        bool isNearCenter = Mathf.Abs(position.x - gridWidth / 2) <= 2 && Mathf.Abs(position.y - gridHeight / 2) <= 2;

        if (isNearCenter)
        {
            // Restrict possible tiles to non-water tiles if near the center
            possibleTiles = possibleTiles.Where(t => t != waterTile).ToList();
        }

        // 2. Water must be connected to at least one water tile (and water can only be on the edges)
        if (possibleTiles.Contains(waterTile))
        {
            bool isConnectedToWater = false;



            // Check each neighbor
            foreach (var offset in neighborOffsets)
            {
                Vector2Int neighborPosition = position + offset;

                // Ensure the neighbor is within bounds
                if (neighborPosition.x >= 0 && neighborPosition.x < gridWidth && neighborPosition.y >= 0 && neighborPosition.y < gridHeight)
                {
                    var neighborCell = grid[neighborPosition.x, neighborPosition.y];

                    // Check if the neighbor has a water tile as a valid option
                    if (neighborCell != null && neighborCell.possibleTiles.Contains(waterTile))
                    {
                        isConnectedToWater = true;
                        break; // No need to check further once connected to water
                    }
                }
            }

            // If not connected to water, restrict possible tiles to non-water tiles
            if (!isConnectedToWater)
            {
                possibleTiles = possibleTiles.Where(t => t != waterTile).ToList();
            }
        }

        // 3. Water must spawn at the edges of the map (if it's within the restricted edge area, it can only be water)
        bool isNearEdge = position.x < waterEdgeDistance || position.y < waterEdgeDistance || position.x >= gridWidth - waterEdgeDistance || position.y >= gridHeight - waterEdgeDistance;

        if (isNearEdge)
        {
            possibleTiles = possibleTiles.Where(t => t == waterTile).ToList();
        }

        // 4. Sand cannot be isolated (must be adjacent to at least one other sand tile)
        if (possibleTiles.Contains(sandTile))
        {
            bool isSandConnected = false;

            // Check neighbors for sand tiles
            foreach (var offset in neighborOffsets)
            {
                Vector2Int neighborPosition = position + offset;

                // Ensure the neighbor is within bounds
                if (neighborPosition.x >= 0 && neighborPosition.x < gridWidth && neighborPosition.y >= 0 && neighborPosition.y < gridHeight)
                {
                    var neighborCell = grid[neighborPosition.x, neighborPosition.y];

                    // Check if the neighbor has sand as a valid option
                    if (neighborCell != null && neighborCell.possibleTiles.Contains(sandTile))
                    {
                        isSandConnected = true;
                        break; // No need to check further once connected to sand
                    }
                }
            }

            // If not connected to another sand tile, restrict possible tiles to non-sand tiles
            if (!isSandConnected)
            {
                possibleTiles = possibleTiles.Where(t => t != sandTile).ToList();
            }
        }
        if (possibleTiles.Contains(waterTile))
        {
            bool isSurroundedByWater = true;

            // Check all 4 neighboring positions (left, right, down, up)
            foreach (var offset in neighborOffsets)
            {
                Vector2Int neighborPosition = position + offset;

                // Ensure the neighbor is within bounds
                if (neighborPosition.x >= 0 && neighborPosition.x < gridWidth && neighborPosition.y >= 0 && neighborPosition.y < gridHeight)
                {
                    var neighborCell = grid[neighborPosition.x, neighborPosition.y];

                    // If any neighbor is not water, then the tile is not completely surrounded
                    if (neighborCell == null || !neighborCell.possibleTiles.Contains(waterTile))
                    {
                        isSurroundedByWater = false;
                        break;
                    }
                }
            }

            // If the tile is surrounded by water, remove water from possible tiles
            if (isSurroundedByWater)
            {
                possibleTiles = possibleTiles.Where(t => t != waterTile).ToList();
            }
        }
        // 5. Normal collapse logic (selecting a tile probabilistically based on weights)
        Dictionary<Tile, float> weightedTiles = new Dictionary<Tile, float>();
        float totalWeight = 0f;

        foreach (var tile in possibleTiles)
        {
            float weight = tile.baseWeight;
            weightedTiles[tile] = weight;
            totalWeight += weight;
        }

        // Select a tile probabilistically based on weights
        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var tile in weightedTiles)
        {
            cumulativeWeight += tile.Value;
            if (randomValue <= cumulativeWeight)
            {
                possibleTiles = new List<Tile> { tile.Key }; // Collapse to this tile
                break;
            }
        }
    }

    public bool FilterPossibilities(Tile[] validTiles)
    {
        int beforeCount = possibleTiles.Count;

        // Keep only tiles that are valid neighbors
        possibleTiles = possibleTiles.FindAll(tile => System.Array.Exists(validTiles, t => t == tile));

        return possibleTiles.Count < beforeCount; // Return true if the possibilities were reduced
    }

    public Tile[] GetValidNeighbors()
    {
        // Gather all possible neighbors from the current possible tiles
        HashSet<Tile> validNeighbors = new HashSet<Tile>();
        foreach (var tile in possibleTiles)
        {
            foreach (var neighbor in tile.possibleAdjacent)
            {
                validNeighbors.Add(neighbor);
            }
        }

        return validNeighbors.ToArray();
    }
    private List<Cell> GetNeighbors(Vector2Int position, Cell[,] grid)
    {
        List<Cell> neighbors = new List<Cell>();

        int x = position.x;
        int y = position.y;

        // Check bounds and add neighbors (Up, Right, Down, Left)
        if (x > 0) neighbors.Add(grid[x - 1, y]);
        if (x < grid.GetLength(0) - 1) neighbors.Add(grid[x + 1, y]);
        if (y > 0) neighbors.Add(grid[x, y - 1]);
        if (y < grid.GetLength(1) - 1) neighbors.Add(grid[x, y + 1]);

        return neighbors;
    }


}