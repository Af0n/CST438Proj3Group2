using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New World Generation Settings", menuName = "Wave Function/World Generation Settings")]
public class WorldGenerationSettings : ScriptableObject{
    // Just a file to preconfigure a world generation settings.
    [Header("Pre-generation")]
    public int OceanBoundarySize = 1;
    public int numberOfBlobs = 3;
    public int blobSize = 3;
    [Header("WFC Grid Settings")]
    public int WFCWidth = 32; // Number of Columns
    public int WFCHeight = 32; // Number of rows!
    [Space(32)]
    [Header("Smoothing")]
    public int smoothingRadius = 1;
    [Space(32)]
    [Header("Tiles")]
    public List<Tile> allTiles; // Array of all possible tile;
    [Space(32)]
    [Header("Features")]
    public int maxAmountOfFeatures; 
}