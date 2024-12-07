using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using Unity.Collections;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class WFCManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int WFCWidth = 32; // Number of Columns
    public int WFCHeight = 32; // Number of rows!

    [Header("Tiles")]
    public List<Tile> allTiles; // Array of all possible tiles
    private GridCell[,] grid;

    private readonly int[,] directions = {
    { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 }, // Up, Down, Left, Right
    { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } // Diagonals
    };

    void Start()
    {
        initGrid();
    }
    // Add some inital rules here.. 
    public void initGrid()
    {
        grid = new GridCell[WFCWidth, WFCHeight];

        for (int x = 0; x < WFCWidth; x++)
        {
            for (int y = 0; y < WFCHeight; y++)
            {
                grid[x, y] = new GridCell(allTiles);
                Debug.Log($"Cell [{x}, {y}] initialized with {grid[x, y].possibleTiles.Count} possible tiles.");
            }
        }
    }

    private GridCell _get_Least_Entropy()
    {
        // Selection Variable
        GridCell selectedCell = null;
        int minEntropy = int.MaxValue;

        // Iterate through the grid
        for (int x = 0; x < WFCWidth; x++)
        {
            for (int y = 0; y < WFCHeight; y++)
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

    private void _collapse_Cell(int x, int y)
    {
        GridCell cell = grid[x, y];
        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int newX = x + directions[i, 0];
            int newY = y + directions[i, 1];
            if (newX >= 0 && newX < grid.GetLength(0) &&
            newY >= 0 && newY < grid.GetLength(1))
            {
                
            }
        }
    }


}
