using System.Collections;
using UnityEngine;
using Cinemachine;

public class Squisher : MonoBehaviour
{
    [SerializeField] float _squishAmount = 0.01f;
    [SerializeField] float _squishRoutineAmount = 0.1f;
    [SerializeField] float _coroutineDuration = 0.8f;
    [SerializeField] float _radiusSideSquishAmount = 0.05f;
    [SerializeField] float _radiusVertSquishAmount = 0.07f;
    // [SerializeField] float _heightDelta = 0.0021f; //This is probably a terrible way to do things regardless
    [SerializeField] CharacterController _charaCont;
    [SerializeField] Mover _mover;
    [SerializeField] Hider _hider;
    [SerializeField] CinemachineCollider _cineCollider;
    [SerializeField] CinemachineFreeLook _camera;
    [SerializeField] CutsceneManager _cutsceneManager;
    [SerializeField] AudioClip _squishAudioClip;
    [SerializeField] AudioClip _unSquishAudioClip;
    [SerializeField] Canvas _canSquishCanvas;
    [SerializeField] MaterialSwapper[] _swappers;
    [SerializeField] Transform _defaultCamFocus;
    // [SerializeField] Transform _xSquishCamFocus;
    [SerializeField] Transform _ySquishCamFocus;
    AudioSource _audioSource;
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
    bool _canSquish = true;
    float _routineDelta = 0f;
    PlayerControls _controls;
    PlayerHealth _playerHealth;
    GameObject _hideCamera = null;
    const string _tightSpace = "TightSpace";
    const string _tunnel = "Tunnel";

    void Awake()
    {
        _controls = new PlayerControls();
        _playerHealth = GetComponent<PlayerHealth>();
        _audioSource = GetComponent<AudioSource>();
    }
    
    void OnEnable()
    {
        _controls.Player.Enable();
        _playerHealth.OnPlayerHurt += ForceUnhide;
        _playerHealth.OnPlayerDefeat += DisableSquishing;
        if(_cutsceneManager)
        {
            _cutsceneManager.OnCinematicPlayed += DisableAndUnsquish;
            _cutsceneManager.OnCinematicFinished += EnableSquishing;
        }
    }

    void OnDisable()
    {
        _controls.Player.Disable();
        _playerHealth.OnPlayerHurt -= ForceUnhide;
        _playerHealth.OnPlayerDefeat -= DisableSquishing;
        if(_cutsceneManager)
        {
            _cutsceneManager.OnCinematicPlayed -= DisableAndUnsquish;
            _cutsceneManager.OnCinematicFinished -= EnableSquishing;
        }
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
            // _cineCollider.enabled = false;
        }
        else if(other.CompareTag(_tunnel))
        {
            _inTightSpace = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag(_tightSpace))
        {
            _inTightSpace = true;
            // _cineCollider.enabled = false;
        }
        else if(other.CompareTag(_tunnel))
        {
            _inTightSpace = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(_tightSpace))
        {
            _inTightSpace = false;
            // Invoke("EnableCineCollider", 1f);
        }
        else if(other.CompareTag(_tunnel))
        {
            _inTightSpace = false;
        }
    }

    void Update()
    {
        if(_inTightSpace && _isSquished)
        {
            _canSquishCanvas.enabled = true;
        }
        else
        {
            _canSquishCanvas.enabled = false;
        }
    }

    void EnableCineCollider()
    {
        if(!_inTightSpace)
        {
            _cineCollider.enabled = true;
        }
    }

    void SideSquish()
    {
        if(_isSquishing || _isHiding || !_canSquish || Time.timeScale == 0) { return; }

        if(_isSquished)
        {
            Unsquish();
            return;
        }
        else
        {
            _isSquishing = true;
            _routineDelta = 0f;
            _audioSource.Stop();
            _audioSource.PlayOneShot(_squishAudioClip);
            StartCoroutine(SideRoutine());
            // _camera.LookAt = _xSquishCamFocus;
        }
    }

    IEnumerator SideRoutine()
    {
        while(_routineDelta < _coroutineDuration)
        {
            transform.localScale = new Vector3(transform.localScale.x - _squishRoutineAmount, 1f, 1f);
            _routineDelta += 0.05f;
            yield return null;
        }
        _isSquishing = false;
        _isSquished = true;
        transform.localScale = new Vector3(_squishAmount, 1f, 1f);
        _charaCont.radius *= _radiusSideSquishAmount;
        _charaCont.center = new Vector3(_centerDefault.x, _centerDefault.y, _zCenterOffset);
        
        foreach (MaterialSwapper swapper in _swappers)
        {
            swapper.SwapMaterials();
        }
    }

    void VerticalSquish()
    {
        if(_isSquishing || _isHiding || !_canSquish || Time.timeScale == 0) { return; }

        if(_isSquished)
        {
            Unsquish();
            return;
        }
        else
        {
            _isSquishing = true;
            _routineDelta = 0f;
            _audioSource.Stop();
            _audioSource.PlayOneShot(_squishAudioClip);
            StartCoroutine(HeightRoutine());
            _camera.LookAt = _ySquishCamFocus;
        }
    }

    IEnumerator HeightRoutine()
    {
        // while(_routineDelta < _coroutineDuration)
        // {
        //     transform.localScale = new Vector3(1f, transform.localScale.y - _squishAmount, 1f);
        //     _routineDelta += _heightDelta;
        //     yield return null;
        // }
        int _routineDeltaInt = 0;

        while(_routineDeltaInt < 10) //This works! It's badly written but I'm on a time limit here
        {
            transform.localScale = new Vector3(1f, transform.localScale.y - (10 * _squishAmount), 1f);
            _routineDeltaInt++;
            yield return new WaitForSeconds(0.01f);
        }

        _isSquishing = false;
        _isSquished = true;
        transform.localScale = new Vector3(1f, _squishAmount, 1f);
        _charaCont.stepOffset = _squishedStepOffset;
        _charaCont.height *= _squishAmount;
        _charaCont.center = new Vector3(_centerDefault.x, _yCenteredSquishAmount, _centerDefault.z);
        _charaCont.radius = _radiusVertSquishAmount;
        
        foreach (MaterialSwapper swapper in _swappers)
        {
            swapper.SwapMaterials();
        }
    }

    void FrontSquish()
    {
        if(_isSquishing || _isHiding || !_canSquish || Time.timeScale == 0) { return; }

        if(_isSquished)
        {
            Unsquish();
            return;
        }
        else
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(_squishAudioClip);
            _isSquished = true;
            transform.localScale = new Vector3(1f, 1f, _squishAmount);
        }

        foreach (MaterialSwapper swapper in _swappers)
        {
            swapper.SwapMaterials();
        }
    }

    void Unsquish()
    {
        if(_inTightSpace || _isHiding || Time.timeScale == 0) { return; }

        _audioSource.Stop();
        _audioSource.PlayOneShot(_unSquishAudioClip);

        transform.localScale = Vector3.one;
        _charaCont.height = _heightDefault;
        _charaCont.center = _centerDefault;
        _charaCont.radius = _radiusDefault;
        _charaCont.stepOffset = _stepOffsetDefault;
        _camera.LookAt = _defaultCamFocus;
        _isSquished = false;
        // _cineCollider.enabled = true;

        foreach (MaterialSwapper swapper in _swappers)
        {
            swapper.UnswapMaterials();
        }
    }

    public void HideInPainting(Transform painting, GameObject camera)
    {
        if(!_canSquish) { return; }

        if(_isHiding)
        {
            transform.position = _previousPosition;
            transform.rotation = _previousRotation;
            _mover.SetIsHiding(false);
            _hider.LeaveStealth();
            _isHiding = false;
            Unsquish();
            if(_hideCamera)
            {
                _hideCamera.SetActive(false);
            }
            _hideCamera = null;
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
        _hider.AttemptStealth();
        _previousPosition = transform.position;
        _previousRotation = transform.rotation;
        FrontSquish();
        transform.position = painting.position;
        transform.rotation = painting.rotation;
        _hideCamera = camera;
        if(_hideCamera)
        {
            _hideCamera.SetActive(true);
        }
        _isHiding = true;
        _mover.HideInteractable();
    }

    void ForceUnhide()
    {
        if(_isHiding)
        {
            transform.position = _previousPosition;
            transform.rotation = _previousRotation;
            _mover.SetIsHiding(false);
            _hider.LeaveStealth();
            _isHiding = false;
            Unsquish();
            if(_hideCamera)
            {
                _hideCamera.SetActive(false);
            }
            _hideCamera = null;
        }

    }

    void DisableSquishing()
    {
        _canSquish = false;
    }

    void DisableAndUnsquish()
    {
        StopAllCoroutines();
        _inTightSpace = false;
        _isHiding = false;
        Unsquish();
        _canSquish = false;
    }

    void EnableSquishing()
    {
        _canSquish = true;
    }
}
