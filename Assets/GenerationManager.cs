using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridWidth = 32;
    public int gridHeight = 32;
    public float tileWidth = 1f;
    public float tileHeight = 1f;

    public int waterEdgeDistance = 3;
    [Header("Tile Settings")]
    public Tile[] tiles;
    private Cell[,] grid;

    private void Start()
    {
        InitializeGrid();
        RunWFC();
    }

    void InitializeGrid()
    {
        grid = new Cell[gridWidth, gridHeight];

        // Initialize the grid with all possible tiles in every cell
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = new Cell(tiles);
            }
        }
    }

    void RunWFC()
    {
        while (!IsFullyCollapsed())
        {
            // 1. Find the cell with the lowest entropy (fewest possibilities)
            Vector2Int cellToCollapse = FindLowestEntropyCell();

            // 2. Collapse the cell (choose a tile)
            CollapseCell(cellToCollapse);

            // 3. Propagate constraints to neighboring cells
            PropagateConstraints(cellToCollapse);
        }
        PlaceTiles();

        Debug.Log("WFC completed!");
    }

    bool IsFullyCollapsed()
    {
        foreach (var cell in grid)
        {
            if (!cell.IsCollapsed)
                return false;
        }
        return true;
    }

    Vector2Int FindLowestEntropyCell()
    {
        Vector2Int bestCell = Vector2Int.zero;
        int lowestEntropy = int.MaxValue;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                var cell = grid[x, y];
                if (!cell.IsCollapsed && cell.Entropy < lowestEntropy)
                {
                    lowestEntropy = cell.Entropy;
                    bestCell = new Vector2Int(x, y);
                }
            }
        }

        return bestCell;
    }

    void CollapseCell(Vector2Int position)
    {
        var cell = grid[position.x, position.y];
        cell.Collapse(position, gridWidth, gridHeight, tiles[3], tiles[2], grid, waterEdgeDistance);
    }
    void PlaceTiles()
    {
        // Parent object to organize instantiated tiles in the scene
        GameObject tilesParent = new GameObject("GeneratedTiles");

        // Adjust these values based on your tile size
        

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                var cell = grid[x, y];

                if (!cell.IsCollapsed)
                {
                    Debug.LogWarning($"Cell at ({x}, {y}) is not collapsed. Skipping placement.");
                    continue;
                }

                // Get the chosen tile
                Tile chosenTile = cell.possibleTiles[0];

                // Create a GameObject to represent the tile
                GameObject tileObject = new GameObject($"Tile_{x}_{y}");
                tileObject.transform.SetParent(tilesParent.transform);

                // Set the position of the tile based on grid coordinates and tile dimensions
                tileObject.transform.position = new Vector3(x * tileWidth, y * tileHeight, 0);

                // Add a SpriteRenderer and assign the tile's sprite
                SpriteRenderer renderer = tileObject.AddComponent<SpriteRenderer>();
                renderer.sprite = chosenTile.tileSprite;

                // Optional: Adjust sprite size if needed
                renderer.drawMode = SpriteDrawMode.Simple;
            }
        }
    }


    void PropagateConstraints(Vector2Int position)
    {
        // Lambda for applying constraints
        Queue<Vector2Int> toProcess = new Queue<Vector2Int>();
        System.Action<Vector2Int, Vector2Int> ApplyConstraints = (from, to) =>
        {
            var fromCell = grid[from.x, from.y];
            var toCell = grid[to.x, to.y];

            if (toCell.IsCollapsed)
                return;

            // Filter the possibilities of the "to" cell based on the chosen tile in the "from" cell
            bool changed = toCell.FilterPossibilities(fromCell.GetValidNeighbors());

            if (changed)
            {
                // If the possibilities were reduced, add this cell to the queue for further propagation
                toProcess.Enqueue(to);
            }
        };

        toProcess.Enqueue(position);

        while (toProcess.Count > 0)
        {
            var current = toProcess.Dequeue();
            int x = current.x;
            int y = current.y;

            // Apply constraints to neighbors
            if (x > 0) ApplyConstraints(current, new Vector2Int(x - 1, y));
            if (x < gridWidth - 1) ApplyConstraints(current, new Vector2Int(x + 1, y));
            if (y > 0) ApplyConstraints(current, new Vector2Int(x, y - 1));
            if (y < gridHeight - 1) ApplyConstraints(current, new Vector2Int(x, y + 1));
        }
    }


}
