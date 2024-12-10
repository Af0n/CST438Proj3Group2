using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class GridCell
{
    public int x;
    public int y;
    // Okay Which are possible?
    public List<Tile> possibleTiles;
    // Is the Cell collapsed?
    public bool IsCollapsed => possibleTiles.Count == 1;

    // GridCell Constructor
    public GridCell(int _x, int _y, List<Tile> initialTiles) {
        x = _x;
        y = _y;  
        possibleTiles = new List<Tile>(initialTiles);
    }
}
