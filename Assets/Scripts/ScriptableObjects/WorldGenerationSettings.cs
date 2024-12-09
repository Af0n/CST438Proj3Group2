using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New World Generation Settings", menuName = "Wave Function/World Generation Settings")]
public class WorldGenerationSettings : ScriptableObject{
    // Just a file to preconfigure a world generation settings.
    [Header("Grid Settings")]
    public int WFCWidth = 32; // Number of Columns
    public int WFCHeight = 32; // Number of rows!
    public int OceanBoundarySize = 1;
    [Header("Tiles")]
    public List<Tile> allTiles; // Array of all possible tile;
}