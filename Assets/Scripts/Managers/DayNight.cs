using System.Collections;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    [Tooltip("How many ticks a day is. Remember, 10 ticks per second")]
    public int dayLength;

    // tick counter for a day
    private int clock;
    // number day you are on
    private int day;
    // how long half a day is
    private int halfDay;

    public bool IsDay{
        get { return clock < halfDay; }
    }

    public float PercentDay{
        get { return (float)clock / (float)dayLength; }
    }

    private void OnEnable() {
        TickSystem.OnTick += Tick;
    }

    private void OnDisable() {
        TickSystem.OnTick -= Tick;
    }

    private void Awake() {
        halfDay = dayLength / 2;
    }

    private void Tick(){
        clock++;

        if(clock >= dayLength){
            day++;
            clock = 0;
        }

        // Debug.Log($"Day {day}, IsDay {IsDay}, PercentDay {PercentDay}");
    }
}
