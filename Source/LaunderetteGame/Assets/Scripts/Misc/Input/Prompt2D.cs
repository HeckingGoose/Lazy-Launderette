using UnityEngine;
using UnityEngine.UI;

public class Prompt2D : MonoBehaviour
{
    // Editor variables
    [Header("Input System")]
    [SerializeField]
    private InputDeviceManager _inputManager;
    [Header("Representing:")]
    [SerializeField]
    private Sprite _keyboardPrompt;
    [SerializeField]
    private Sprite _playstationPrompt;
    [SerializeField]
    private Sprite _xboxPrompt;
    [Header("Target")]
    [SerializeField]
    private Image _glyphDisplay;

    // Unity methods
    private void Start()
    {
        // Subscribe to input manager
        _inputManager.OnInputDeviceChanged += DeviceChanged;
        _inputManager.OnControllerTypeChanged += ControllerChanged;

        // Hide and show glyphs for first frame
        HideAll();
        ShowGlyph(
            _inputManager.CurrentInputDevice,
            _inputManager.CurrentControllerType
            );
    }

    // Private Methods
    /// <summary>
    /// Shows the correct glyph based on the given input device.
    /// </summary>
    private void ShowGlyph(
        InputDeviceManager.Device device,
        InputDeviceManager.ControllerType controller = InputDeviceManager.ControllerType.None)
    {
        // What device is active?
        switch (device)
        {
            // Keyboard
            case InputDeviceManager.Device.Keyboard:
                ShowKeyboardGlyph();
                break;

            // Controller
            case InputDeviceManager.Device.Controller:
                ShowControllerGlyph(controller);
                break;

            // Unknown
            default:
                Debug.LogWarning($"Unknown input device: '{device.ToString()}'");
                break;
        }
    }
    /// <summary>
    /// Shows the controller glyph for this prompt.
    /// </summary>
    private void ShowControllerGlyph(InputDeviceManager.ControllerType controller)
    {
        // Unhide glyph
        _glyphDisplay.color = Color.white;

        // What type of controller is connected
        switch (controller)
        {
            // Playstation
            case InputDeviceManager.ControllerType.Playstation:
                _glyphDisplay.sprite = _playstationPrompt;
                break;

            // Xbox
            case InputDeviceManager.ControllerType.Xbox:
                _glyphDisplay.sprite = _xboxPrompt;
                break;

            // Unknown
            default:
                Debug.LogWarning($"Unknown controller type: '{controller.ToString()}'");
                break;
        }
    }
    /// <summary>
    /// Shows the keyboard glyph for this prompt.
    /// </summary>
    private void ShowKeyboardGlyph()
    {
        // Unhide glyph
        _glyphDisplay.color = Color.white;

        // Set keyboard glyph
        _glyphDisplay.sprite = _keyboardPrompt;
    }
    /// <summary>
    /// Hides all glyph displays.
    /// </summary>
    private void HideAll()
    {
        // Hide glyph display
        _glyphDisplay.color = Color.clear;
    }

    // Callbacks
    private void DeviceChanged(InputDeviceManager.Device device, InputDeviceManager.ControllerType controller)
    {
        ShowGlyph(device, controller);
    }
    private void ControllerChanged(InputDeviceManager.Device device, InputDeviceManager.ControllerType controller)
    {
        ShowGlyph(device, controller);
    }
}
