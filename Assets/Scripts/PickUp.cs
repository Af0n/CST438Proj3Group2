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
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        Debug.Log(transform.name + " has been picked up by " + p.name);
    }

    public void Drop(){
        transform.parent = null;

        if(transform.CompareTag("Item"))
        {
            GetComponent<Item>().Drop();
        }

        Debug.Log(transform.name + " has been dropped");
    }
}
