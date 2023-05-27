using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRunManager : MonoBehaviour
{
    [field:SerializeField] public int CollectedHearts { get; private set; }
    
    PlayerHealth _playerHealth;
    List<float> _completionTimes = new List<float>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _completionTimes[0] = 0; // This is to sync the numbers better since the main menu is build index 0
    }

    void OnNewSceneLoaded()
    {
        
    }

    public void SetCompletionTime(int levelIndex, float compTime)
    {
        _completionTimes[levelIndex] = compTime;
    }

    public void SetCollectedHeartsCount()
    {
        CollectedHearts++;
    }

    public int GetCollectedHeartsCount()
    {
        return CollectedHearts;
    }
}
