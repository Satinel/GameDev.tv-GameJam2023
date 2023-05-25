using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portrait : MonoBehaviour
{
    [SerializeField] Transform _hidingPosition;
    [SerializeField] GameObject _portraitCamera;

    PlayerControls _controls;
    Squisher _squisher;

    void Awake()
    {
        _controls = new PlayerControls();
    }

    void OnDisable()
    {
        _controls.Player.Disable();
    }

    void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<Squisher>())
        {
            _squisher = other.GetComponent<Squisher>();

            _controls.Player.Enable();
            _controls.Player.Interact.performed += _ => Hide();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Squisher>())
        {
            _controls.Player.Disable();
        }
    }

    void Hide()
    {
        _squisher.HideInPainting(_hidingPosition, _portraitCamera);
        // _portraitCamera.SetActive(!_portraitCamera.activeSelf);
    }
}
