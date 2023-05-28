using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CurrentRunManager : MonoBehaviour
{
    private static CurrentRunManager instance; // c/o https://community.gamedev.tv/t/why-did-we-go-back-to-teaching-the-bad-singletons/182833/5
    public static CurrentRunManager Instance => instance; //prevents other classes from changing it
    [field:SerializeField] public int CollectedHearts { get; private set; } = 0;
    [field:SerializeField] public int AlertedGuards { get; private set; } = 0;
    List<float> _completionTimes = new List<float>();
    
    PlayerHealth _playerHealth;

    // void Awake()
    // {
    //     if(FindObjectOfType<CurrentRunManager>() != this)
    //     {
    //         Destroy(this.gameObject);
    //     }
    //     else
    //     {
    //         DontDestroyOnLoad(gameObject);
    //     }
    // }
    void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance=this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetCompletionTime(int levelIndex, float compTime)
    {
        // _completionTimes.Insert(levelIndex, compTime);
        _completionTimes.Add(compTime);
    }

    public List<float> GetCompletionTimes()
    {
        return _completionTimes;
    }

    public void SetCollectedHeartsCount(int value)
    {
        CollectedHearts += value;
    }

    public void SetTotalGuardsAlerted(int value)
    {
        AlertedGuards += value;
    }
}
