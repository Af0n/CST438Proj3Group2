using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerStats stats;
    public HUDScript hud;

    // initializes the yapping variable to false
    private void Awake()
    {
        stats.isYapping = false;
        stats.isBusy = false;
    }

    void Update() {
        hud.UpdateMoneyText(stats.money);
    }

    public bool CanAfford(int price){
        bool result = stats.money >= price;
        if(stats.debug){
            Debug.Log($"Price: {price}, {result}");
        }
        return result;
    }

    public void EnterbuildMode(){
        DropItem();
        stats.isBuilding = true;
    }

    public void DropItem(){
        stats.hasItem = false;

        // do other stuff relating to dropping your item
    }

    public bool isBusyCheck()
    {
        return stats.isBusy;
    }

    public void flipYapping()
    {
        stats.isYapping = !stats.isYapping;
        flipBusy();

    }

    public void flipBusy()
    {
        stats.isBusy = !stats.isBusy;
    }

}
