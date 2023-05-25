using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int _maxHealth = 3;
    [SerializeField] int _iframeDuration = 12;
    [SerializeField] float _blinkDelay = 0.05f;
    [SerializeField] SkinnedMeshRenderer[] _renderers;
    [SerializeField] MeshRenderer _crownRenderer;
    [SerializeField] Image[] _heartImages;
    
    Squisher _squisher;
    [SerializeField] int _currentHealth;
    bool _isInvincible = false;
    int _blinkTime = 0;

    void Awake()
    {
        _squisher = GetComponent<Squisher>();
        _currentHealth = _maxHealth;
    }

    void Start()
    {
        UpdateHealthHUD();
    }

    public void DealDamage(int damage)
    {
        if(_isInvincible) { return; }

        _squisher.ForceUnsquish();

        _currentHealth -= damage;
        UpdateHealthHUD();
        
        if(_currentHealth <= 0)
        {
            HandleDeath();
        }
        else
        {
            //TODO play Hurt SFX
            //TODO update UI that hasn't been made yet
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

    private void HandleDeath()
    {
        //TODO actually handle death
        Debug.Log("I'm dead!");
    }
}
