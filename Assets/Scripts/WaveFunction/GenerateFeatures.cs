using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateFeatures : MonoBehaviour
{
    private WorldGenerationSettings _wgs;
    public void generate(WorldGenerationSettings wgs)
    {
        _wgs = wgs;
        BoundsInt bounds = _wgs.tilemap.cellBounds;
        foreach (Vector2Int position in bounds.allPositionsWithin)
        {
            //TODO: 
            // Just spawn the feature
        }
    }
}
