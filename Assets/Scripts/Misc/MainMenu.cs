using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Canvas _mainOptionsCanvas;
    [SerializeField] Canvas _audioCanvas;
    [SerializeField] Canvas _creditsCanvas;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] GameObject _startButton;
    [SerializeField] GameObject _audioMenuButton;
    [SerializeField] GameObject _creditsMenuButton;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_startButton);
    }

    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _audioSource.Stop();
        SceneManager.LoadScene(1);
    }

    public void ToggleAudioCanvas()
    {
        if(_audioCanvas.enabled)
        {
            _audioCanvas.enabled = false;
            _mainOptionsCanvas.enabled = true;
            EventSystem.current.SetSelectedGameObject(_startButton);
        }
        else
        {
            _mainOptionsCanvas.enabled = false;
            _audioCanvas.enabled = true;
            EventSystem.current.SetSelectedGameObject(_audioMenuButton);
        }
    }

    public void ToggleCreditsCanvas()
    {
        if(_creditsCanvas.enabled)
        {
            _creditsCanvas.enabled = false;
            _mainOptionsCanvas.enabled = true;
            EventSystem.current.SetSelectedGameObject(_startButton);
        }
        else
        {
            _mainOptionsCanvas.enabled = false;
            _creditsCanvas.enabled = true;
            EventSystem.current.SetSelectedGameObject(_creditsMenuButton);
        }
    }

    public void QuitGame()
    {
        _audioSource.Stop();
        Application.Quit();
    }
}
