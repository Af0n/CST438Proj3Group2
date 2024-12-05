using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    //-=======================================-
    // Public
    [Tooltip("A refrence to player stats object")]
    public PlayerStats stats;

    public PlayerMovement movement;
    private PauseMenu _pause_menu;
    public enum Input{
        Keyboard, 
        Controller,
        Mobile,
    }
    public Input inputType;
    private InputAction _move; 
    private InputAction _pause; 
    private InputAction _action;
    private PlayerControls _controls;
    private void OnEnable() => EnableControls();
    private void OnDisable()=> DisableControls();

    public InteractionSide interaction;

    // Private bool that lets the game know if you're currently interacting with something
    private bool currentlyInteracting = false;

    // private bool that waits for a second as to not speed through dialogue
    private bool justStartedDia = false;

    public void EnableControls() {
        _move.Enable();
        _pause.Enable();
        _action.Enable();
    }

    public void DisableControls() {
        _move.Disable();
        _pause.Disable();
        _action.Disable();
    }

    private void Awake() {
        _controls = new PlayerControls();
        _move = _controls.Movement.Move;
        _action = _controls.Movement.Action;
        _pause = _controls.Movement.Pause;

        _pause.performed += _ => pause();
        _action.performed += _ => action();

        _action.started += _ => flipAction();
        _action.canceled += _ => flipAction();

    }
    private void Start() {
        _pause_menu = GameObject.FindWithTag("PauseMenu").GetComponent<PauseMenu>();

        stats.isBusy = false;
        stats.isYapping = false;

    }

    public void Update() {
        movement.move(_move.ReadValue<Vector2>());
    }

    public void pause() {
        Debug.Log("Pause the game homie");
        _pause_menu.togglePause();
    }

    public void action() {
        // will only check for interactions if the player is not busy
        if (!stats.isBusy)
        {
            interaction.CheckInteraction();
        }
    }

    // flips if the player is currently interacting and will also give them some buffer time
    private void flipAction()
    {
        currentlyInteracting = !currentlyInteracting;

        if (!currentlyInteracting)
        {
            StartCoroutine(waitForHuman());
        }

    }

    public bool actionTriggered()
    {
        return currentlyInteracting;
    }

    // Waits for a second
    IEnumerator waitForHuman ()
    {

        if (justStartedDia)
        {
            yield break;
        }

        justStartedDia = true;
        yield return new WaitForSeconds(.25f);
        justStartedDia = false;
    }

    // Checks to see if the player wants to progress dialogue but does a wait check
    public bool dialogueInputCheck()
    {
        if (actionTriggered())
        {
            if (justStartedDia)
            {
                return false;
            }
            return true;
        }
        return false;
    }

}
