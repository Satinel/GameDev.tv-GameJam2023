using UnityEngine;
using Cinemachine;

public class LeverCutscene : MonoBehaviour
{
    [SerializeField] GameObject _camera;
    [SerializeField] CinemachineInputProvider _followCamInputProvider;
    [SerializeField] float _delayTime = 1f;
    [SerializeField] GameObject _tipText;

    public void EnableCamera()
    {
        if(_tipText)
        {
            _tipText.SetActive(false);
        }
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
