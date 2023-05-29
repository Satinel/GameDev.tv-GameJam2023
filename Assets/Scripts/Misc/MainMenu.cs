using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Canvas _mainOptionsCanvas;
    [SerializeField] Canvas _audioCanvas;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] GameObject _startButton;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_startButton);
    }

    public void StartGame()
    {
        _audioSource.Stop();
        SceneManager.LoadScene(1);
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
        _audioSource.Stop();
        Application.Quit();
    }
}
