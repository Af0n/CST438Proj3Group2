using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int WFCWidth = 32; // Number of Columns
    public int WFCHeight = 32; // Number of rows!

    [Header("Tiles")]
    public List<Tile> allTiles; // Array of all possible tiles
    private GridCell[,] grid;
    void Start() {
        initGrid();
    }
    void initGrid() {
        grid = new GridCell[WFCWidth, WFCHeight];

        for (int x = 0; x < WFCWidth; x++) {
            for (int y = 0; y < WFCHeight; y++) {
                grid[x, y] = new GridCell(allTiles);
                Debug.Log($"Cell [{x}, {y}] initialized with {grid[x, y].possibleTiles.Count} possible tiles.");
            }
        }
    }
}
