using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New TileAdj", menuName = "Wave Function/TileAdj")]
public class TileAdj : ScriptableObject
{
    [Tooltip("ID of both Adj tile")]
    public int IDAdj;
    public int IDBase;
    [Header("Adj tiles")]
    public TileBase upperLeft; 
    public TileBase upperMiddle; 
    public TileBase upperRight; 
    public TileBase middleRight; 
    public TileBase middleLeft; 
    public TileBase lowerLeft; 
    public TileBase lowerMiddle; 
    public TileBase lowerRight; 
}