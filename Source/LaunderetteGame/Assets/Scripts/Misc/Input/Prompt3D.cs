using UnityEngine;

public class Prompt3D : MonoBehaviour
{
    // Editor variables
    [Header("Representing:")]
    [SerializeField]
    private Material _keyboardPrompt;
    [SerializeField]
    private Material _playstationPrompt;
    [SerializeField]
    private Material _xboxPrompt;
    [Header("Target")]
    [SerializeField]
    private MeshRenderer _glyphDisplay;

    // Unity methods
    private void Start()
    {
        // Subscribe to input manager
        InputDeviceManager.OnInputDeviceChanged += DeviceChanged;
        InputDeviceManager.OnControllerTypeChanged += ControllerChanged;

        // Hide and show glyphs for first frame
        HideAll();
        ShowGlyph();
    }

    // Private Methods
    /// <summary>
    /// Alternate version of ShowGlyph that directly uses InputDeviceManager for the current device.
    /// </summary>
    private void ShowGlyph()
    {
        ShowGlyph(
            InputDeviceManager.CurrentInputDevice,
            InputDeviceManager.CurrentControllerType
            );
    }
    /// <summary>
    /// Shows the correct glyph based on the given input device.
    /// </summary>
    private void ShowGlyph(InputDeviceManager.Device device,
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
        _glyphDisplay.enabled = true;

        // What type of controller is connected
        switch (controller)
        {
            // Playstation
            case InputDeviceManager.ControllerType.Playstation:
                _glyphDisplay.material = _playstationPrompt;
                break;

            // Xbox
            case InputDeviceManager.ControllerType.Xbox:
                _glyphDisplay.material = _xboxPrompt;
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
        _glyphDisplay.enabled = true;

        // Set keyboard glyph
        _glyphDisplay.material = _keyboardPrompt;
    }
    /// <summary>
    /// Hides all glyph displays.
    /// </summary>
    private void HideAll()
    {
        // Hide glyph display
        _glyphDisplay.enabled = false;
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
