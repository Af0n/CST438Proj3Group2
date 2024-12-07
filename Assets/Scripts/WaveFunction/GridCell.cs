using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GridCell
{
    // Okay Which are possible?
    public List<Tile> possibleTiles;
    // Is the Cell collapsed?
    public bool IsCollapsed => possibleTiles.Count == 1;

    // GridCell Constructor
    public GridCell(List<Tile> initialTiles) {
        // Cast as Arraylist
        possibleTiles = new List<Tile>(initialTiles);
    }
}
