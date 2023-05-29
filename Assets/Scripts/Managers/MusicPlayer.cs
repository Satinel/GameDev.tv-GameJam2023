using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioClip _levelSong;
    [SerializeField] AudioClip _victoryJingle;
    [SerializeField] AudioClip _defeatJingle;
    [SerializeField] AudioClip _introClip;
    [SerializeField] LevelManager _levelManager;
    [SerializeField] PlayerHealth _playerHealth;
    AudioSource _audioSource;
    bool _waitingForIntro = false;

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

    void Update()
    {
        if(!_audioSource.isPlaying && _waitingForIntro)
        {
            _audioSource.loop = true;
            _audioSource.clip = _levelSong;
            _audioSource.Play();
            _waitingForIntro = false;
        }

    }

    void StartMusic()
    {
        if(_introClip)
        {
            _audioSource.loop = false;
            _audioSource.PlayOneShot(_introClip);
            _waitingForIntro = true;
        }
        else if(_levelSong)
        {
            _audioSource.loop = true;
            _audioSource.clip = _levelSong;
            _audioSource.Play();
        }
    }



    void StopMusic()
    {
        _waitingForIntro = false;
        _audioSource.Stop();
        _audioSource.loop = false;
        if(_victoryJingle != null)
        {
            _audioSource.PlayOneShot(_victoryJingle);
        }
    }

    void PlayDefeatMusic()
    {
        _waitingForIntro = false;
        _audioSource.Stop();
        _audioSource.loop = false;
        if(_defeatJingle != null)
        {
            _audioSource.PlayOneShot(_defeatJingle);
        }
    }
}
