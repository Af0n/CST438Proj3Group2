using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Workstation : MonoBehaviour
{
    [Tooltip("Determines the type of workstation")]
    public StationTypes type;

    [Header("Unity Setup")]
    public StationSprites stationSprites;
    
    private SpriteRenderer spriteRenderer;
    private int typeCode;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetStation(type);
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
