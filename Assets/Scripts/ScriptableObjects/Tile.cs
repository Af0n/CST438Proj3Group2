using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "Wave Function/Tile")]
public class Tile : ScriptableObject
{
    [Header("Tile object")]
    [Tooltip("Sprites or models representing the tile")]
    public Sprite tileSprite;
    [Tooltip("Possible adjacent tiles for each direction (Up, Right, Down, Left)")]
    public Tile[] possibleAdjacent;
    [Range(0,100)]
    [Tooltip("Weight of Adjacent Tiles")]
    public int[] Adjacentweight;
}