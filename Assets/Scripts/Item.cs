using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType type;

    public bool isHeld;

    public void PickUp(){
        isHeld = true;
    }

    public void Drop(){
        isHeld = false;
    }
}
