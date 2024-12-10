using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New feature", menuName = "Wave Function/Feature")]
public class Feature : ScriptableObject{
    [Header("Feature")]
    [Tooltip("What object is our feature?")]
    public GameObject featureObject;
    [Tooltip("What tile does this feature spawn on?")]
    public Tile baseTile;
    [Range(0, 100)]
    [Tooltip("How often this feature spawns in comparison to others?")]
    public int weight; 
    [Range(0, 25f)]
    [Tooltip("Min distance between features")]
    public float minDistance;
}