using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _blendTime = 0.1f;

    float _runSpeed;
    float _walkSpeed;
    Vector2 _movementValue;
    bool _isWalking = false;
    PlayerControls _controls;
    CharacterController _characterController;
    Animator _animator;

    readonly string BLEND_HASH = "MoveBlend";


    void Awake()
    {
        _controls = new PlayerControls();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _runSpeed = _moveSpeed;
        _walkSpeed = _moveSpeed/2;
    }

    void OnEnable()
    {
        _controls.Player.Enable();
    }

    void OnDisable()
    {
        _controls.Player.Disable();
    }

    void Start()
    {
        _controls.Player.ToggleWalk.started += _ => ToggleWalk();
    }

    void Update()
    {
        PlayerInput();
    }

    void PlayerInput()
    {
        _movementValue = _controls.Player.Move.ReadValue<Vector2>();
        Vector3 movement = new Vector3();
        movement.x = _movementValue.x;
        movement.y = 0;
        movement.z = _movementValue.y;
        if(_isWalking) // I should be able to clean up the if/else's but gamejam time frame so leaving it messy
        {
            _characterController.Move(movement * _walkSpeed * Time.deltaTime);
        }
        else
        {
            _characterController.Move(movement * _runSpeed * Time.deltaTime);
        }

        if(_movementValue == Vector2.zero)
        { 
            _animator.SetFloat(BLEND_HASH, 0, _blendTime, Time.deltaTime);
            return; 
        }

        transform.rotation = Quaternion.LookRotation(movement);
        if(_isWalking)
        {
            _animator.SetFloat(BLEND_HASH, _movementValue.magnitude/2, _blendTime, Time.deltaTime);
        }
        else
        {
            _animator.SetFloat(BLEND_HASH, _movementValue.magnitude, _blendTime, Time.deltaTime);
        }
    }

    void ToggleWalk()
    {
        _isWalking = !_isWalking;
        if(_isWalking)
        {
            _moveSpeed = _walkSpeed;
        }
    }
}
