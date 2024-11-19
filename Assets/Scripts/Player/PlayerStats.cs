using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats", order = 0)]

public class PlayerStats : ScriptableObject {
    [Header("DON'T USE THESE FOR PLAYER SPEED\nUse playerStats.MoveSpeed")]
    public float freeSpeed;
    public float carrySpeed;

    [Header("Currency")]
    public int money;

    [Header("States")]
    public bool hasItem;
    public bool isBuilding;

    [Header("Debug")]
    public bool debug;

    public float MoveSpeed{
        get {
            float useSpeed;

            if(hasItem){
                useSpeed = carrySpeed;
            }else{
                useSpeed = freeSpeed;
            }
            
            if(debug){
                Debug.Log($"Using speed: {useSpeed}");
            }

            return useSpeed;
        }
    }

    public float MoveDeceleration = 0.5f; 
}