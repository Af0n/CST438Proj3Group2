using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickSystem : MonoBehaviour
{
    public delegate void onHalfSec();
    public static event onHalfSec onHalfSecAction;
    public delegate void onSec();
    public static event onSec onSecAction;
    public delegate void on2Sec();
    public static event on2Sec on2SecAction;
    private double _tickTimer;
    private int _tick = 0;
    private const float _MAX_TICK = 0.1f; // This would allow for ticks to be 1/10 of a second.. 
    // Todo: add a queue system.. 
    void Update() {
        _tickTimer += Time.deltaTime;
        if(_tickTimer >= _MAX_TICK) {
            _tickTimer = 0;
            _tick += 1; 
        }

        if (_tick % 5 == 0) {
            // Half a second..
            onHalfSecAction?.Invoke();
        }

        if(_tick % 10 == 0) {
            // every second.. 
            onSecAction?.Invoke();
        }

        if(_tick % 20 == 0) {
            // Every 2 seconds
            on2SecAction?.Invoke();
        }
    }

    
}