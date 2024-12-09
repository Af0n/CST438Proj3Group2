using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pregeneration : MonoBehaviour
{
    [HideInInspector]
    public long tilesToCollapse = 0;
    private GridCell[,] grid;
    private WorldGenerationSettings _wgs;
    public GridCell[,] PreGeneration(WorldGenerationSettings wgs)
    {
        _wgs = wgs;
        tilesToCollapse = wgs.WFCWidth * wgs.WFCHeight;
        grid = new GridCell[wgs.WFCWidth, wgs.WFCHeight];
        for (int x = 0; x < wgs.WFCWidth; x++)
        {
            for (int y = 0; y < wgs.WFCHeight; y++)
            {
                grid[x, y] = new GridCell(x, y, wgs.allTiles);
            }
        }
        generateBlobs();

        // Ocean boundary
        for (int x = 0; x < wgs.WFCWidth; x++)
        {
            for (int y = 0; y < wgs.WFCHeight; y++)
            {
                if (x < wgs.OceanBoundarySize || x >= wgs.WFCWidth - wgs.OceanBoundarySize || y < wgs.OceanBoundarySize || y >= wgs.WFCHeight - wgs.OceanBoundarySize)
                {
                    grid[x, y].possibleTiles = new List<Tile> { wgs.allTiles[3] };
                }
            }
        }
        return grid;
    }

    void generateBlobs()
    {
        for (int i = 0; i < _wgs.numberOfBlobs; i++)
        {
            // Random starting position within the tilemap boundaries
            Vector2Int startPosition = new Vector2Int(
                Random.Range(0, _wgs.WFCWidth),
                Random.Range(0, _wgs.WFCHeight)
            );

            // Random blob size
            int blobSize = Random.Range(_wgs.blobMinSize, _wgs.blobMaxSize);
            int randomIndex = Random.Range(0, _wgs.allTiles.Count - 2);
            _generateBlob(startPosition, blobSize, _wgs.allTiles[randomIndex]);
        }
        // Spawn box
        Vector2Int boxPos = new Vector2Int(_wgs.WFCWidth / 2 + _wgs.spawnBoxOffset.x, _wgs.WFCHeight / 2 + _wgs.spawnBoxOffset.y);
        _generateBox(boxPos, _wgs.spawnBoxSize, _wgs.allTiles[0]);
    }
    private void _generateBlob(Vector2Int center, int area, Tile tile)
    {
        float radius = Mathf.Sqrt(area / Mathf.PI);
        HashSet<Vector2Int> filledPositions = new HashSet<Vector2Int>();

        // Start with a basic filled circle
        for (int x = Mathf.FloorToInt(-radius); x <= Mathf.CeilToInt(radius); x++)
        {
            for (int y = Mathf.FloorToInt(-radius); y <= Mathf.CeilToInt(radius); y++)
            {
                Vector2Int offset = new Vector2Int(x, y);
                float distance = offset.magnitude;

                // Check if the point is within the radius
                if (distance <= radius)
                {
                    // Deform the circle by slightly shifting the radius for this point
                    float deformation = Random.Range(-radius * 0.1f, radius * 0.1f);
                    if (distance + deformation <= radius)
                    {
                        Vector2Int tilePosition = center + offset;

                        // Check bounds to ensure tile stays within the tilemap
                        if (tilePosition.x >= 0 && tilePosition.x < _wgs.WFCWidth &&
                            tilePosition.y >= 0 && tilePosition.y < _wgs.WFCHeight)
                        {
                            filledPositions.Add(tilePosition);
                        }
                    }
                }
            }
        }

        // Limit the filled positions to match the exact area
        foreach (Vector2Int position in filledPositions)
        {
            if (filledPositions.Count > area)
                break;

            // Set the tile in the grid
            grid[position.x, position.y].possibleTiles = new List<Tile> { tile };
        }
    }




    private void _generateBox(Vector2Int position, Vector2Int size, Tile tile)
    {
        // Loop through all positions in the box area
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                // Calculate the tile position based on the starting position and size
                Vector2Int tilePosition = new Vector2Int(position.x + x, position.y + y);

                // Place the tile at the calculated position
                grid[tilePosition.x, tilePosition.y].possibleTiles = new List<Tile> { tile };
            }
        }
    }
}