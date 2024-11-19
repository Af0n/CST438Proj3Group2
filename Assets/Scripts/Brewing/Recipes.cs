using UnityEngine;

[CreateAssetMenu(fileName = "Recipes", menuName = "ScriptableObjects/Recipes", order = 0)]
public class Recipes : ScriptableObject {
    public Recipe[] recipes;
}