using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSide : MonoBehaviour
{
    [Header("Unity Setup")]
    [Tooltip("The center of the interaction sphere")]
    public Transform interactionCheck;

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
        }
        interactionCheck.localPosition = Vector3.zero + move3;
    }
}
