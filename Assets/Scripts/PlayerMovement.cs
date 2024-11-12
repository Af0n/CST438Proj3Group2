using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// Alexander Betancourt: 2D player Controller from Cubed
/// Summary: Makes a basic 2D player Controller for player movement.
/// Not sure who wrote the orginal code (Angel?), but I adapted it for most projects that use a 2D controller
/// See orginal code: https://github.com/XOR-SABER/Cubed-Arcade/blob/main/CubedPrime/Assets/Scripts/Player/PlayerMovement.cs

public class PlayerMovement : MonoBehaviour
{
    //-=======================================-
    // Public
    [Tooltip("A refrence to player stats object")]
    public PlayerStats stats;
    [Tooltip("Current Player sprite to move")]
    public GameObject playerSprite;
    //-=======================================-
    // Private
    private Camera _cam;
    private Vector2 _movement_vec;
    private Vector2 _current_velocity;
    private Rigidbody2D _rb;
    private float _move_speed;
    private float _current_move_speed;
    private float _deceleration;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (stats == null)
        {
            Debug.LogError("Error in PlayerMovement: No playerStats attached");
        }
    }

    private void Start()
    {
        _move_speed = stats.MoveSpeed;
        _deceleration = stats.MoveDeceleration;
        _current_move_speed = _move_speed;
        _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void move(Vector2 moveRead)
    {
        _movement_vec = moveRead;
    }
    private void FixedUpdate()
    {
        fixedMove();
    }

    // Fixed update since we are using RB physics.
    public void fixedMove()
    {
        Vector2 targetVelocity = _movement_vec * _current_move_speed;
        if (_movement_vec.magnitude == 0f)
        {
            _current_velocity = Vector2.LerpUnclamped(_current_velocity, Vector2.zero, _deceleration * Time.fixedDeltaTime);
        }
        else
        {
            _current_velocity = Vector2.LerpUnclamped(_current_velocity, targetVelocity, Time.fixedDeltaTime * 10f);
        }

        _rb.MovePosition(_rb.position + _current_velocity * Time.fixedDeltaTime);
    }
}