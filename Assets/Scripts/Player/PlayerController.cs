using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
    }
    private void Start() {
        _pause_menu = GameObject.FindWithTag("PauseMenu").GetComponent<PauseMenu>();
    }

    public void Update() {
        movement.move(_move.ReadValue<Vector2>());
    }

    public void pause() {
        Debug.Log("Pause the game homie");
        _pause_menu.togglePause();
    }

    public void action() {
        Debug.Log("Do something homie");
    }
}
