using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteLevel : MonoBehaviour
{
    int _currentSceneIndex;
    bool _hasEntered = false;

    void Awake()
    {
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !_hasEntered)
        {
            _hasEntered = true;
            SceneManager.LoadScene(_currentSceneIndex + 1);
        }
    }
}
