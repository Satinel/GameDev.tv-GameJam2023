using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class FreeLookSensitivity : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook _camera;
    [SerializeField] Slider _slider;
    [SerializeField] Slider _ySlider;
    [SerializeField] Toggle _xInvert;
    [SerializeField] Toggle _yInvert;

    void Start()
    {
        _slider.value = PlayerPrefs.GetFloat("LookSensitivity", 0.125f);
        _ySlider.value = PlayerPrefs.GetFloat("YLookSensitivity", 0.001f);
        _camera.m_XAxis.m_MaxSpeed = _slider.value;
        _camera.m_YAxis.m_MaxSpeed = _ySlider.value;
        if(PlayerPrefs.GetInt("InvertX", 0) == 0)
        {
            _xInvert.isOn = false;
            _camera.m_XAxis.m_InvertInput = false;
        }
        else
        {
            _xInvert.isOn = true;
            _camera.m_XAxis.m_InvertInput = true;
        }
        if(PlayerPrefs.GetInt("InvertY", 1) == 0)
        {
            _yInvert.isOn = false;
            _camera.m_YAxis.m_InvertInput = false;
        }
        else
        {
            _yInvert.isOn = true;
            _camera.m_YAxis.m_InvertInput = true;
        }
    }

    // void Update()
    // {
    //     Debug.Log("X Speed: " + _camera.m_XAxis.m_MaxSpeed);
    //     Debug.Log("Y Speed: " + _camera.m_YAxis.m_MaxSpeed);
    // }

    public void AdjustSensitivity()
    {
        _camera.m_XAxis.m_MaxSpeed = _slider.value;
        PlayerPrefs.SetFloat("LookSensitivity", _slider.value);
        // _camera.m_YAxis.m_MaxSpeed = _slider.value;
    }

    public void AdjustYSensitivity()
    {
        _camera.m_YAxis.m_MaxSpeed = _ySlider.value;
        PlayerPrefs.SetFloat("YLookSensitivity", _ySlider.value);
    }

    public void InvertX()
    {
        if(_camera.m_XAxis.m_InvertInput)
        {
            _camera.m_XAxis.m_InvertInput = false;
            PlayerPrefs.SetInt("InvertX", 0);
        }
        else
        {
            _camera.m_XAxis.m_InvertInput = true;
            PlayerPrefs.SetInt("InvertX", 1);
        }
    }

    public void InvertY()
    {
        if(_camera.m_YAxis.m_InvertInput)
        {
            _camera.m_YAxis.m_InvertInput = false;
            PlayerPrefs.SetInt("InvertY", 0);
        }
        else
        {
            _camera.m_YAxis.m_InvertInput = true;
            PlayerPrefs.SetInt("InvertY", 1);
        }
    }
}
