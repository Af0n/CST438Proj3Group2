using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerationPipeline : MonoBehaviour {
    public Pregeneration pregeneration;
    public WFCManager wfc; 
    public Tilemap tilemap;
    public Smoothing smoothing;
    public bool generating = false; 
    // Threads for smoothing.. 
    private void Start() {
        StartCoroutine(generateWorld());
    }
    public void regenerate() {
        if(generating) return;
        StartCoroutine(generateWorld());
    }

    public void smooth() {
        if(generating) return;
        StartCoroutine(smoothTileMap());
    }

    public IEnumerator generateWorld() {
        generating = true;
        GridCell[,] grid = pregeneration.PreGeneration();
        tilemap = wfc.initGrid(tilemap, grid, pregeneration.tilesToCollapse);
        generating = false;
        yield return null;
    }

    public IEnumerator smoothTileMap() {
        generating = true;
        tilemap = smoothing.SmoothTilemap(tilemap);
        generating = false;
        yield return null;
    }
}