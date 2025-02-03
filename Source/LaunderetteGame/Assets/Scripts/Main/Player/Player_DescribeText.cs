using TMPro;
using UnityEngine;

public class Player_DescribeText : MonoBehaviour
{
    // Const
    private const string DESCRIBETEXt_EMPTY = "";
    private const string DESCRIBETEXT_TALK = "Talk";
    private const string DESCRIBETEXT_PICKUP = "Pickup";
    private const string DESCRIBETEXT_INVENTORYFULL = "Inventory Full";
    private const string DESCRIBETEXT_NEEDCLEANBAG = "Need empty bag";
    private const string DESCRIBETEXT_PICKUPVENT = "Remove";
    private const string DESCRIBETEXT_NEEDSCREWDRIVER = "Needs a screwdriver";
    private const string DESCRIBETEXT_MACHINEDOOR = "Toggle Door";
    private const string DESCRIBETEXT_MACHINEINUSE = "Someone else is using this machine";
    private const string DESCRIBETEXT_PROMPTWASHCLOTHES = "Wash clothes?";
    private const string DESCRIBETEXT_NEEDTOHOLDCLOTHESBAG = "Select clothes before using machine.";

    // Editor Variables
    [Header("Player Interact Script")]
    [SerializeField]
    private Player_Interact _player;
    [Header("Text Box Display")]
    [SerializeField]
    private TextMeshProUGUI _display;

    // Unity Methods
    private void Start()
    {
        // Subscribe to look object change event
        _player.OnLookTargetChanged += LookTargetChanged;
    }

    // Private Methods
    private void LookTargetChanged(GameObject lookTarget)
    {
        // Are we looking at anything?
        if (lookTarget == null)
        {
            // We aren't so hide text
            _display.text = DESCRIBETEXt_EMPTY;
        }

        // Otherwise
        else
        {
            // We are looking at something, but what?
            switch (lookTarget.tag)
            {
                // It's a washing machine
                case Player_Interact.TAG_WASHINGMACHINE:
                    // Set the text to imply we can use the machine
                    _display.text = DESCRIBETEXT_MACHINEDOOR;

                    // Let's try to find this machine's script
                    lookTarget.transform.parent.parent.parent.TryGetComponent(out Machine_Main machineMainScript);

                    // If we found something, we can use it to runcheck the machine
                    if (machineMainScript != null && machineMainScript.Running) { _display.text = DESCRIBETEXT_MACHINEINUSE; }
                    break;

                // It's a character
                case Player_Interact.TAG_CHARACTER:
                    // Show the talk prompt
                    _display.text = DESCRIBETEXT_TALK;
                    break;

                // It's an item we can interact with
                case Player_Interact.TAG_TOOL:
                    break;

                // It's our goal
                case Player_Interact.TAG_GOAL:
                    break;

                // We don't know this item
                default:
                    // Set text to blank
                    _display.text = DESCRIBETEXt_EMPTY;

                    // Log issue
                    Debug.LogWarning($"Attempted to describe unknown tag '{lookTarget.tag}'!");
                    break;
            }
        }
    }
}