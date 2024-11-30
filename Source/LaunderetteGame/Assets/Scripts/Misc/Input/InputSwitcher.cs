using UnityEngine;
using UnityEngine.InputSystem;

public class InputSwitcher : MonoBehaviour
{
    // Unity methods
    private void Start()
    {
        // Runs on any action
        InputSystem.onActionChange += ReadDeviceType;
    }

    // Private methods
    private void ReadDeviceType(object inputAction, InputActionChange changeDone)
    {
        // Check type
        if (!(inputAction is InputAction))
        {
            // Log error and early out
            Debug.LogWarning("Input action event returned incorrect type?");
            return;
        }

        // Cast action to action
        InputAction action = (InputAction)inputAction;

        // Given there is an active control is not null
        if (action.activeControl != null)
        {
            // Fetch last device used
            InputDevice lastDevice = action.activeControl.device;


            // Switch device type
            switch (lastDevice.layout.ToLower())
            {
                // KbnM layout
                case "mouse":
                case "keyboard":
                    GLOBAL.CurrentInputDevice = GLOBAL.InputMode.Keyboard;
                    break;

                // All other devices are assumed to be controller
                default:
                    GLOBAL.CurrentInputDevice = GLOBAL.InputMode.Controller;
                    break;
            }
        }
    }
}
