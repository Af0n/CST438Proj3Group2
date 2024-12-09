using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pregeneration : MonoBehaviour {
    public WorldGenerationSettings wgs;
    public long tilesToCollapse = 0;
    public GridCell[,] PreGeneration() {
        tilesToCollapse = wgs.WFCWidth * wgs.WFCHeight;
        GridCell[,] grid = new GridCell[wgs.WFCWidth, wgs.WFCHeight];
        for (int x = 0; x < wgs.WFCWidth; x++)
        {
            for (int y = 0; y < wgs.WFCHeight; y++)
            {
                grid[x, y] = new GridCell(x, y, wgs.allTiles);
                if (x < wgs.OceanBoundarySize || x >= wgs.WFCWidth - wgs.OceanBoundarySize || y < wgs.OceanBoundarySize || y >= wgs.WFCHeight - wgs.OceanBoundarySize)
                {
                    grid[x, y].possibleTiles = new List<Tile> { wgs.allTiles[3] };
                    tilesToCollapse--;
                }

            }
        }
        return grid;
    }
}