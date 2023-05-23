using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Squisher : MonoBehaviour
{
    [SerializeField] float _squishAmount = 0.01f;
    [SerializeField] float _squishRoutineAmount = 0.1f;
    [SerializeField] float _coroutineDuration = 0.8f;
    [SerializeField] float _radiusSideSquishAmount = 0.05f;
    [SerializeField] float _radiusVertSquishAmount = 0.07f;
    [SerializeField] CharacterController _charaCont;
    [SerializeField] Mover _mover;
    // [SerializeField] CinemachineCollider _cineCollider;
    float _heightDefault;
    Vector3 _centerDefault;
    Vector3 _previousPosition;
    Quaternion _previousRotation;
    float _zCenterOffset = 0f;
    float _radiusDefault;
    float _stepOffsetDefault;
    float _squishedStepOffset = 0.03f;
    float _yCenteredSquishAmount = 0.1f;
    bool _isSquished = false;
    bool _isSquishing = false;
    bool _inTightSpace = false;
    bool _isHiding = false;
    float _routineDelta = 0f;
    PlayerControls _controls;
    const string _tightSpace = "TightSpace";

    void Awake()
    {
        _controls = new PlayerControls();
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
        _heightDefault = _charaCont.height;
        _centerDefault = new Vector3(_charaCont.center.x, _charaCont.center.y, _charaCont.center.z);
        _radiusDefault = _charaCont.radius;
        _stepOffsetDefault = _charaCont.stepOffset;
        _controls.Player.XSquish.performed += _ => SideSquish();
        _controls.Player.YSquish.performed += _ => VerticalSquish();
        _controls.Player.ZSquish.performed += _ => FrontSquish();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(_tightSpace))
        {
            _inTightSpace = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(_tightSpace))
        {
            _inTightSpace = false;
        }
    }

    void SideSquish()
    {
        if(_isSquishing || _isHiding) { return; }

        if(_isSquished)
        {
            Unsquish();
            return;
        }
        else
        {
            _isSquishing = true;
            _routineDelta = 0f;
            //TODO Play a sound here
            StartCoroutine(SideRoutine());
        }
    }

    IEnumerator SideRoutine()
    {
        while(_routineDelta < _coroutineDuration)
        {
            transform.localScale = new Vector3(transform.localScale.x - _squishRoutineAmount, 1f, 1f);
            _routineDelta += 0.05f;
            yield return new WaitForEndOfFrame();
        }
        _isSquishing = false;
        _isSquished = true;
        transform.localScale = new Vector3(_squishAmount, 1f, 1f);
        _charaCont.radius *= _radiusSideSquishAmount;
        _charaCont.center = new Vector3(_centerDefault.x, _centerDefault.y, _zCenterOffset);
        // _cineCollider.enabled = false;
    }

    void VerticalSquish()
    {
        if(_isSquishing || _isHiding) { return; }

        if(_isSquished)
        {
            Unsquish();
            return;
        }
        else
        {
            _isSquishing = true;
            _routineDelta = 0f;
            //TODO Play a sound here
            StartCoroutine(HeightRoutine());
        }
    }

    IEnumerator HeightRoutine()
    {
        while(_routineDelta < _coroutineDuration)
        {
            transform.localScale = new Vector3(1f, transform.localScale.y - _squishAmount, 1f);
            _routineDelta += 0.0021f;
            yield return new WaitForEndOfFrame();
        }
        _isSquishing = false;
        _isSquished = true;
        transform.localScale = new Vector3(1f, _squishAmount, 1f);
        _charaCont.stepOffset = _squishedStepOffset;
        _charaCont.height *= _squishAmount;
        _charaCont.center = new Vector3(_centerDefault.x, _yCenteredSquishAmount, _centerDefault.z);
        _charaCont.radius = _radiusVertSquishAmount;
        // _cineCollider.enabled = false;
    }

    void FrontSquish()
    {
        if(_isSquishing || _isHiding) { return; }

        if(_isSquished)
        {
            Unsquish();
            return;
        }
        else
        {
            _isSquished = true;
            transform.localScale = new Vector3(1f, 1f, _squishAmount);
            // _cineCollider.enabled = false;
        }
    }

    void Unsquish()
    {
        if(_inTightSpace || _isHiding) { return; }

        //TODO play a POP! sort of sound

        transform.localScale = Vector3.one;
        _charaCont.height = _heightDefault;
        _charaCont.center = _centerDefault;
        _charaCont.radius = _radiusDefault;
        _charaCont.stepOffset = _stepOffsetDefault;
        _isSquished = false;
        // _cineCollider.enabled = true;
    }

    public void HideInPainting(Transform painting)
    {
        if(_isHiding)
        {
            transform.position = _previousPosition;
            transform.rotation = _previousRotation;
            _mover.SetIsHiding(false);
            _isHiding = false;
            Unsquish();
            return;
        }

        StopAllCoroutines();
        _isSquishing = false;

        if(_isSquished)
        {
            transform.localScale = Vector3.one;
            _charaCont.height = _heightDefault;
            _charaCont.center = _centerDefault;
            _charaCont.radius = _radiusDefault;
            _charaCont.stepOffset = _stepOffsetDefault;
            _isSquished = false;
        }

        _mover.SetIsHiding(true);
        _previousPosition = transform.position;
        _previousRotation = transform.rotation;
        FrontSquish();
        transform.position = painting.position;
        transform.rotation = painting.rotation;
        _isHiding = true;
    }
}
