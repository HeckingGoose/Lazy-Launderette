public static class GLOBAL
{
    // Public values
    public enum InputMode
    {
        Keyboard,
        Controller
    }
    public enum ControllerType
    {
        Playstation,
        Xbox
    }
    public static string LoadTarget = "Menu";
    public static InputMode CurrentInputDevice = InputMode.Keyboard;
    public static ControllerType CurrentControllerType = ControllerType.Playstation;


    // User data
    public static PlayerData RuntimePlayerData = new PlayerData(
        crouchToggleEnabled: false,
        allowAutoDetectInputType: true,
        allowAutoDetectControllerType: true
        );
}
