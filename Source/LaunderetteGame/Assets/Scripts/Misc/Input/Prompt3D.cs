using UnityEngine;

public class Prompt3D : MonoBehaviour
{
    // Editor variables
    [Header("Representing:")]
    [SerializeField]
    private Texture2D _keyboardPrompt;
    [SerializeField]
    private Texture2D _playstationPrompt;
    [SerializeField]
    private Texture2D _xboxPrompt;
    [Header("Target")]
    [SerializeField]
    private Material _glyphDisplay;

    // Private variables
    private GLOBAL.InputMode _deviceLastFrame;
    private GLOBAL.ControllerType _controllerLastFrame;

    // Unity methods
    private void Start()
    {
        // Hide and show glyphs for first frame
        HideAll();
        ShowGlyph();
    }
    private void Update()
    {
        // Check if input device or controller type has changed
        if (_deviceLastFrame != GLOBAL.CurrentInputDevice ||
            _controllerLastFrame != GLOBAL.CurrentControllerType
            )
        {
            // Hide all displays
            HideAll();

            // Update the device and type trackers
            _deviceLastFrame = GLOBAL.CurrentInputDevice;
            _controllerLastFrame = GLOBAL.CurrentControllerType;

            // Show correct glyph
            ShowGlyph();
        }
    }

    // Methods
    /// <summary>
    /// Shows the correct glyph based on the current input device.
    /// </summary>
    private void ShowGlyph()
    {
        // What device is active?
        switch (_deviceLastFrame)
        {
            // Keyboard
            case GLOBAL.InputMode.Keyboard:
                ShowKeyboardGlyph();
                break;

            // Controller
            case GLOBAL.InputMode.Controller:
                ShowControllerGlyph();
                break;

            // Unknown
            default:
                Debug.LogWarning($"Unknown input device: '{_deviceLastFrame.ToString()}'");
                break;
        }
    }
    /// <summary>
    /// Shows the controller glyph for this prompt.
    /// </summary>
    private void ShowControllerGlyph()
    {
        // Unhide glyph
        _glyphDisplay.color = Color.white;

        // What type of controller is connected
        switch (_controllerLastFrame)
        {
            // Playstation
            case GLOBAL.ControllerType.Playstation:
                _glyphDisplay.SetTexture("_MainTex", _playstationPrompt);
                break;

            // Xbox
            case GLOBAL.ControllerType.Xbox:
                _glyphDisplay.SetTexture("_MainTex", _xboxPrompt);
                break;

            // Unknown
            default:
                Debug.LogWarning($"Unknown controller type: '{_controllerLastFrame.ToString()}'");
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
        _glyphDisplay.SetTexture("_MainTex", _keyboardPrompt);
    }
    /// <summary>
    /// Hides all glyph displays.
    /// </summary>
    private void HideAll()
    {
        // Hide glyph display
        _glyphDisplay.color = Color.clear;
    }
}
