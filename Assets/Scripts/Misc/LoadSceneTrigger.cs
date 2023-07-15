using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneTrigger : MonoBehaviour
{
    [SerializeField] int _sceneIndex = 1;

    void LoadScene()
    {
        SceneManager.LoadScene(_sceneIndex);
    }
}
