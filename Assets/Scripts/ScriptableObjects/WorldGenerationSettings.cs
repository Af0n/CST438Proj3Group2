using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New World Generation Settings", menuName = "Wave Function/World Generation Settings")]
public class WorldGenerationSettings : ScriptableObject{
    // Just a file to preconfigure a world generation settings.
    public Tilemap tilemap;
    [Space(32)]
    [Header("Pre-generation")]
    public int OceanBoundarySize = 1;
    public int numberOfBlobs = 3;
    public int blobYOffset = 10;
    public int blobXOffset = 9;
    public int blobMinSize = 2;
    public int blobMaxSize = 10;
    public Vector2Int spawnBoxSize = new Vector2Int(5, 5);
    public Vector2Int spawnBoxOffset = new Vector2Int(-5,-5);
    [Space(32)]
    [Header("WFC Grid Settings")]
    public int WFCWidth = 32; // Number of Columns
    public int WFCHeight = 32; // Number of rows!
    [Space(32)]
    [Header("Smoothing")]
    public int smoothingRadius = 1;
    public int numberOfPasses = 3;
    [Space(32)]
    [Header("Tiles")]
    public List<Tile> allTiles; // Array of all possible tile;
    [Space(32)]
    [Header("Features")]
    public int maxAmountOfFeatures; 
}