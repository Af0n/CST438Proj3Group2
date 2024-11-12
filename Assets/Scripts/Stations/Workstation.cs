using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Workstation : MonoBehaviour
{
    [Header("Station Stats")]
    [Tooltip("Determines the type of workstation")]
    public StationType type;
    [Tooltip("Number of ticks it takes to process an ingredient")]
    public int processTime;

    [Header("Unity Setup")]
    public StationSprites stationSprites;

    [Header("Testing")]
    [Tooltip("Used for testing while tick system isn't implemented. Set to -1 to disable fake ticks.")]
    public float testTickTime;
    
    // the child object that determines where the item will be 'held'
    private Transform itemPos;
    private SpriteRenderer spriteRenderer;
    private int typeCode;
    private int tickTimer;

    public bool HasItem{
        get{ return itemPos.childCount == 1; }
    }

    public bool IsProcessing{
        get{ return tickTimer != 0; }
    }

    private void Awake() {
        tickTimer = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetStation(type);
    }

    private void Start() {
        // check if we should do test cycling
        if (testTickTime < 0)
        {
            return;
        }

        StartCoroutine("TestProcessCycle");
    }

    public void Tick(){
        tickTimer--;
    }

    // assumes the item is processable at this station
    public void ProcessItem(Item item){
        switch(type){
            case StationType.GRINDSTONE:
                Grind(item);
                return;
            case StationType.MIXING:
                Mix(item);
                return;
            case StationType.CLEANSING:
                Clean(item);
                return;
            default:
                Debug.Log("Could Not Process Item");
                return;
        }
    }

    private void Grind(Item item){
        switch(item.Type){
            case ItemType.SPARKLING_RUBY:
                item.ChangeItem(ItemType.GEMSTONE_DUST);
                return;
            default:
                Debug.Log("Cannot Grind Item");
                return;
        }
    }

    private void Grind(Item item){
        switch(item.Type){
            case ItemType.SPARKLING_RUBY:
                item.ChangeItem(ItemType.GEMSTONE_DUST);
                return;
            default:
                Debug.Log("Cannot Grind Item");
                return;
        }
    }

    public void SetStation(StationType t)
    {
        typeCode = GetStationTypeCode(t);
        spriteRenderer.sprite = stationSprites.sprites[typeCode];
    }

    // gets the numerical value for the type of station
    public static int GetStationTypeCode(StationType t){
        switch(t){
            case StationType.GRINDSTONE:
                return 0;
            case StationType.MIXING:
                return 1;
            case StationType.CLEANSING:
                return 2;
            default:
                Debug.Log("Could Not Find Type");
                return -1;
        }
    }
}
