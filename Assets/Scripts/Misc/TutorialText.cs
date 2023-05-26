using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TutorialText : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] string _firstLine;
    [SerializeField] string _lastLine;
    
    PlayerControls _controls;
    string _keyboardXSquishString;
    string _gamepadXSquishString;
    string _keyboardYSquishString;
    string _gamepadYSquishString;
    string _keyboardInteractString;
    string _gamepadInteractString;

    [SerializeField] bool _isXTip;
    [SerializeField] bool _isYTip;
    [SerializeField] bool _isInteractTip;

    void Awake()
    {
        _controls = new PlayerControls();
    }

    void Start()
    {
        _keyboardXSquishString = _controls.Player.XSquish.GetBindingDisplayString(0);
        _gamepadXSquishString = _controls.Player.XSquish.GetBindingDisplayString(1);
        _keyboardYSquishString = _controls.Player.YSquish.GetBindingDisplayString(0);
        _gamepadYSquishString = _controls.Player.YSquish.GetBindingDisplayString(1);
        _keyboardInteractString = _controls.Player.Interact.GetBindingDisplayString(0);
        _gamepadInteractString = _controls.Player.Interact.GetBindingDisplayString(1);
    }

    void OnEnable()
    {
        _controls.Player.Enable();
    }

    void OnDisable()
    {
        _controls.Player.Disable();
    }
    

    void Update()
    {
        if(_isXTip)
        {
            _text.text = $"{_firstLine} {_keyboardXSquishString} or Gamepad <color=blue>{_gamepadXSquishString}</color> {_lastLine}";
        }
        else if(_isYTip)
        {
            _text.text = $"{_firstLine} {_keyboardYSquishString} or Gamepad <color=green>{_gamepadYSquishString}</color> {_lastLine}";
        }
        else if(_isInteractTip)
        {
            _text.text = $"{_firstLine} {_keyboardInteractString} or Gamepad <color=orange>{_gamepadInteractString}</color> {_lastLine}";
        }
        
    }
}
