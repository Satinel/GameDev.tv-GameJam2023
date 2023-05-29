using System;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public event Action OnCinematicPlayed;
    public event Action OnCinematicFinished;
    PlayableDirector _director;
    bool _hasPlayed = false;
    
    void Awake()
    {
        _director = GetComponent<PlayableDirector>();
    }

    private void Start() {
        _director.stopped += FinishCinematic;
    }

    void OnTriggerEnter(Collider other)
    {
        if(!_hasPlayed && other.CompareTag("Player"))
        {
            _hasPlayed = true;
            OnCinematicPlayed?.Invoke();
            _director.Play();
        }
    }

    public void FinishCinematic(PlayableDirector director)
    {
        OnCinematicFinished?.Invoke();
    }
}
