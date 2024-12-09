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
    private GridCell[,] grid;
    private readonly int[,] directions = {
    { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 }, // Up, Down, Left, Right
    { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } // Diagonals
    };
    private long _tilesToCollapse = 0;
    private WorldGenerationSettings _wgs;
    // Add some inital rules here.. 
    public GridCell[,] initGrid(GridCell[,] map, long toCollapse, WorldGenerationSettings wgs)
    {
        _wgs = wgs;
        grid = map;
        _tilesToCollapse = toCollapse;
        _RunWFC();
        return map;
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

        for (int x = 0; x < _wgs.WFCWidth; x++)
        {
            for (int y = 0; y < _wgs.WFCHeight; y++)
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

                if (newX >= 0 && newX < _wgs.WFCWidth && newY >= 0 && newY < _wgs.WFCHeight)
                {
                    if (!grid[newX, newY].IsCollapsed) continue;
                    Tile neighborTile = grid[newX, newY].possibleTiles[0];
                    if (!Array.Exists(tile.possibleAdjacent, t => t == neighborTile))
                    {
                        isValid = false;
                        break;
                    }

                }
            }
            if (isValid)
            {
                validTiles.Add(tile);
            }
        }
        if (validTiles.Count == 0)
        {
            Debug.LogWarning("No valid tiles available to collapse.");
            return;
        }

        List<Tile> weightedTiles = new List<Tile>();
        foreach (Tile validTile in validTiles)
        {
            int adjacentSameTypeCount = 0;
            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int newX = cell.x + directions[i, 0];
                int newY = cell.y + directions[i, 1];

                if (newX >= 0 && newX < _wgs.WFCWidth && newY >= 0 && newY < _wgs.WFCHeight)
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
            int adjustedWeight = validTile.weight + Mathf.RoundToInt(adjacentSameTypeCount * validTile.adjacencyMultplier);
            for (int i = 0; i < adjustedWeight; i++)
            {
                weightedTiles.Add(validTile);
            }
        }

        int randomIndex = UnityEngine.Random.Range(0, weightedTiles.Count);
        Tile selectedTile = weightedTiles[randomIndex];
        cell.possibleTiles = new List<Tile> { selectedTile };
        _tilesToCollapse--;
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

                if (neighborX >= 0 && neighborX < _wgs.WFCWidth && neighborY >= 0 && neighborY < _wgs.WFCHeight)
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
        int inverseDirection = (direction + 2) % 4;
        return Array.Exists(collapsedTile.possibleAdjacent, t => t == neighborTile);
    }







}
