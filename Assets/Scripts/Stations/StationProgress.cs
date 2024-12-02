using UnityEngine;

public class StationProgress : MonoBehaviour
{
    private Workstation stationScript;
    private Brewing brewingScript;

    private int duration;
    private int current;
    
    private bool IsCauldron{
        get { return brewingScript != null; }
    }

    public float Progress{
        get { return current / duration; }
    }

    private void Awake() {
        stationScript = GetComponent<Workstation>();
        brewingScript = GetComponent<Brewing>();

        // grab appropriate maxTime
        if(IsCauldron){
            duration = brewingScript.processTime;
        }else{
            duration = stationScript.processTime;
        }
    }

    private void Update() {
        if(IsCauldron){
            current = brewingScript.TickTimer;
        }else{
            current = stationScript.TickTimer;
        }
    }
}
