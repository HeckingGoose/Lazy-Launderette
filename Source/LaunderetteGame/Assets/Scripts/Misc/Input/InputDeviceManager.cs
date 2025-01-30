using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceManager : MonoBehaviour
{
    // Enum
    public enum Device
    {
        None,
        Keyboard,
        Controller,
        Touch
    }
    public enum ControllerType
    {
        None,
        Playstation,
        Xbox,
        Nintendo
    }

    // Events
    public event DeviceChangeEventHandler OnInputDeviceChanged;
    public event ControllerTypeChangeEventHandler OnControllerTypeChanged;

    // Delegates
    public delegate void DeviceChangeEventHandler(Device device, ControllerType controller);
    public delegate void ControllerTypeChangeEventHandler(Device device, ControllerType controller);

    // Private Variables
    private Device _currentInputDevice;
    private ControllerType _currentControllerType;

    // Unity methods
    private void Start()
    {
        // Subscribe to all action events
        InputSystem.onActionChange += ReadDeviceType;
    }

    // Private methods
    private void ReadDeviceType(object inputAction, InputActionChange changeDone)
    {
        // Are we allowed to auto-detect input device?
        if (Settings.InputTypeOverride != Device.None)
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

            // Cache last device and controller type
            Device dCache = _currentInputDevice;
            ControllerType cCache = _currentControllerType;

            // Switch device type
            switch (lastDevice.displayName.ToLower())
            {
                // KbnM layout
                case "mouse":
                case "keyboard":
                    CurrentInputDevice = Device.Keyboard;
                    break;

                // If the device is an xbox controller
                case "xbox controller":
                    // Set to controller
                    CurrentInputDevice = Device.Controller;
                    CurrentControllerType = ControllerType.Xbox;
                    break;

                // All other devices are assumed to be an controller
                default:
                    // Set to controller
                    CurrentInputDevice = Device.Controller;
                    break;
            }

            // Does the controller cache differ
            if (Settings.ControllerTypeOverride == ControllerType.None &&
                cCache != CurrentControllerType)
            {
                // Is anyone listening?
                if (OnControllerTypeChanged != null)
                {
                    // Then raise event
                    OnControllerTypeChanged.Invoke(CurrentInputDevice, CurrentControllerType);
                }
            }

            // Does the device cache differ? (and are we allowed)
            if (Settings.InputTypeOverride == Device.None &&
                dCache != CurrentInputDevice)
            {
                // Is anyone listening?
                if (OnInputDeviceChanged != null)
                {
                    // Then raise the event
                    OnInputDeviceChanged.Invoke(CurrentInputDevice, CurrentControllerType);
                }
            }
        }
    }

    // Accessors
    /// <summary>
    /// Gets or sets the current input device.
    /// </summary>
    public Device CurrentInputDevice
    {
        get { return Settings.InputTypeOverride == Device.None ? _currentInputDevice : Settings.InputTypeOverride; }
        private set { _currentInputDevice = value; }
    }
    /// <summary>
    /// Gets or sets the current controller type.
    /// </summary>
    public ControllerType CurrentControllerType
    {
        get { return Settings.ControllerTypeOverride == ControllerType.None ? _currentControllerType : Settings.ControllerTypeOverride; }
        private set { _currentControllerType = value; }
    }
}
