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
        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            Tile tile = _wgs.allTiles[WorldGenerationPipeline.baseToTile[_wgs.tilemap.GetTile(position)]];
            if (tile == _wgs.allTiles[3]) continue;

            foreach (Feature feature in _wgs.allFeatures)
            {
                int rand = Random.Range(0, 100);
                if (rand > feature.weight) continue;
                if (feature.baseTile != tile) continue;
                Collider[] nearbyFeatures = Physics.OverlapSphere(position, feature.minDistance);
                if (nearbyFeatures.Any(collider => collider.GetComponent<Feature>() != null))
                {
                    continue; // Skip this feature if others are nearby
                }
                Instantiate(feature.featureObject, position, Quaternion.identity, transform);
            }
        }
    }
}
