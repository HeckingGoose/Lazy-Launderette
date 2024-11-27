using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyPrompt : MonoBehaviour
{
    // Editor variables
    [Header("Representing:")]
    [SerializeField]
    private string _keyboardPrompt = "None";
    [SerializeField]
    private Sprite _controllerPrompt;
    [Header("Targets")]
    [SerializeField]
    private TextMeshProUGUI _keyboardDisplay;
    [SerializeField]
    private Image _controllerDisplay;

    // Private variables
    private GLOBAL.InputMode _deviceLastFrame;

    // Unity methods
    private void Start()
    {
        // Hide and show glyphs for first frame
        HideAll();
        ShowGlyph();
    }
    private void Update()
    {
        // Check if input device has changed
        if (_deviceLastFrame != GLOBAL.CurrentInputDevice)
        {
            // Hide all displays
            HideAll();

            // Show correct glyph
            ShowGlyph();

            // Update the device tracker
            _deviceLastFrame = GLOBAL.CurrentInputDevice;
        }
    }

    // Methods
    /// <summary>
    /// Shows the correct glyph based on the current input device.
    /// </summary>
    private void ShowGlyph()
    {
        // What device is active?
        switch (GLOBAL.CurrentInputDevice)
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
        // Unhide controller glyph
        _controllerDisplay.color = Color.white;

        // Set controller glyph
        _controllerDisplay.sprite = _controllerPrompt;
    }
    /// <summary>
    /// Shows the keyboard glyph for this prompt.
    /// </summary>
    private void ShowKeyboardGlyph()
    {
        // Unhide keyboard glyph
        _keyboardDisplay.text = _keyboardPrompt;
    }
    /// <summary>
    /// Hides all glyph displays.
    /// </summary>
    private void HideAll()
    {
        // Hide keyboard display
        _keyboardDisplay.text = string.Empty;

        // Hide controller display
        _controllerDisplay.color = Color.clear;
    }
}
