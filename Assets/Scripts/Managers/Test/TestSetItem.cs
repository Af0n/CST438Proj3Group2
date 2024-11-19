using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSetItem : MonoBehaviour
{
    public Workstation testStation;
    public Transform testItem;
    public bool debug;

    private void Start() {
        if(debug){
            testStation.SetItem(testItem);
        }
    }
}
