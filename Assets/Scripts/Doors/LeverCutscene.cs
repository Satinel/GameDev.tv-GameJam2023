using UnityEngine;
using Cinemachine;

public class LeverCutscene : MonoBehaviour
{
    [SerializeField] GameObject _camera;
    [SerializeField] CinemachineInputProvider _followCamInputProvider;
    [SerializeField] float _delayTime = 1f;

    public void EnableCamera()
    {
        _camera.SetActive(true);
        _followCamInputProvider.enabled = false;
        Invoke("DisableCamera", _delayTime);
    }

    void DisableCamera()
    {
        _camera.SetActive(false);
        _followCamInputProvider.enabled = true;
    }
}
