using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squisher : MonoBehaviour
{
    [SerializeField] CharacterController _charaCont;
    [SerializeField] float _squishAmount = 0.01f;
    float _heightDefault;
    Vector3 _centerDefault;
    float _radiusDefault;
    bool _isSquished = false;
    PlayerControls _controls;

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

        _controls.Player.XSquish.performed += _ => SideSquish();
        _controls.Player.YSquish.performed += _ => VerticalSquish();
        _controls.Player.ZSquish.performed += _ => FrontSquish();
    }

    void SideSquish()
    {
        if(_isSquished)
        {
            Unsquish();
            return;
        }
        else
        {
            _isSquished = true;
            transform.localScale = new Vector3(_squishAmount, 1f, 1f);
            _charaCont.radius *= _squishAmount;
        }
    }

    void VerticalSquish()
    {
        if(_isSquished)
        {
            Unsquish();
            return;
        }
        else
        {
            _isSquished = true;
            transform.localScale = new Vector3(1f, _squishAmount, 1f);
            transform.localPosition = new Vector3(0, _squishAmount, 0);
            _charaCont.height *= _squishAmount;
            _charaCont.center = new Vector3(_centerDefault.x, _centerDefault.y * _squishAmount, _centerDefault.z);
        }
    }

    void FrontSquish()
    {
        if(_isSquished)
        {
            Unsquish();
            return;
        }
        else
        {
            _isSquished = true;
            transform.localScale = new Vector3(1f, 1f, _squishAmount);
        }
    }

    private void Unsquish()
    {
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        _charaCont.height = _heightDefault;
        _charaCont.center = _centerDefault;
        _charaCont.radius = _radiusDefault;
        _isSquished = false;
    }
}
