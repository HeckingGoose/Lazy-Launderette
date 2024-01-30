using UnityEngine;

public class Machine_Main : MonoBehaviour
{
    // Editor variables
    [SerializeField]
    private bool running;

    // Internal variables
    private Animator animator;
    private BoxCollider boxCollider;

    private void Start()
    {
        // Fetch components, throw error on fail
        bool success = TryGetComponent(out animator) ? TryGetComponent(out boxCollider) ? true : false : false;

        if (!success)
        {
            Debug.LogError($"Failed to fetch Animator/BoxCollider on {name}!");
        }
    }

    // Externally accessible methods
    public void ToggleDoor()
    {
        // If machine isn't currently running
        if (!running)
        {
            // Flip door state
            bool doorOpen = animator.GetBool("DoorOpen");
            animator.SetBool("DoorOpen", !doorOpen);
            boxCollider.enabled = doorOpen;
        }
        // If machine is running
        else
        {
            // Close door just in case
            animator.SetBool("DoorOpen", false);
        }
    }

    // Accessors
    public bool Running
    {
        get { return running; }
    }
}
