using UnityEngine;
using UnityEngine.InputSystem;

public class Rumbler : MonoBehaviour
{
    // Motor speeds per item
    private readonly (float L, float R) MS_DEFAULT = (0, 1f);
    private readonly (float L, float R) MS_EMPTYBAG = (0.4f, 0);
    private readonly (float L, float R) MS_CHOCCY = (0.2f, 0.75f);
    private readonly (float L, float R) MS_SCREW = (0, 0.3f);
    private readonly (float L, float R) MS_VENT = (1f, 0.5f);

    // Motor times per item
    private const float T_DEFAULT = 0.025f;
    private const float T_EMPTYBAG = 0.1f;
    private const float T_CHOCCY = 0.05f;
    private const float T_SCREW = 0.1f;
    private const float T_VENT = 0.3f;

    // Private variables
    private float _timeRemaining = -1;
    private bool _hapticsRunning = false;

    // Unity methods
    private void Update()
    {
        // Is timer
        if (_timeRemaining > 0)
        {
            // Decrement timer
            _timeRemaining -= Time.deltaTime;
        }

        // Otherwise
        else
        {
            // If we are running
            if (_hapticsRunning)
            {
                // Then let's stop
                _hapticsRunning = false;

                // Disable haptics if we have a controller
                if (Gamepad.all.Count > 0)
                {
                    Gamepad.current.ResetHaptics();
                }
            }
        }
    }

    // Public Methods
    /// <summary>
    /// Begins haptic feedback on the current controller for the given motor speed and time.
    /// </summary>
    /// <param name="leftSpeed">The motor speed of the low frequency motor (0-1).</param>
    /// <param name="rightSpeed">The motor speed of the high frequency motor (0-1).</param>
    /// <param name="time">The time to rumble for.</param>
    public void StartHaptics(float leftSpeed, float rightSpeed, float time)
    {
        // Cap values
        leftSpeed = Mathf.Clamp01(leftSpeed);
        rightSpeed = Mathf.Clamp01(rightSpeed);

        // Set haptics to enabled
        _hapticsRunning = true;

        // Set timer
        _timeRemaining = time;

        // Check if we have a gamepad
        if (Gamepad.all.Count > 0)
        {
            // Set motor speeds
            Gamepad.current.SetMotorSpeeds(leftSpeed, rightSpeed);
        }

        // Otherwise
        else
        {
            // Log error
            Debug.LogWarning("No gamepads connected for rumble!");
        }
    }
    /// <summary>
    /// Begins haptic feedback on the current controller for the given motor speed and time.
    /// </summary>
    /// <param name="item">The item to rumble for.</param>
    public void StartHaptics(Inventory.Item item)
    {
        // What item is it?
        switch (item)
        {
            // If no item then skip
            case Inventory.Item.None:
                return;

            // Choccy (Screwdriver can re-use this lighter sound)
            case Inventory.Item.Choccy:
            case Inventory.Item.Screwdriver:
                // Play a fast, tappy rumble
                StartHaptics(MS_CHOCCY.L, MS_CHOCCY.R, T_CHOCCY);
                break;

            // Empty clothes bag
            case Inventory.Item.EmptyBag:
                // Play an empty type of rumble
                StartHaptics(MS_EMPTYBAG.L, MS_EMPTYBAG.R, T_EMPTYBAG);
                break;

            // Screws (coins should never fire haptics (for now) but can re-use this as default)
            case Inventory.Item.Screws:
            case Inventory.Item.Coin:
                // Play a long tippy sound
                StartHaptics(MS_SCREW.L, MS_SCREW.R, T_SCREW);
                break;

            // Vent cover being removed
            case Inventory.Item.VentCover:
                // Play a long, deep rumble
                StartHaptics(MS_VENT.L, MS_VENT.R, T_VENT);
                break;

            // Item has no special sound
            default:
                // Play at default speeds
                StartHaptics(MS_DEFAULT.L, MS_DEFAULT.R, T_DEFAULT);
                break;
        }

        // Log Done
        Debug.Log($"Rumbled for '{item.ToString()}'");
    }
}
