using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float _wipeDelay = 0.1f;
    [SerializeField] float _unWipeSpeed = 1f;
    [SerializeField] float _wipeSpeed = 0.5f;
    [SerializeField] Image _image;
    [SerializeField] Canvas _statCanvas;
    [SerializeField] Canvas _timerCanvas;
    [SerializeField] GameObject _nextLevelButton;
    [SerializeField] TMP_Text _timerTextField;

    CurrentRunManager _currentRunManager;

    int _currentSceneIndex;
    float _completionTime;
    bool _timerStarted;
    int _minutes;
    int _seconds;
    int _milliseconds;
    
    void Awake()
    {
        Time.timeScale = 0;
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        _image.fillClockwise = false;

        _currentRunManager = FindObjectOfType<CurrentRunManager>();
    }

    IEnumerator Start()
    {
        while(_image.fillAmount > 0)
        {
            _image.fillAmount -= _unWipeSpeed * Time.unscaledDeltaTime;
            yield return null;
        }
        
        StartTimer();
    }

    void Update()
    {
        if(_timerStarted)
        {
            _completionTime += Time.deltaTime;
            _minutes = ((int)_completionTime / 60);
            _seconds = ((int)_completionTime % 60);
            _milliseconds = (int)(_completionTime * 1000) % 1000;
            _timerTextField.text = $"{_minutes.ToString("D2")}:{_seconds.ToString("D2")}<sup>.<size=70%>{_milliseconds.ToString("D3")}</size></sup>";
        }
    }

    void StartTimer()
    {
        _completionTime = 0;

        _minutes = ((int)_completionTime / 60);
        _seconds = ((int)_completionTime % 60);
        _milliseconds = (int)(_completionTime * 1000) % 1000;
        _timerTextField.text = $"{_minutes.ToString("D2")}:{_seconds.ToString("D2")}<sup>.<size=70%>{_milliseconds.ToString("D3")}</size></sup>";
        _timerCanvas.enabled = true;
        
        Time.timeScale = 1;
        _timerStarted = true;
    }

    void StopTimer()
    {
        _timerStarted = false;
        _currentRunManager.SetCompletionTime(_currentSceneIndex, _completionTime);
    }

    IEnumerator ImageWipe()
    {
        _image.fillClockwise = true;
        yield return new WaitForSeconds(_wipeDelay);

        while(_image.fillAmount < 1)
        {
            _image.fillAmount += _wipeSpeed * Time.deltaTime;
            yield return null;
        }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_nextLevelButton);
        _statCanvas.enabled = true;
        Time.timeScale = 0f;
    }

    public void LevelCompleted()
    {
        StopTimer();
        StartCoroutine(ImageWipe());
        //TODO play/change music??!
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(_currentSceneIndex + 1);
    }
}
