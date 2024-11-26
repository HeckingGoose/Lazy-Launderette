using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
public class GlyphDealer : MonoBehaviour
{
    // Editor variables
    [Header("Glyph Sets")]
    [SerializeField]
    private Sprite[] _xbox360Sprites;
    [SerializeField]
    private Sprite[] _playstation3Sprites;
    [Header("Error Glyph")]
    [SerializeField]
    private Sprite _errorSprite;

    // Private stuff
    private Dictionary<GamepadButton, int> _playstation3Map = new Dictionary<GamepadButton, int>
    {
        { GamepadButton.Y, 0 },
        { GamepadButton.B, 1 },
        { GamepadButton.A, 2 },
        { GamepadButton.X, 3 },
        { GamepadButton.DpadUp, 4 },
        { GamepadButton.DpadRight, 5 },
        { GamepadButton.DpadDown, 6 },
        { GamepadButton.DpadLeft, 7 },
        { GamepadButton.LeftStick, 8 },
        { GamepadButton.RightStick, 10 },
        { GamepadButton.LeftShoulder, 12 },
        { GamepadButton.RightShoulder, 13 },
        { GamepadButton.LeftTrigger, 14 },
        { GamepadButton.RightTrigger, 15 },
        { GamepadButton.Select, 17 },
        { GamepadButton.Start, 18 },
    };
}
