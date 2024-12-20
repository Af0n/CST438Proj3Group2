using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSide : MonoBehaviour
{
    [Tooltip("Interaction Radius")]
    public float radius;
    [Tooltip("Interaction Center Distance")]
    public float distance;
    [Header("Unity Setup")]
    [Tooltip("A refrence to player stats object")]
    public PlayerStats stats;
    [Tooltip("The transform to parent when picking up")]
    public Transform itemPos;
    [Tooltip("Layer to check on for pickup")]
    public LayerMask pickupMask;
    [Tooltip("Layer to check on for interaction")]
    public LayerMask interactMask;
    

    private PlayerMovement movement;
    private Vector3 move3;

    private void Awake() {
        movement = GetComponent<PlayerMovement>();
    }

    private void Update() {
        Vector2 move2 = movement.GetMovementVector();
        if(move2 != Vector2.zero){
            move3 = new Vector3(move2.x, move2.y, 0);
            move3.Normalize();
            move3 = move3 * distance;
        }
        itemPos.localPosition = Vector3.zero + move3;
    }

    public void CheckInteraction(){
        if(stats.hasItem){
            // go in here if player has an item
            if(TryPlaceItem()){
                return;
            }

            // go here if no place to put item
            ItemDrop();
            return;
        }

        if(TryItemPickup()){
            return;
        }

        if (TryCauldronInteract())
        {
            return;
        }

        if (TryDialogueInteract())
        {
            return;
        }

    }

    private bool TryCauldronInteract(){
        Collider2D[] objs = Physics2D.OverlapCircleAll(itemPos.position, radius, interactMask);

        // if there are no nearby stations, quit
        if(objs.Length == 0){
            return false;
        }

        foreach (Collider2D col in objs)
        {
            // check if the interactable is the cauldron
            Brewing brew = col.GetComponent<Brewing>();
            if(brew == null){
                continue;
            }

            // if we get here, then we've selected the cauldron

            brew.TryCycleRecipe();
            return true;
        }

        return false;
    }

    // Will attempt to interact with a nearby object if the correcrt script is attached
    private bool TryDialogueInteract()
    {
        Collider2D[] objs = Physics2D.OverlapCircleAll(itemPos.position, radius, interactMask);

        // if there are no nearby stations, quit
        if (objs.Length == 0)
        {
            return false;
        }

        foreach (Collider2D col in objs)
        {
            // check if the interactable is the cauldron
            Dialogue dialo = col.GetComponent<Dialogue>();
            if (dialo == null)
            {
                continue;
            }

            // if we get here, then we've selected the cauldron

            dialo.RunDialogue();
            return true;
        }

        return false;
    }

    private bool TryItemPickup(){
        Collider2D[] objs = Physics2D.OverlapCircleAll(itemPos.position, radius, pickupMask);

        // if there are no nearby items to pickup, quit
        if(objs.Length == 0){
            return false;
        }

        stats.hasItem = true;

        // default value that will guarantee at least one selected item
        float dist = 100;
        Collider2D closestItem = null;

        // finding closest item to PLAYER
        foreach (Collider2D obj in objs)
        {
            if(Vector2.Distance(transform.position, obj.transform.position) < dist){
                closestItem = obj;
            }
        }

        // we have found the closest item
        // call pickup on that item
        closestItem.GetComponent<PickUp>().Pick(itemPos);

        return true;
    }

    private bool TryPlaceItem(){
        Collider2D[] objs = Physics2D.OverlapCircleAll(itemPos.position, radius, interactMask);

        // if there are no nearby stations, quit
        if(objs.Length == 0){
            return false;
        }

        // default value that will guarantee at least one selected station
        float dist = 100;
        Collider2D closestItem = null;

        // finding closest station to PLAYER
        foreach (Collider2D obj in objs)
        {
            if(Vector2.Distance(transform.position, obj.transform.position) < dist){
                closestItem = obj;
            }
        }

        // we have found the closest station
        // call SetItem on that station
        bool success = closestItem.GetComponent<Workstation>().SetItem(itemPos.GetChild(0));

        if(!success){
            return false;
        }

        // successfully set item
        stats.hasItem = false;

        return true;
    }

    private void ItemDrop(){
        stats.hasItem = false;
        itemPos.GetChild(0).GetComponent<PickUp>().Drop(transform);
    }
}