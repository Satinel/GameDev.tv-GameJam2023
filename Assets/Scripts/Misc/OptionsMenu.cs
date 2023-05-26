using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] Canvas _mainOptionsCanvas;
    [SerializeField] Canvas _audioCanvas;
    [SerializeField] GameObject _resumeButton;
    [SerializeField] GameObject _menuPanel;
    [SerializeField] PlayerHealth _playerHealth;
    
    PlayerControls _controls;
    bool _playerDefeated = false;

    void Awake()
    {
        _controls = new PlayerControls();
    }

    void OnEnable()
    {
        _controls.Player.Enable();
        _playerHealth.OnPlayerDefeat += HideResumeButton;
    }

    void OnDisable()
    {
        _controls.Player.Disable();
        _playerHealth.OnPlayerDefeat -= HideResumeButton;
    }

    void Start()
    {
        _controls.Player.Options.performed += _ => ToggleMainMenuCanvas();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void HideResumeButton()
    {
        _playerDefeated = true;
        _resumeButton.SetActive(false);
        _menuPanel.SetActive(false);
    }

    public void ToggleMainMenuCanvas()
    {
        if(_audioCanvas.enabled)
        {
            ToggleAudioCanvas();
        }
        else if(_playerDefeated)
        {
            return; 
        }
        else if(_mainOptionsCanvas.enabled)
        {
            _mainOptionsCanvas.enabled = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 1;
        }
        else
        {
            _mainOptionsCanvas.enabled = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ToggleAudioCanvas()
    {
        if(_audioCanvas)
        {
            _audioCanvas.enabled = !_audioCanvas.enabled;
        }
        if(_mainOptionsCanvas)
        {
            _mainOptionsCanvas.enabled = !_mainOptionsCanvas.enabled;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
