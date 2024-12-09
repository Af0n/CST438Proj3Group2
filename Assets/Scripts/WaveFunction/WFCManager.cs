using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using Unity.Collections;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WFCManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public WorldGenerationSettings settings;
    private GridCell[,] grid;
    private readonly int[,] directions = {
    { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 }, // Up, Down, Left, Right
    { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } // Diagonals
    };
    private long _tilesToCollapse = 0;
    // Add some inital rules here.. 
    public Tilemap initGrid(Tilemap tilemap, GridCell[,] map, long toCollapse)
    {
        _tilesToCollapse = toCollapse;
        _RunWFC();
        return _PlaceSprites(tilemap);
    }

    private void _RunWFC()
    {
        while (_tilesToCollapse > 0)
        {
            GridCell cell = _get_Least_Entropy();
            if (cell == null) return; // Safety check

            _CollapseCell(cell);
            _PropagateConstraints(cell);
        }

        Debug.Log("WFC completed!");
    }

    private GridCell _get_Least_Entropy()
    {
        GridCell selectedCell = null;
        int minEntropy = int.MaxValue;

        for (int x = 0; x < settings.WFCWidth; x++)
        {
            for (int y = 0; y < settings.WFCHeight; y++)
            {
                GridCell cell = grid[x, y];
                if (cell.IsCollapsed) continue;
                if (cell.possibleTiles.Count < minEntropy)
                {
                    minEntropy = cell.possibleTiles.Count;
                    selectedCell = cell;
                }
            }
        }

        return selectedCell;
    }

    private void _CollapseCell(GridCell cell)
    {
        // List to store tiles that satisfy the adjacency conditions
        List<Tile> validTiles = new List<Tile>();

        // Check the adjacent cells and filter the possible tiles based on the adjacency rules
        foreach (Tile tile in cell.possibleTiles)
        {
            bool isValid = true;

            // Check each adjacent direction
            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int newX = cell.x + directions[i, 0];
                int newY = cell.y + directions[i, 1];

                if (newX >= 0 && newX < settings.WFCWidth && newY >= 0 && newY < settings.WFCHeight)
                {
                    // Get the neighboring tile, if it's collapsed
                    if (grid[newX, newY].IsCollapsed)
                    {
                        Tile neighborTile = grid[newX, newY].possibleTiles[0];

                        // Check if the current tile is adjacent to the neighbor tile
                        if (!Array.Exists(tile.possibleAdjacent, t => t == neighborTile))
                        {
                            isValid = false;
                            break; // No need to check further if any adjacent rule fails
                        }
                    }
                }
            }

            // If the tile satisfies all adjacency rules, add it to the validTiles list
            if (isValid)
            {
                validTiles.Add(tile);
            }
        }

        // If no valid tiles are found, warn and return (this is a fail state, ideally shouldn't happen)
        if (validTiles.Count == 0)
        {
            Debug.LogWarning("No valid tiles available to collapse.");
            return;
        }

        // Now, create the weighted list based on the valid tiles
        List<Tile> weightedTiles = new List<Tile>();
        foreach (Tile validTile in validTiles)
        {
            // Calculate the number of adjacent tiles of the same type
            int adjacentSameTypeCount = 0;
            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int newX = cell.x + directions[i, 0];
                int newY = cell.y + directions[i, 1];

                if (newX >= 0 && newX < settings.WFCWidth && newY >= 0 && newY < settings.WFCHeight)
                {
                    if (grid[newX, newY].IsCollapsed)
                    {
                        Tile neighborTile = grid[newX, newY].possibleTiles[0];
                        if (neighborTile == validTile)
                        {
                            adjacentSameTypeCount++;
                        }
                    }
                }
            }

            // Adjust the weight using the adjacency multiplier
            int adjustedWeight = validTile.weight + Mathf.RoundToInt(adjacentSameTypeCount * validTile.adjacencyMultplier);

            // Add the tile to the weighted list according to its adjusted weight
            for (int i = 0; i < adjustedWeight; i++)
            {
                weightedTiles.Add(validTile);
            }
        }

        // Select a tile randomly from the weighted list
        int randomIndex = UnityEngine.Random.Range(0, weightedTiles.Count);
        Tile selectedTile = weightedTiles[randomIndex];

        // Collapse the cell by setting possibleTiles to only the selected tile
        cell.possibleTiles = new List<Tile> { selectedTile };
        _tilesToCollapse--; // Decrease the counter for remaining cells to collapse

        Debug.Log($"Collapsed cell at [{cell.x}, {cell.y}] to tile: {selectedTile.name} with weight {selectedTile.weight}");
    }


    private void _PropagateConstraints(GridCell collapsedCell)
    {
        Queue<(int x, int y)> queue = new Queue<(int, int)>();

        queue.Enqueue((collapsedCell.x, collapsedCell.y));

        while (queue.Count > 0)
        {
            var (currentX, currentY) = queue.Dequeue();
            GridCell currentCell = grid[currentX, currentY];

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int neighborX = currentX + directions[i, 0];
                int neighborY = currentY + directions[i, 1];

                if (neighborX >= 0 && neighborX < settings.WFCWidth && neighborY >= 0 && neighborY < settings.WFCHeight)
                {
                    GridCell neighborCell = grid[neighborX, neighborY];
                    if (neighborCell.IsCollapsed) continue;

                    List<Tile> validTiles = new List<Tile>();
                    foreach (Tile tile in neighborCell.possibleTiles)
                    {
                        if (_IsValidNeighbor(tile, currentCell.possibleTiles[0], i))
                        {
                            validTiles.Add(tile);
                        }
                    }

                    if (validTiles.Count < neighborCell.possibleTiles.Count)
                    {
                        neighborCell.possibleTiles = validTiles;
                        queue.Enqueue((neighborX, neighborY));
                    }
                }
            }
        }
    }

    private bool _IsValidNeighbor(Tile neighborTile, Tile collapsedTile, int direction)
    {
        // Inverse the direction to match the adjacency array
        int inverseDirection = (direction + 2) % 4;
        return Array.Exists(collapsedTile.possibleAdjacent, t => t == neighborTile);
    }

    public Tilemap _PlaceSprites(Tilemap tilemap)
    {

        // Clear existing tiles from the Tilemap (optional, depends on whether you want to clear previous ones)
        tilemap.ClearAllTiles();

        // Calculate the center of the Tilemap grid
        Vector3Int tilemapCenter = new Vector3Int(settings.WFCWidth / 2, settings.WFCHeight / 2, 0);

        // Loop through the grid and place the tiles into the Tilemap
        for (int x = 0; x < settings.WFCWidth; x++)
        {
            for (int y = 0; y < settings.WFCHeight; y++)
            {
                GridCell cell = grid[x, y];
                if (cell.IsCollapsed)
                {
                    Tile collapsedTile = cell.possibleTiles[0];

                    // Calculate the offset from the center (using WFCWidth / 2 and WFCHeight / 2)
                    Vector3Int tilePosition = new Vector3Int(x - tilemapCenter.x, y - tilemapCenter.y, 0);

                    // Place the tile in the Tilemap (you can replace Tile with your custom tile if needed)
                    tilemap.SetTile(tilePosition, collapsedTile.tileSprite); // Use tile sprite here if you want
                }
            }
        }
        return tilemap;
    }





}
