using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Collections.Generic;
using Cinemachine;
using System.Runtime.InteropServices;

public class LevelManager : MonoBehaviour
{
    public event Action OnLevelCompleted;
    public event Action OnLevelStarted;
    [SerializeField] float _unWipeDelay = 0.2f;
    [SerializeField] float _wipeDelay = 0.1f;
    [SerializeField] float _unWipeSpeed = 1.25f;
    [SerializeField] float _wipeSpeed = 0.5f;
    [SerializeField] Image _image;
    [SerializeField] Canvas _statCanvas;
    [SerializeField] Canvas _timerCanvas;
    [SerializeField] GameObject _nextLevelButton;
    [SerializeField] TMP_Text _timerTextField;
    [SerializeField] TMP_Text _statsText;
    [SerializeField] CutsceneManager _cutsceneManager;
    [SerializeField] CinemachineFreeLook _freelookCamera;

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
        _freelookCamera.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnEnable()
    {
        if(_cutsceneManager)
        {
            _cutsceneManager.OnCinematicPlayed += PauseTimer;
        }
    }

    void OnDisable()
    {
        if(_cutsceneManager)
        {
            _cutsceneManager.OnCinematicPlayed -= PauseTimer;
        }
    }

    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(_unWipeDelay);

        while(_image.fillAmount > 0)
        {
            _image.fillAmount -= _unWipeSpeed * Time.unscaledDeltaTime;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        _freelookCamera.enabled = true;
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

    void PauseTimer()
    {
        _timerStarted = false;
    }

    void StopTimer()
    {
        _timerStarted = false;
        CurrentRunManager.Instance.SetCompletionTime(_currentSceneIndex - 1, _completionTime);
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
        string timeStats = "<size=120%>-Level Completion Times-</size>";
        _copyStats = "-Level Completion Times-";
        List<float> times = CurrentRunManager.Instance.GetCompletionTimes();
        for (int i = 0; i < times.Count; i++)
        {
            string formattedTime = FormatTime(times[i]);
            timeStats += $"\nLevel {i+1}: {formattedTime}";
            _copyStats += $"\nLevel {i+1}: {PlainTextFormatTime(times[i])}";
            totalTime += times[i];
        }
        timeStats += $"\nTotal Time: {FormatTime(totalTime)}";
        _copyStats += $"\nTotal Time: {PlainTextFormatTime(totalTime)}";
        // if(_heartWasCollected)
        // {
            _totalStats = $"Collected Hearts: {CurrentRunManager.Instance.CollectedHearts}/{_currentSceneIndex}\nGuards Alerted: {_timesSeen}\nTotal Guards Alerted: {CurrentRunManager.Instance.AlertedGuards}\n{timeStats}";
            _copyStats = $"Collected Hearts: {CurrentRunManager.Instance.CollectedHearts}/{_currentSceneIndex}\nGuards Alerted: {_timesSeen}\nTotal Guards Alerted: {CurrentRunManager.Instance.AlertedGuards}\n{_copyStats}";
        // }
        // else
        // {
        //     _totalStats = $"Collected heart: 0/1\nGuards Alerted: {_timesSeen}\n{timeStats}";
        //     _totalStats = $"Collected heart: 0/1\nGuards Alerted: {_timesSeen}\n{_copyStats}";
        // }
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
        _freelookCamera.enabled = false;
        StartCoroutine(ImageWipe());
        OnLevelCompleted?.Invoke();
        CurrentRunManager.Instance.SetTotalGuardsAlerted(_timesSeen);
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
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            CopyToClipboard(_copyStats); // See below
        }
        else
        {
            GUIUtility.systemCopyBuffer = _copyStats;
        }
    }

    [DllImport("__Internal")] // https://pudding-entertainment.medium.com/unity-webgl-add-a-share-button-93831b3555e9
    private static extern void CopyToClipboard(string textToCopy);
}
