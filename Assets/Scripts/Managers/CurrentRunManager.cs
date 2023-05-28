using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRunManager : MonoBehaviour
{
    [field:SerializeField] public int CollectedHearts { get; private set; } = 0; // This doesn't need to be public but anyway
    List<float> _completionTimes = new List<float>();
    
    PlayerHealth _playerHealth;

    void Awake()
    {
        if(FindObjectOfType<CurrentRunManager>() != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetCompletionTime(int levelIndex, float compTime)
    {
        _completionTimes.Insert(levelIndex, compTime);
    }

    public List<float> GetCompletionTimes()
    {
        return _completionTimes;
    }

    public void SetCollectedHeartsCount(int value)
    {
        CollectedHearts += value;
    }

    public int GetCollectedHeartsCount()
    {
        return CollectedHearts;
    }
}
