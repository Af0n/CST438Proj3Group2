using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe", order = 0)]

public class Recipe : ScriptableObject {
    public ItemType type;
    public ItemType[] ingredients;
    public Sprite sprite;
    public bool containsBucket;
}