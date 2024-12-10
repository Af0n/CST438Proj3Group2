using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerationPipeline : MonoBehaviour
{
    public WorldGenerationSettings wgs;
    public Pregeneration pregeneration;
    public WFCManager wfc;
    public Cleanup cleanup;
    public Smoothing smoothing;
    public bool generating = false;
    public ResizeCollider resize;
    private GridCell[,] _grid;
    public static Dictionary<TileBase, int> baseToTile = new Dictionary<TileBase, int>();
    // Threads for smoothing.. 
    private void Start()
    {
        wgs.tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        resize = FindAnyObjectByType<ResizeCollider>();
        foreach(Tile tile in wgs.allTiles) {
            baseToTile[tile.tileSprite] = tile.TileID;
        }

        foreach (var item in baseToTile)
        {
            Debug.Log(item);
        }
    
        StartCoroutine(runPipeline());
    }
    public void pipeline()
    {
        if (generating) return;
        StartCoroutine(runPipeline());
    }

    public IEnumerator runPipeline()
    {
        generating = true;
        _grid = pregeneration.PreGeneration(wgs);
        _grid = wfc.initGrid(_grid, pregeneration.tilesToCollapse, wgs);
        wgs.tilemap = placeSprites(_grid);
        for (int i = 0; i < wgs.numberOfPasses; i++)
        {
            wgs.tilemap = smoothing.SmoothTilemap(wgs);
        }
        cleanup.cleanupTiles(wgs);
        generating = false;
        yield return null;
    }


    public Tilemap placeSprites(GridCell[,] grid)
    {
        // Calculate the center of the Tilemap grid
        Vector3Int tilemapCenter = new Vector3Int(wgs.WFCWidth / 2, wgs.WFCHeight / 2, 0);
        wgs.tilemap.ClearAllTiles();
        for (int x = 0; x < wgs.WFCWidth; x++)
        {
            for (int y = 0; y < wgs.WFCHeight; y++)
            {
                GridCell cell = grid[x, y];
                // If its not collapsed, we need to fix it.. 
                if (!cell.IsCollapsed)
                {
                    continue;
                }
                Tile collapsedTile = cell.possibleTiles[0];
                // Calculate the offset from the center (using WFCWidth / 2 and WFCHeight / 2)
                Vector3Int tilePosition = new Vector3Int(x - tilemapCenter.x, y - tilemapCenter.y, 0);
                wgs.tilemap.SetTile(tilePosition, collapsedTile.tileSprite);

            }
        }
        resize.resizeCollider();
        return wgs.tilemap;
    }
}