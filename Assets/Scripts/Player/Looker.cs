using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looker : MonoBehaviour
{
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

    void Update()
    {
        
    }
}
