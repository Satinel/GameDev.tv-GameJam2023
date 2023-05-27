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
    [SerializeField] float _wipeDelay = 0.5f;
    [SerializeField] GameObject _retryButton;
    
    Animator _animator;
    [SerializeField] int _currentHealth;
    bool _isInvincible = false;
    int _blinkTime = 0;

    readonly int DEATH_HASH = Animator.StringToHash("Death");

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _currentHealth = _maxHealth;
    }

    void Start()
    {
        UpdateHealthHUD();
    }

    public void DealDamage(int damage)
    {
        if(_isInvincible) { return; }

        _currentHealth -= damage;
        UpdateHealthHUD();
        OnPlayerHurt?.Invoke();
        
        if(_currentHealth <= 0)
        {
            HandleDeath();
        }
        else
        {
            //TODO play Hurt SFX
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
        //TODO play sad SFX/Music
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
            _faderImage.fillAmount += Time.deltaTime;
            yield return null;
        }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_retryButton);
        _mainMenuCanvas.enabled = true;
        Time.timeScale = 0f;
    }

    public void GainHeart(int value)
    {
        _maxHealth += value;
        _currentHealth += value;
        UpdateHealthHUD();
    }
}
