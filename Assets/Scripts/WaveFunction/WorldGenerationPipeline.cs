using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerationPipeline : MonoBehaviour {
    public WorldGenerationSettings wgs;
    public Pregeneration pregeneration;
    public WFCManager wfc; 
    
    public Smoothing smoothing;
    public bool generating = false; 
    // Threads for smoothing.. 
    private void Start() {
        wgs.tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        StartCoroutine(runPipeline());
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
        GridCell[,] grid = pregeneration.PreGeneration(wgs);
        wgs.tilemap = wfc.initGrid(grid, pregeneration.tilesToCollapse, wgs);
        generating = false;
        yield return null;
    }

    public IEnumerator smoothTileMap() {
        generating = true;
        wgs.tilemap = smoothing.SmoothTilemap(wgs);
        generating = false;
        yield return null;
    }

    public IEnumerator runPipeline() {
        generating = true;
        GridCell[,] grid = pregeneration.PreGeneration(wgs);
        wgs.tilemap = wfc.initGrid(grid, pregeneration.tilesToCollapse, wgs);
        for(int i = 0; i < wgs.numberOfPasses; i++)
            wgs.tilemap = smoothing.SmoothTilemap(wgs);
        generating = false;
        yield return null;
    }
}