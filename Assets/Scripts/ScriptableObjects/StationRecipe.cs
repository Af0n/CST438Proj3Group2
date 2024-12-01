using UnityEngine;

[CreateAssetMenu(fileName = "StationRecipe", menuName = "ScriptableObjects/StationRecipe", order = 0)]
public class StationRecipe : ScriptableObject {
    public ItemType input;
    public ItemType[] output;
}