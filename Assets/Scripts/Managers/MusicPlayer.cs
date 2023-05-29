using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioClip _levelSong;
    [SerializeField] AudioClip _victoryJingle;
    [SerializeField] AudioClip _defeatJingle;
    [SerializeField] LevelManager _levelManager;
    [SerializeField] PlayerHealth _playerHealth;
    AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if(!_levelManager)
        {
            _levelManager = FindObjectOfType<LevelManager>();
        }
        if(!_playerHealth)
        {
            _playerHealth = FindObjectOfType<PlayerHealth>();
        }
    }

    void OnEnable()
    {
        _levelManager.OnLevelStarted += StartMusic;
        _levelManager.OnLevelCompleted += StopMusic;
        _playerHealth.OnPlayerDefeat += PlayDefeatMusic;
    }

    void OnDisable()
    {
        _levelManager.OnLevelStarted -= StartMusic;
        _levelManager.OnLevelCompleted -= StopMusic;
        _playerHealth.OnPlayerDefeat -= PlayDefeatMusic;
    }

    void StartMusic()
    {
        if(_levelSong)
        {
            _audioSource.loop = true;
            _audioSource.PlayOneShot(_levelSong);
        }
    }

    void StopMusic()
    {
        _audioSource.Stop();
        if(_victoryJingle != null)
        {
            _audioSource.PlayOneShot(_victoryJingle);
        }
    }

    void PlayDefeatMusic()
    {
        _audioSource.Stop();
        _audioSource.loop = false;
        if(_defeatJingle != null)
        {
            _audioSource.PlayOneShot(_defeatJingle);
        }
    }
}
