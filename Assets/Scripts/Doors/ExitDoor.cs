using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    bool _hasActivated = false;
    PlayerControls _controls;
    Animator _myAnimator;
    Animator _playerAnimator;
    Interactable _interactable;
    readonly int INTERACT_HASH = Animator.StringToHash("Interact");
    AudioSource _audioSource;
    [SerializeField] AudioClip _audioClip;

    void Awake()
    {
        _controls = new PlayerControls();
        _interactable = GetComponent<Interactable>();
        _myAnimator = GetComponent<Animator>();
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
            _controls.Player.Interact.performed += _ => OpenDoor();
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

    void OpenDoor()
    {
        if(_hasActivated) { return; }
        
        if(_playerAnimator != null)
        {
            _playerAnimator.SetTrigger(INTERACT_HASH);
        }

        _myAnimator.SetTrigger(INTERACT_HASH);
        _hasActivated = true;
        _controls.Player.Disable();
        _audioSource.PlayOneShot(_audioClip);
        _interactable._IsActive = false;
    }
}
