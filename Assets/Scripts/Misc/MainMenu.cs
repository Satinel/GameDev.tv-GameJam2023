using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Canvas _mainOptionsCanvas;
    [SerializeField] Canvas _audioCanvas;

    public void StartGame()
    {
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
        Application.Quit();
    }
}
