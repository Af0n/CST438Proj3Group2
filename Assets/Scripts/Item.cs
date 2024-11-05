using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSprites sprites;

    public ItemType type;

    public bool isHeld;

    private void Awake() {
        // set sprite
    }

    public void PickUp(){
        isHeld = true;
    }

    public void Drop(){
        isHeld = false;
    }
}
