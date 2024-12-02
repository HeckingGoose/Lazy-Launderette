// Data storage class for player data
public class PlayerData
{
    // Variables
    public bool CrouchToggleEnabled { get; set; }
    public bool AllowAutoDetectInputType { get; set; }
    public bool AllowAutoDetectControllerType { get; set; }

    // Constructor
    public PlayerData(
        bool crouchToggleEnabled,
        bool allowAutoDetectInputType,
        bool allowAutoDetectControllerType
        )
    {
        // Pass in values
        CrouchToggleEnabled = crouchToggleEnabled;
        AllowAutoDetectInputType = allowAutoDetectInputType;
        AllowAutoDetectControllerType = allowAutoDetectControllerType;
    }
}
