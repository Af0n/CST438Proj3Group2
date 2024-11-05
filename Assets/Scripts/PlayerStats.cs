using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerStats", order = 0)]

public class PlayerStats : ScriptableObject {
    [Header("DON'T USE THESE FOR PLAYER SPEED\nUse playerStats.MoveSpeed")]
    public float speed;
    public float carrySpeed;

    [Header("Currency")]
    public int money;

    [Header("States")]
    public bool hasItem;

    public float MoveSpeed{
        get{
            if(hasItem){
                return carrySpeed;
            }
            return speed;
        }
    }
}