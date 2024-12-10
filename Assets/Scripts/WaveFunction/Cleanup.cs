using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cleanup : MonoBehaviour
{
    private WorldGenerationSettings _wgs;
    private readonly Vector2Int[] directions = {
        new Vector2Int(-1, 0),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(0, 1),
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(1, 1)
    };
    public Tilemap cleanupTiles(WorldGenerationSettings wgs)
    {
        _wgs = wgs;
        BoundsInt bounds = _wgs.tilemap.cellBounds;
        foreach (Vector2Int position in bounds.allPositionsWithin)
        {
            TileBase currentTile = _wgs.tilemap.GetTile((Vector3Int)position);
            // Skip empty positions
            if (currentTile == null)
            {
                continue;
            }
            TileBase retval = _checkConstraints((Vector3Int)position);
            if (retval != currentTile)
            {
                _wgs.tilemap.SetTile((Vector3Int)position, retval);
            }

        }
        return wgs.tilemap;
    }

    private TileBase _checkConstraints(Vector3Int position)
    {
        TileBase currentTileBase = _wgs.tilemap.GetTile(position);
        if (currentTileBase == null)
            return null;

        int tileIndex = WorldGenerationPipeline.baseToTile[currentTileBase];
        Tile currentTile = _wgs.allTiles[tileIndex];

        foreach (Vector3Int direction in directions)
        {
            Vector3Int adjacentPosition = position + direction;
            TileBase adjacentTileBase = _wgs.tilemap.GetTile(adjacentPosition);
            if (adjacentTileBase == null)
                continue;

            Tile adjacentTile = _wgs.allTiles[WorldGenerationPipeline.baseToTile[adjacentTileBase]];

            bool isValid = false;
            foreach (Tile possibleTile in currentTile.possibleAdjacent)
            {
                if (possibleTile == adjacentTile)
                {
                    isValid = true;
                    break;
                }
            }
            if (isValid)
            {
                continue;
            }

            foreach (Tile replacementTile in _wgs.allTiles)
            {
                bool replacementIsValid = true;
                foreach (Vector3Int checkDirection in directions)
                {
                    Vector3Int neighborPosition = position + checkDirection;
                    TileBase neighborTileBase = _wgs.tilemap.GetTile(neighborPosition);

                    if (neighborTileBase == null)
                        continue;

                    Tile neighborTile = _wgs.allTiles[WorldGenerationPipeline.baseToTile[neighborTileBase]];
                    if (!Array.Exists(replacementTile.possibleAdjacent, tile => tile == neighborTile))
                    {
                        replacementIsValid = false;
                        break;
                    }
                }
                if (replacementIsValid)
                {
                    _wgs.tilemap.SetTile(position, replacementTile.tileSprite);
                    return replacementTile.tileSprite;
                }
            }

        }
        return currentTileBase;
    }
}