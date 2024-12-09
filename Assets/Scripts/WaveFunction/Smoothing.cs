using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityEngine.Tilemaps;

public class Smoothing : MonoBehaviour
{
    public WorldGenerationSettings settings;
    public Tilemap SmoothTilemap(Tilemap tilemap)
    { 
        // Get the size of the tilemap bounds
        BoundsInt bounds = tilemap.cellBounds;

        // Create a temporary storage for the new tiles
        Dictionary<Vector3Int, TileBase> newTilemapState = new Dictionary<Vector3Int, TileBase>();

        // Iterate through each tile position in the tilemap
        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            TileBase currentTile = tilemap.GetTile(position);

            // Skip empty positions
            if (currentTile == null)
                continue;

            // Get the most common tile in the surrounding area
            TileBase mostCommonTile = GetMostCommonTile(tilemap, position);
            newTilemapState[position] = mostCommonTile;
        }

        // Apply the changes to the tilemap
        foreach (var kvp in newTilemapState)
        {
            tilemap.SetTile(kvp.Key, kvp.Value);
        }

        tilemap.RefreshAllTiles();
        return tilemap;
    }

    private TileBase GetMostCommonTile(Tilemap tilemap, Vector3Int position)
    {
        Dictionary<TileBase, int> tileFrequency = new Dictionary<TileBase, int>();

        // Sample tiles in a square around the position
        for (int x = -settings.smoothingRadius; x <= settings.smoothingRadius; x++)
        {
            for (int y = -settings.smoothingRadius; y <= settings.smoothingRadius; y++)
            {
                Vector3Int neighborPosition = position + new Vector3Int(x, y, 0);
                TileBase neighborTile = tilemap.GetTile(neighborPosition);

                if (neighborTile != null)
                {
                    if (!tileFrequency.ContainsKey(neighborTile))
                    {
                        tileFrequency[neighborTile] = 0;
                    }
                    tileFrequency[neighborTile]++;
                }
            }
        }

        TileBase mostCommonTile = null;
        int maxFrequency = 0;
        foreach (var kvp in tileFrequency)
        {
            if (kvp.Value > maxFrequency)
            {
                mostCommonTile = kvp.Key;
                maxFrequency = kvp.Value;
            }
        }

        return mostCommonTile;
    }
}
