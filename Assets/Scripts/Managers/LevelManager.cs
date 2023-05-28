using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public event Action OnLevelCompleted;
    public event Action OnLevelStarted;
    [SerializeField] float _unWipeDelay = 1f;
    [SerializeField] float _wipeDelay = 0.1f;
    [SerializeField] float _unWipeSpeed = 1f;
    [SerializeField] float _wipeSpeed = 0.5f;
    [SerializeField] Image _image;
    [SerializeField] Canvas _statCanvas;
    [SerializeField] Canvas _timerCanvas;
    [SerializeField] GameObject _nextLevelButton;
    [SerializeField] TMP_Text _timerTextField;
    [SerializeField] TMP_Text _statsText;

    CurrentRunManager _currentRunManager;

    int _currentSceneIndex;
    float _completionTime;
    bool _timerStarted;
    int _minutes;
    int _seconds;
    int _milliseconds;
    
    bool _heartWasCollected = false;
    int _timesSeen = 0;

    string _totalStats;
    string _copyStats;

    
    void Awake()
    {
        _image.enabled = true;
        Time.timeScale = 0;
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        _image.fillClockwise = false;

        _currentRunManager = FindObjectOfType<CurrentRunManager>();
    }

    IEnumerator Start()
    {
        yield return _unWipeDelay;

        while(_image.fillAmount > 0)
        {
            _image.fillAmount -= _unWipeSpeed * Time.unscaledDeltaTime;
            yield return null;
        }
        
        StartTimer();
        OnLevelStarted?.Invoke();
    }

    void Update()
    {
        if(_timerStarted)
        {
            _completionTime += Time.deltaTime;
            _timerTextField.text = FormatTime(_completionTime);
        }
    }

    void StartTimer()
    {
        _completionTime = 0;
        _timerTextField.text = FormatTime(_completionTime);
        _timerCanvas.enabled = true;
        
        Time.timeScale = 1;
        _timerStarted = true;
    }

    void StopTimer()
    {
        _timerStarted = false;
        _currentRunManager.SetCompletionTime(_currentSceneIndex - 1, _completionTime);
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
        UpdateStats();
        _statCanvas.enabled = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void UpdateStats()
    {
        float totalTime = 0;
        string timeStats = "<size=120%>Level Completion Times:</size>";
        _copyStats = "Level Completion Times:";
        List<float> times = _currentRunManager.GetCompletionTimes();
        for (int i = 0; i < times.Count; i++)
        {
            string formattedTime = FormatTime(times[i]);
            timeStats += $"\nLevel {i}: {formattedTime}";
            _copyStats += $"\nLevel {i}: {PlainTextFormatTime(times[i])}";
            totalTime += times[i];
        }
        timeStats += $"\n\nTotal Time: {FormatTime(totalTime)}";
        _copyStats += $"\n\nTotal Time: {PlainTextFormatTime(totalTime)}";
        if(_heartWasCollected)
        {
            _totalStats = $"Collected Hearts: 1/1\nGuards Alerted: {_timesSeen}\n\n{timeStats}";
            _totalStats = $"Collected Hearts: 1/1\nGuards Alerted: {_timesSeen}\n\n{_copyStats}";
        }
        else
        {
            _totalStats = $"Collected heart: 0/1\nGuards Alerted: {_timesSeen}\n\n{timeStats}";
            _totalStats = $"Collected heart: 0/1\nGuards Alerted: {_timesSeen}\n\n{_copyStats}";
        }
        _statsText.text = _totalStats;
    }

    string FormatTime(float timeToFormat)
    {
        _minutes = ((int)timeToFormat / 60);
        _seconds = ((int)timeToFormat % 60);
        _milliseconds = (int)(timeToFormat * 1000) % 1000;
        string timeFormatted = $"{_minutes.ToString("D2")}:{_seconds.ToString("D2")}<sup>.<size=70%>{_milliseconds.ToString("D3")}</size></sup>";
        return timeFormatted;
    }

    string PlainTextFormatTime(float timeToFormat)
    {
        _minutes = ((int)timeToFormat / 60);
        _seconds = ((int)timeToFormat % 60);
        _milliseconds = (int)(timeToFormat * 1000) % 1000;
        string timeFormatted = $"{_minutes.ToString("D2")}:{_seconds.ToString("D2")}.{_milliseconds.ToString("D3")}";
        return timeFormatted;
    }

    public void LevelCompleted()
    {
        StopTimer();
        StartCoroutine(ImageWipe());
        OnLevelCompleted?.Invoke();
        //TODO play/change music??!
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(_currentSceneIndex + 1);
    }

    public void ReportHeartCollected(bool wasCollected)
    {
        _heartWasCollected = wasCollected;
    }

    public void ReportGuardsAlerted(bool wasAlerted)
    {
        if(wasAlerted)
        {
            _timesSeen++;
        }
    }

    public void CopyStats()
    {
        GUIUtility.systemCopyBuffer = _copyStats;
    }
}
