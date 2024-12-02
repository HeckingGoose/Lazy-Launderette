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
        // Are we allowed to auto-detect input device?
        if (!GLOBAL.RuntimePlayerData.AllowAutoDetectInputType)
        {
            // Early out if not allowed
            return;
        }

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
            switch (lastDevice.displayName.ToLower())
            {
                // KbnM layout
                case "mouse":
                case "keyboard":
                    GLOBAL.CurrentInputDevice = GLOBAL.InputMode.Keyboard;
                    break;

                // If the device is an xbox controller
                case "xbox controller":
                    // Set to controller
                    GLOBAL.CurrentInputDevice = GLOBAL.InputMode.Controller;

                    // Are we allowed to detect controller type?
                    if (GLOBAL.RuntimePlayerData.AllowAutoDetectControllerType)
                    {
                        // Set type to xbox
                        GLOBAL.CurrentControllerType = GLOBAL.ControllerType.Xbox;
                    }
                    break;

                // All other devices are assumed to be an x360 controller
                default:
                    // Set to controller
                    GLOBAL.CurrentInputDevice = GLOBAL.InputMode.Controller;

                    // Are we allowed to detect controller type?
                    if (GLOBAL.RuntimePlayerData.AllowAutoDetectControllerType)
                    {
                        // Set type to xbox
                        GLOBAL.CurrentControllerType = GLOBAL.ControllerType.Xbox;
                    }
                    break;
            }
        }
    }
}
