using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public event Action OnPlayerHurt;
    public event Action OnPlayerDefeat;
    [SerializeField] int _maxHealth = 3;
    [SerializeField] int _iframeDuration = 12;
    [SerializeField] float _blinkDelay = 0.05f;
    [SerializeField] SkinnedMeshRenderer[] _renderers;
    [SerializeField] MeshRenderer _crownRenderer;
    [SerializeField] Image[] _heartImages;
    [SerializeField] Canvas _defeatCanvas;
    [SerializeField] Canvas _mainMenuCanvas;
    [SerializeField] Image _faderImage;
    [SerializeField] float _wipeDelay = 0.15f;
    [SerializeField] float _wipeSpeed = 1.5f;
    [SerializeField] GameObject _retryButton;
    [SerializeField] LevelManager _levelManager;
    [SerializeField] AudioClip _hurtAudioClip;
    AudioSource _audioSource;
    
    Animator _animator;
    [SerializeField] int _currentHealth;
    int _heartsCollected = 0;
    bool _isInvincible = false;
    bool _hasDied = false;
    int _blinkTime = 0;

    readonly int DEATH_HASH = Animator.StringToHash("Death");

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _currentHealth = _maxHealth;
    }

    void Start()
    {
        GainHeart(CurrentRunManager.Instance.CollectedHearts);
    }

    void OnEnable()
    {
        _levelManager.OnLevelCompleted += UpdateCurrentRunHeartCount;
    }

    void OnDisable()
    {
        _levelManager.OnLevelCompleted -= UpdateCurrentRunHeartCount;
    }

    public void DealDamage(int damage)
    {
        if(_isInvincible || _hasDied) { return; }

        _currentHealth -= damage;
        UpdateHealthHUD();
        OnPlayerHurt?.Invoke();
        
        if(_currentHealth <= 0)
        {
            _hasDied = true;
            HandleDeath();
        }
        else
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(_hurtAudioClip);
            _blinkTime = 0;
            StartCoroutine(IFrames());
        }
    }

    void UpdateHealthHUD()
    {
        foreach (Image heart in _heartImages)
        {
            heart.enabled = false;
        }
        for (int i = 0; i < _currentHealth; i++)
        {
            _heartImages[i].enabled = true;
        }
    }

    IEnumerator IFrames()
    {
        _isInvincible = true;

        while (_blinkTime < _iframeDuration)
        {
            _blinkTime += 1;

            foreach(SkinnedMeshRenderer renderer in _renderers)
            {
                renderer.enabled = !renderer.enabled;
            }
            _crownRenderer.enabled = !_crownRenderer.enabled;
            
            yield return new WaitForSeconds(_blinkDelay);
        }

        foreach(SkinnedMeshRenderer renderer in _renderers)
        {
            renderer.enabled = true;
        }
        _crownRenderer.enabled = true;

        _isInvincible = false;
    }

    void HandleDeath()
    {
        _animator.SetTrigger(DEATH_HASH);
        OnPlayerDefeat?.Invoke();
        _defeatCanvas.enabled = true;
        Time.timeScale = 0.1f;
        StartCoroutine(ImageWipe());
    }

    IEnumerator ImageWipe()
    {
        yield return new WaitForSeconds(_wipeDelay);

        while(_faderImage.fillAmount < 1)
        {
            _faderImage.fillAmount += _wipeSpeed * Time.deltaTime;
            yield return null;
        }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_retryButton);
        _mainMenuCanvas.enabled = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void UpdateCurrentRunHeartCount() // these method names are becoming unwieldly
    {
        CurrentRunManager.Instance.SetCollectedHeartsCount(_heartsCollected);
        _levelManager.ReportHeartCollected(_heartsCollected > 0);
    }

    void GainHeart(int value)
    {
        _maxHealth += value;
        _currentHealth += value;
        UpdateHealthHUD();
    }

    public void PickupHeart(int value)
    {
        _heartsCollected += value;
        GainHeart(value);
    }
}
