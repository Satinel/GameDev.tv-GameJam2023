using Cinemachine;
using TMPro;
using UnityEngine;

public class DisplayMouseNumbers : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook _camera;
    [SerializeField] TMP_Text _xDisplay;
    [SerializeField] TMP_Text _yDisplay;
    [SerializeField] Canvas _debugCanvas;
    PlayerControls _controls;

    void Awake()
    {
        _controls = new PlayerControls();
    }

    void OnEnable()
    {
        _controls.Player.Enable();
    }

    void OnDisable()
    {
        _controls.Player.Disable();
    }

    void Start()
    {
        _controls.Player.ToggleWalk.performed += _ => ToggleDebugCanvas();
    }

    void Update()
    {
        _xDisplay.text = $"X Axis Value:\n {_camera.m_XAxis.m_InputAxisValue}";
        _yDisplay.text = $"Y Axis Value:\n {_camera.m_YAxis.m_InputAxisValue}";
    }

    void ToggleDebugCanvas()
    {
        _debugCanvas.enabled = !_debugCanvas.enabled;
    }
}
