using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateFeatures : MonoBehaviour
{
    private WorldGenerationSettings _wgs;
    public void generate(WorldGenerationSettings wgs)
    {
        _wgs = wgs;
        BoundsInt bounds = _wgs.tilemap.cellBounds;
         Debug.Log($"Bounds: xMin = {bounds.xMin}, xMax = {bounds.xMax}, yMin = {bounds.yMin}, yMax = {bounds.yMax}");

        // Iterate over the bounds and corresponding tiles
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                TileBase tileBase = _wgs.tilemap.GetTile(position);

                // Skip if there's no tile at this position
                if (tileBase == null) continue;

                // Map the tileBase to a Tile object
                if (!WorldGenerationPipeline.baseToTile.ContainsKey(tileBase)) continue;
                Tile tile = _wgs.allTiles[WorldGenerationPipeline.baseToTile[tileBase]];

                // Skip specific tiles (e.g., index 3)
                if (tile == _wgs.allTiles[3]) continue;

                foreach (Feature feature in _wgs.allFeatures)
                {
                    if (IsFeaturePlaceable(position, tile, feature))
                    {
                        // Convert grid position to world position
                        Vector3 worldPosition = _wgs.tilemap.CellToWorld(position);
                        Debug.Log(worldPosition);
                        // Adjust for tile center (optional, based on your setup)
                        worldPosition += _wgs.tilemap.tileAnchor;

                        Instantiate(feature.featureObject, worldPosition, Quaternion.identity, transform);
                    }
                }
            }
        }
    }

    private bool IsFeaturePlaceable(Vector3Int position, Tile tile, Feature feature)
    {
        if (Random.Range(0, 100) > feature.weight) return false;
        if (feature.baseTile != tile) return false;

        // Convert grid position to world position for overlap check
        Vector3 worldPosition = _wgs.tilemap.CellToWorld(position);
        Collider[] nearbyFeatures = Physics.OverlapSphere(worldPosition, feature.minDistance, _wgs.featureMask);

        return !nearbyFeatures.Any(collider => collider.GetComponent<Feature>() != null);
    }


}
