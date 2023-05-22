using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] Transform[] _doors;
    [SerializeField] Material _activateMaterial;
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] float _anglesPerSecond = 180;
    [SerializeField] float _rotationDuration = 1f;
    [SerializeField] LeverCutscene _cutscene;

    MeshRenderer _renderer;
    bool _hasActivated = false;
    PlayerControls _controls;

    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _controls = new PlayerControls();
    }

    void OnDisable()
    {
        _controls.Player.Disable();
    }

    void OnTriggerEnter(Collider other)
    {
        if(_hasActivated) { return; }

        if(other.CompareTag("Player"))
        {
            _controls.Player.Enable();
            _controls.Player.Interact.performed += _ => OpenDoors();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(_hasActivated) { return; }

        if(other.CompareTag("Player"))
        {
            _controls.Player.Disable();
        }
    }

    void OpenDoors()
    {
        if(_hasActivated) { return; }

        if(_cutscene != null)
        {
            _cutscene.EnableCamera();
        }

        StartCoroutine(Rotate());
        _hasActivated = true;
        _controls.Player.Disable();
        _renderer.material = _activateMaterial;
        //TODO Play a sound!
        foreach (Transform door in _doors)
        {
            StartCoroutine(RaiseDoor(door));
        }
    }

    IEnumerator Rotate()
    {
        float startTime = 0f;
        while(startTime < _rotationDuration)
        {
            startTime += Time.deltaTime;
            Vector3 rotation = transform.localEulerAngles;
            rotation.z -= _anglesPerSecond * Time.deltaTime;
            transform.localEulerAngles = rotation;
            yield return null;
        }
    }

    IEnumerator RaiseDoor(Transform door)
    {
        //TODO Start another sound
        while(door.localPosition.y < 4f)
        {
            door.localPosition += new Vector3(0f, 0.1f, 0f) * _moveSpeed * Time.deltaTime;
            yield return null;
        }
        //TODO Stop that other sound
    }

}
