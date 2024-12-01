using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StationInput", menuName = "ScriptableObjects/StationInput", order = 0)]
public class StationInput : ScriptableObject {
    public StationRecipe[] recipes;
}