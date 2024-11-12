using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Workstation : MonoBehaviour
{
    [Header("Station Stats")]
    [Tooltip("Determines the type of workstation")]
    public StationTypes type;
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

        StartCoroutine("TestSpawnCycle");
    }

    public void Tick(){
        tickTimer--;
    }

    public void StartProcessing(Item i){

    }

    public void SetStation(StationTypes t)
    {
        typeCode = GetStationTypeCode(t);
        spriteRenderer.sprite = stationSprites.sprites[typeCode];
    }

    // gets the numerical value for the type of station
    public static int GetStationTypeCode(StationTypes t){
        switch(t){
            case StationTypes.GRINDSTONE:
                return 0;
            case StationTypes.MIXING:
                return 1;
            case StationTypes.CLEANSING:
                return 2;
            default:
                Debug.Log("Could Not Find Type");
                return -1;
        }
    }
}
