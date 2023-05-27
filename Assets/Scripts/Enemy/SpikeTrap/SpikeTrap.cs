using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    Animator _animator;
    readonly int ACTIVE_HASH = Animator.StringToHash("Active");

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //TODO play a sound! (I should really get some sounds...)
            _animator.SetTrigger(ACTIVE_HASH);
        }
    }
}
