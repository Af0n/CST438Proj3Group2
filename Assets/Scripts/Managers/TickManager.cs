using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickSystem : MonoBehaviour
{
    // public delegate void onHalfSec();
    // public static event onHalfSec onHalfSecAction;
    // public delegate void onSec();
    // public static event onSec onSecAction;
    // public delegate void on2Sec();
    // public static event on2Sec on2SecAction;

    // private double _tickTimer;
    // private int _tick = 0;

    private const float _MAX_TICK = 0.1f; // This would allow for ticks to be 1/10 of a second.. 
    // Todo: add a queue system.. 
    
    // void Update() {
    //     _tickTimer += Time.deltaTime;
        
    //     // every MAX_TICK seconds, add a tick
    //     if(_tickTimer >= _MAX_TICK) {
    //         _tickTimer = 0;
    //         _tick += 1; 
    //     }

    //     Debug.Log("Tick:" + _tick);
    //     Debug.Log("Mod 10: " + _tick % 10);

    //     if (_tick % 5 == 0) {
    //         Debug.Log("Do Half Tick");
    //         // Half a second..
    //         onHalfSecAction?.Invoke();
    //     }

    //     if(_tick % 10 == 0) {
    //         Debug.Log("Do Second Tick");
    //         // every second.. 
    //         onSecAction?.Invoke();
    //     }

    //     if(_tick % 20 == 0) {
    //         Debug.Log("Do 2 second Tick");
    //         // Every 2 seconds
    //         on2SecAction?.Invoke();
    //     }
    // }

    public delegate void onTick();
    // SUBSCRIBE TO THIS, TO JOIN TICK SYSTEM
    public static event onTick OnTick;

    private void Start() {
        StartCoroutine("BroadcastTick");
    }

    private IEnumerator BroadcastTick(){
        while(true){
            yield return new WaitForSeconds(_MAX_TICK);
            OnTick?.Invoke();
        }
    }
}