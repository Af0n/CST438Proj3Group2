using UnityEngine;

[CreateAssetMenu(fileName = "Prices", menuName = "ScriptableObjects/Prices", order = 0)]
public class Prices : ScriptableObject {
    public int calmPrice;
    public int healPrice;
    public int manaPrice;
}