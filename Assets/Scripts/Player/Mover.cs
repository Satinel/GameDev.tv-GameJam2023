using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _blendTime = 0.1f;
    [SerializeField] float _rotationSpeed = 15f;
    [SerializeField] Canvas _interactCanvas;
    [SerializeField] LevelManager _levelManager;

    Transform _mainCamera;
    float _verticalVelocity;
    float _runSpeed;
    float _walkSpeed;
    Vector2 _movementValue;
    bool _isWalking = false;
    bool _isHiding = false;
    bool _isDisabled = false;
    PlayerControls _controls;
    CharacterController _characterController;
    PlayerHealth _playerHealth;
    Animator _animator;

    readonly int BLEND_HASH = Animator.StringToHash("MoveBlend");
    readonly int HIDE_HASH = Animator.StringToHash("Hide");

    void Awake()
    {
        _controls = new PlayerControls();
        _characterController = GetComponent<CharacterController>();
        _playerHealth = GetComponentInChildren<PlayerHealth>();
        _animator = GetComponentInChildren<Animator>();
        _runSpeed = _moveSpeed;
        _walkSpeed = _moveSpeed/2;
    }

    void OnEnable()
    {
        _controls.Player.Enable();
        _playerHealth.OnPlayerDefeat += DisableControl;
        _levelManager.OnLevelCompleted += DisableControl;
    }

    void OnDisable()
    {
        _controls.Player.Disable();
        _playerHealth.OnPlayerDefeat -= DisableControl;
        _levelManager.OnLevelCompleted -= DisableControl;
    }

    void Start()
    {
        _mainCamera = Camera.main.transform;
        _controls.Player.ToggleWalk.started += _ => ToggleWalk();
        _controls.Player.Interact.performed += _ => HideInteractable();
    }

    void Update()
    {
        SetGravity();
        PlayerInput();
    }

    void SetGravity()
    {
        if(!_characterController.isGrounded)
        {
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        else if(_verticalVelocity < 0f)
        {
            _verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
    }

    void PlayerInput()
    {
        if(_isHiding || _isDisabled)
        {
            return;
        }

        _movementValue = _controls.Player.Move.ReadValue<Vector2>();
        Vector3 movement = CalculateMovement();

        if (_isWalking) // I should be able to clean up the if/else's but gamejam time frame so leaving it messy
        {
            _characterController.Move(((movement * _walkSpeed) + new Vector3(0, _verticalVelocity, 0)) * Time.deltaTime);
        }
        else
        {
            _characterController.Move(((movement * _runSpeed) + new Vector3(0, _verticalVelocity, 0)) * Time.deltaTime);
        }

        if (_movementValue == Vector2.zero)
        {
            _animator.SetFloat(BLEND_HASH, 0, _blendTime, Time.deltaTime);
            return;
        }

        FaceMovementDirection(movement);

        if (_isWalking)
        {
            _animator.SetFloat(BLEND_HASH, _movementValue.magnitude / 2, _blendTime, Time.deltaTime);
        }
        else
        {
            _animator.SetFloat(BLEND_HASH, _movementValue.magnitude, _blendTime, Time.deltaTime);
        }
    }

    void DisableControl()
    {
        _isDisabled = true;
    }

    Vector3 CalculateMovement()
    {
        Vector3 cameraRight = _mainCamera.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        Vector3 cameraForward = _mainCamera.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        return cameraRight * _movementValue.x + cameraForward * _movementValue.y;
    }

    void FaceMovementDirection(Vector3 movement)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), _rotationSpeed * Time.deltaTime);
    }

    void ToggleWalk()
    {
        _isWalking = !_isWalking;
        if(_isWalking)
        {
            _moveSpeed = _walkSpeed;
        }
    }

    public void SetIsHiding(bool value)
    {
        _isHiding = value;
        _animator.SetBool(HIDE_HASH, value);
    }

    public void ShowInteractable()
    {
        //TODO play notification sound if you have one
        _interactCanvas.enabled = true;
    }

    public void HideInteractable()
    {
        _interactCanvas.enabled = false;
    }
}
