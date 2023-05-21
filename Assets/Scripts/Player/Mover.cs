using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _blendTime = 0.1f;
    [SerializeField] float _rotationSpeed = 15f;

    Transform _mainCamera;
    float _verticalVelocity;
    float _runSpeed;
    float _walkSpeed;
    Vector2 _movementValue;
    bool _isWalking = false;
    PlayerControls _controls;
    CharacterController _characterController;
    Animator _animator;

    readonly int BLEND_HASH = Animator.StringToHash("MoveBlend");


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
        _mainCamera = Camera.main.transform;
        _controls.Player.ToggleWalk.started += _ => ToggleWalk();
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
        // transform.position = new Vector3(transform.position.x, 0, transform.position.z); // TODO check if this prevents going up slopes/stairs

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
}
