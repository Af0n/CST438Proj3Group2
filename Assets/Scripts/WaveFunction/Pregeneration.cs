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
            int randomIndex = Random.Range(0, _wgs.allTiles.Count-2);
            _generateBlob(startPosition, blobSize, _wgs.allTiles[randomIndex]);
        }
        // Spawn box
        Vector2Int boxPos = new Vector2Int(_wgs.WFCWidth/2 + _wgs.spawnBoxOffset.x, _wgs.WFCHeight/2 + _wgs.spawnBoxOffset.y);
        _generateBox(boxPos, _wgs.spawnBoxSize, _wgs.allTiles[0]);
    }
    private void _generateBlob(Vector2Int position, int size, Tile tile)
    {
        for (int i = 0; i < size; i++)
        {
            // Random offset from the starting position
            Vector2Int offset = new Vector2Int(
                Random.Range(-_wgs.blobYOffset, _wgs.blobXOffset), // Range for x-offset
                Random.Range(-_wgs.blobXOffset, _wgs.blobYOffset)  // Range for y-offset
            );

            Vector2Int tilePosition = position + offset;

            if (tilePosition.x >= 0 && tilePosition.x < _wgs.WFCWidth &&
                tilePosition.y >= 0 && tilePosition.y < _wgs.WFCHeight)
            {
                grid[tilePosition.x, tilePosition.y].possibleTiles = new List<Tile> { tile };
            }
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