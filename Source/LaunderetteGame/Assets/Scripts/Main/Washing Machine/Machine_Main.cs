using UnityEngine;

public class Machine_Main : MonoBehaviour
{
    // Editor variables
    [Header("Is the machine in use?")]
    [SerializeField]
    private bool _running;

    // Private variables
    private Animator _animator;

    // Unity Methods
    private void Start()
    {
        // Fetch components, return false on any fail
        bool success = TryGetComponent(out _animator);

        // Did we fail
        if (!success)
        {
            // Log the error
            Debug.LogError($"Failed to fetch Animator on {name}!");
        }
    }

    // Externally accessible methods
    /// <summary>
    /// Toggles the door state between open and closed, as long as the machine is not running.
    /// </summary>
    public void ToggleDoor()
    {
        // If machine isn't currently running
        if (!_running)
        {
            // Flip door state
            bool doorOpen = _animator.GetBool("DoorOpen");
            _animator.SetBool("DoorOpen", !doorOpen);
        }
        // If machine is running
        else
        {
            // Close door just in case
            _animator.SetBool("DoorOpen", false);
        }
    }

    // Accessors
    /// <summary>
    /// Fetches whether this washing machine is in use or not.
    /// </summary>
    public bool Running
    {
        get { return _running; }
    }
}
