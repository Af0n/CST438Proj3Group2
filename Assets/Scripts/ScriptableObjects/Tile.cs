using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "ScriptableObjects/Tile")]
public class Tile : ScriptableObject
{
    [Header("Tile object")]
    public string tileName;
    [Tooltip("Sprites or models representing the tile")]
    public Sprite tileSprite;
    [Tooltip("Possible adjacent tiles for each direction (Up, Right, Down, Left)")]
    public Tile[] possibleAdjacent;
    [Tooltip("Weight multiplier for tile placement priority")]
    public float baseWeight = 1f;
}
