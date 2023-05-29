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
    [SerializeField] bool _opensDoors;
    [SerializeField] AudioClip _audioClip;
    AudioSource _audioSource;

    MeshRenderer _renderer;
    bool _hasActivated = false;
    PlayerControls _controls;
    Animator _playerAnimator;
    Interactable _interactable;
    readonly int INTERACT_HASH = Animator.StringToHash("Interact");

    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _controls = new PlayerControls();
        _interactable = GetComponent<Interactable>();
        _audioSource = GetComponent<AudioSource>();
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
            _playerAnimator = other.GetComponentInChildren<Animator>();
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

        if(_playerAnimator != null)
        {
            _playerAnimator.SetTrigger(INTERACT_HASH);
        }

        StartCoroutine(Rotate());
        _hasActivated = true;
        _controls.Player.Disable();
        _renderer.material = _activateMaterial;
        _audioSource.PlayOneShot(_audioClip);
        _interactable._IsActive = false;
        
        if(_opensDoors)
        {
            foreach(Transform door in _doors)
            {
                door.GetComponentInChildren<Animator>().SetTrigger(INTERACT_HASH);
                door.GetComponentInChildren<AudioSource>().Play();
            }
            return;
        }
        else
        {
            foreach (Transform door in _doors)
            {
                StartCoroutine(RaiseDoor(door));
            }
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
        AudioSource doorAudio = door.GetComponent<AudioSource>();
        doorAudio.Play();
        while(door.localPosition.y < 4f)
        {
            door.localPosition += new Vector3(0f, 0.1f, 0f) * _moveSpeed * Time.deltaTime;
            yield return null;
        }
        doorAudio.Stop();
    }

}
