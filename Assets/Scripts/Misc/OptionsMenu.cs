using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] Canvas _mainOptionsCanvas;
    [SerializeField] Canvas _audioCanvas;
    [SerializeField] GameObject _resumeButton;
    [SerializeField] GameObject _menuPanel;
    [SerializeField] PlayerHealth _playerHealth;
    [SerializeField] GameObject _optionsFirstButton;
    [SerializeField] GameObject _audioFirstButton;
    [SerializeField] GameObject _optionsAlternateFirstButton;
    
    PlayerControls _controls;
    bool _playerDefeated = false;
    float _currentTimescale = 1f;

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
        EventSystem.current.SetSelectedGameObject(null);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void HideResumeButton()
    {
        _playerDefeated = true;
        _resumeButton.SetActive(false);
        _menuPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_optionsAlternateFirstButton);
    }

    public void ToggleMainMenuCanvas()
    {
        if(_audioCanvas.enabled)
        {
            ToggleAudioCanvas();
            if(_playerDefeated)
            {
                EventSystem.current.SetSelectedGameObject(_optionsAlternateFirstButton);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(_optionsFirstButton);
            }
        }
        else if(_mainOptionsCanvas.enabled)
        {
            EventSystem.current.SetSelectedGameObject(null);
            _mainOptionsCanvas.enabled = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = _currentTimescale;
            
        }
        else
        {
            _currentTimescale = Time.timeScale;
            _mainOptionsCanvas.enabled = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            EventSystem.current.SetSelectedGameObject(null);
            if(_playerDefeated)
            {
                EventSystem.current.SetSelectedGameObject(_optionsAlternateFirstButton);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(_optionsFirstButton);
            }
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
        if(_audioCanvas.enabled)
        {
            _audioCanvas.enabled = false;
            _mainOptionsCanvas.enabled = true;
            if(_playerDefeated)
            {
                EventSystem.current.SetSelectedGameObject(_optionsAlternateFirstButton);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(_optionsFirstButton);
            }
        }
        else
        {
            _audioCanvas.enabled = true;
            _mainOptionsCanvas.enabled = false;
            EventSystem.current.SetSelectedGameObject(_audioFirstButton);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
