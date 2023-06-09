using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteLevel : MonoBehaviour
{
    [SerializeField] LevelManager _levelManager;

    int _currentSceneIndex;
    bool _hasEntered = false;

    void Awake()
    {
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(!_levelManager)
        {
            _levelManager = FindObjectOfType<LevelManager>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !_hasEntered)
        {
            _hasEntered = true;
            _levelManager.LevelCompleted();
        }
    }
}
