using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityEngine.Tilemaps;

public class Smoothing : MonoBehaviour
{
    private WorldGenerationSettings _wgs; 
    public Tilemap SmoothTilemap(WorldGenerationSettings wgs)
    { 
        _wgs = wgs;
        // Get the size of the tilemap bounds
        BoundsInt bounds = _wgs.tilemap.cellBounds;

        // Create a temporary storage for the new tiles
        Dictionary<Vector3Int, TileBase> newTilemapState = new Dictionary<Vector3Int, TileBase>();

        // Iterate through each tile position in the tilemap
        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            TileBase currentTile = _wgs.tilemap.GetTile(position);

            // Skip empty positions
            if (currentTile == null)
                continue;

            // Get the most common tile in the surrounding area
            TileBase mostCommonTile = GetMostCommonTile(_wgs.tilemap, position);
            newTilemapState[position] = mostCommonTile;
        }

        // Apply the changes to the tilemap
        foreach (var kvp in newTilemapState)
        {
            _wgs.tilemap.SetTile(kvp.Key, kvp.Value);
        }

        _wgs.tilemap.RefreshAllTiles();
        return _wgs.tilemap;
    }

    private TileBase GetMostCommonTile(Tilemap tilemap, Vector3Int position)
    {
        Dictionary<TileBase, int> tileFrequency = new Dictionary<TileBase, int>();

        // Sample tiles in a square around the position
        for (int x = -_wgs.smoothingRadius; x <= _wgs.smoothingRadius; x++)
        {
            for (int y = -_wgs.smoothingRadius; y <= _wgs.smoothingRadius; y++)
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
