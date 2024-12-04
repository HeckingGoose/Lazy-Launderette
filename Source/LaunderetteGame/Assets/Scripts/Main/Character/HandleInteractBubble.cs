using UnityEngine;

public class HandleInteractBubble : MonoBehaviour
{
    // Const
    private const float ROTATION_RANGE = 5f;
    private const float TIME_BETWEEN_SWITCHES = 0.5f;

    // Editor variables
    [Header("Scene References")]
    [SerializeField]
    private GameObject _bubbleObject;
    [SerializeField]
    private CharacterData _characterData;
    [SerializeField]
    private Transform _eyeLevel;

    // Private variables
    private float _timer;
    private float _baseRotation;

    // Unity Methods
    private void Start()
    {
        // Cache starting rotation
        _baseRotation = transform.localRotation.z;
    }
    private void Update()
    {
        // If the bubble object is even active right now
        if (_bubbleObject.activeInHierarchy)
        {
            // Are we ready to do a new rotation?
            if (_timer > TIME_BETWEEN_SWITCHES)
            {
                // Reset timer
                _timer = 0;

                // Generate new rotation
                float offset = Random.value * ((ROTATION_RANGE / 2) - ROTATION_RANGE);

                // Apply new rotation
                _bubbleObject.transform.eulerAngles = new Vector3(
                    _bubbleObject.transform.eulerAngles.x,
                    _bubbleObject.transform.eulerAngles.y,
                    _baseRotation + offset
                    );
            }

            // Increment timer
            _timer += Time.deltaTime;
        }
    }

    // Public methods
    /// <summary>
    /// Sets this bubble to be visible
    /// </summary>
    public void ShowBubble()
    {
        // Set bubble to be active
        _bubbleObject.SetActive(true);
    }
    /// <summary>
    /// Sets this bubble to be hidden
    /// </summary>
    public void HideBubble()
    {
        // Set bubble to be not active
        _bubbleObject.SetActive(false);
    }
    /// <summary>
    /// Begins a conversation with the character referenced by this script.
    /// </summary>
    /// <param name="caller">The player requesting this conversation.</param>
    public void StartTalk(in PlayerInteractor caller)
    {
        // Log that something happened
        Debug.Log($"Conversation requested by {caller.gameObject.name}");

        // Tell caller that we're done here
        caller.StartTalking(in _characterData, in _eyeLevel);
    }
}
