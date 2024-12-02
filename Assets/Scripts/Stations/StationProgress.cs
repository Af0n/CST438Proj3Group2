using UnityEngine;
using UnityEngine.UI;

public class StationProgress : MonoBehaviour
{
    public Image image;
    private Workstation stationScript;
    private Brewing brewingScript;

    private int duration;
    private int current;
    
    private bool IsCauldron{
        get { return brewingScript != null; }
    }

    public float Progress{
        get { return (float)current / (float)duration; }
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

        if(duration <= 0){
            duration = 1;
        }
    }

    private void Update() {
        if(IsCauldron){
            current = brewingScript.TickTimer;
        }else{
            current = stationScript.TickTimer;
        }

        image.fillAmount = Progress;
    }
}
