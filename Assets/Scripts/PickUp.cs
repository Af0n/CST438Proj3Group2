using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public void Pick(Transform p){
        transform.parent = p;
        transform.localPosition = Vector3.zero;

        if(transform.CompareTag("Item"))
        {
            GetComponent<Item>().PickUp();
        }

        Debug.Log(transform.name + " has been picked up by " + p.name);
    }
}
