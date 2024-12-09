using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerationPipeline : MonoBehaviour {
    public WFCManager wfc; 
    public Tilemap tilemap;
    public bool generating = false; 
    // Threads for smoothing.. 
    private void Start() {
        StartCoroutine(generateWorld());
    }

    public void regenerate() {
        StartCoroutine(generateWorld());
    }
    public IEnumerator generateWorld() {
        generating = true;
        tilemap = wfc.initGrid(tilemap);
        generating = false;
        yield return null;
    }
}