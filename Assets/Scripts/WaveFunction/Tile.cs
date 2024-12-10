using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile", menuName = "Wave Function/Tile")]
public class Tile : ScriptableObject
{
    [Header("Tile object")]
    [Tooltip("ID of the tile")]
    public int TileID = 0; 
    [Tooltip("Sprites or models representing the tile")]
    public TileBase tileSprite;
    [Tooltip("Possible adjacent tiles for each direction (Up, Right, Down, Left)")]
    public Tile[] possibleAdjacent;
    [Range(0,100)]
    [Tooltip("Weight of CurrentTile")]
    public int weight;
    [Range(0,10)]
    [Tooltip("The multplier for Adjacencents tiles of the same type.")]
    public float adjacencyMultplier;
}