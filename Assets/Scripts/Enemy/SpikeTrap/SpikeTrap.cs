using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] AudioClip _audioClip;
    [SerializeField] float _resetTime = 1f;
    AudioSource _audioSource;
    Animator _animator;
    readonly int ACTIVE_HASH = Animator.StringToHash("Active");
    bool _hasTriggered = false;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Mover>() && !_hasTriggered)
        {
            _hasTriggered = true;
            _audioSource.Stop();
            _audioSource.PlayOneShot(_audioClip);
            _animator.SetTrigger(ACTIVE_HASH);
            Invoke("ResetTrap", _resetTime);
        }
    }

    void ResetTrap()
    {
        _hasTriggered = false;
    }
}
