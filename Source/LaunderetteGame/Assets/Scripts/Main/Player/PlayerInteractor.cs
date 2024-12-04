using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInteractor : MonoBehaviour
{
    // Const
    private const string TAG_WASHINGMACHINE = "Machine";
    private const string TAG_CHARACTER = "Talkable";
    private const string TAG_TOOL = "Pickup";
    private const string TAG_GOAL = "WashSpot";
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

    // Editor variables
    [Header("Player Components")]
    [SerializeField]
    private PlayerController _playerController;
    [SerializeField]
    private ManageInventory _inventory;
    [Header("Conversation References")]
    [SerializeField]
    private GameObject _conversationRoot;
    [SerializeField]
    private ManageConversation _conversationManager;
    [Header("Raycasting Definitions")]
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private float _rayDistance = 2f;
    [Header("UI Settings & References")]
    [SerializeField]
    private float _crossHairTimerMax = 0.15f;
    [SerializeField]
    private float _crosshairMaxScale = 1.5f;
    [SerializeField]
    private RectTransform _crosshairTransform;
    [SerializeField]
    private Image _crosshairImage;
    [SerializeField]
    private TextMeshProUGUI _describeText;
    [Header("Coin Manager")]
    [SerializeField]
    private ManageCoins _coinManager;
    [Header("Sound Effects")]
    [SerializeField]
    private AudioClip _coinPickupSound;
    [SerializeField]
    private AudioSource _coinAudioSource;
    [SerializeField]
    private AudioSource _pickupSoundSource;
    [SerializeField]
    private AudioClip[] _pickupSounds;
    [Header("Rumble")]
    [SerializeField]
    private Rumbler _rumbler;
    [Header("Scene End Script")]
    [SerializeField]
    private EndScene _endSceneScript;

    // Private variables
#nullable enable
    private HandleInteractBubble? _interactBubbleHandle;
#nullable enable
    private GameObject? _targetObject;
    private bool _talking = false;
    private float _crosshairTimer = 0;
    private Vector3 _crosshairSizeBase;
    private Vector3 _crosshairSizeMax;
    private Vector4 _crosshairColourBase;
    private Vector4 _crosshairColourMax;

    // Public
    [HideInInspector]
    public bool Enabled = true;

    // Action map
#nullable enable
    private InputActionMap? _freeRoamActionMap;

    // Actions
#nullable enable
    private InputAction? _interactAction;

    // Unity Methods
    private void Start()
    {
        // Fetch crosshair base state
        _crosshairSizeBase = _crosshairTransform.localScale;
        _crosshairColourBase = _crosshairImage.color;

        // Calculate values for max states
        _crosshairSizeMax = _crosshairSizeBase * _crosshairMaxScale;
        _crosshairColourMax = _crosshairImage.color;
        _crosshairColourMax.w = 1;

        // Fetch action map
        _freeRoamActionMap = InputSystem.actions.FindActionMap(InputDefinitions.ACTIONMAP_FREEROAM);

        // Fetch relevant actions
        _interactAction = _freeRoamActionMap.FindAction(InputDefinitions.FRAM_INTERACT);

        // Subscribe to interact action
        _interactAction.performed += DoInteract;
    }

    private void FixedUpdate()
    {
        // Make a ray from the middle of the screen
        Ray screenRay = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // Do raycast, true on hit
        if (Physics.Raycast(screenRay, out RaycastHit hit, _rayDistance))
        {
            // Set target to hit
            _targetObject = hit.transform.gameObject;
        }
        // We didn't hit anything
        else
        {
            // Set target to null
            _targetObject = null;
        }
    }
    private void Update()
    {
        // Handle crosshair display animation
        AnimateCrosshair();

        // Check if we are looking at something
        if (_targetObject != null)
        {
            // What are we looking at?
            switch (_targetObject.tag)
            {
                // Object is a character
                case TAG_CHARACTER:
                    // If we are not currently talking
                    if (!_talking)
                    {
                        // Display the interaction text as 'Talk'
                        _describeText.text = DESCRIBETEXT_TALK;

                        // Attempt to fetch a handle to the interact bubble
                        _targetObject.TryGetComponent<HandleInteractBubble>(out _interactBubbleHandle);

                        // If we got anything
                        if (_interactBubbleHandle != null)
                        {
                            // Show the bubble
                            _interactBubbleHandle.ShowBubble();
                        }
                    }

                    // Given we are in a conversation
                    else
                    {
                        // Check we have a reference to a bubble
                        if (_interactBubbleHandle != null)
                        {
                            // Hide it
                            _interactBubbleHandle.HideBubble();
                        }

                        // Blank out the display text
                        _describeText.text = string.Empty;
                    }
                    break;

                // Object is a pickup
                case TAG_TOOL:
                    // Set describe text accordingly
                    _describeText.text = DESCRIBETEXT_PICKUP;

                    // Attempt to fetch a reference to its pickup script
                    _targetObject.TryGetComponent(out HandlePickup pickupHandle);

                    // If we found something
                    if (pickupHandle != null)
                    {
                        // If this is Sam's washed clothes
                        if (pickupHandle.Item == Inventory.Item.WashedClothes)
                        {
                            // Do we have a bag
                            if (_inventory.GetHeldItem() != Inventory.Item.EmptyBag)
                            {
                                // Inform player that they need a bag
                                _describeText.text = DESCRIBETEXT_NEEDCLEANBAG;
                            }

                            // Otherwise
                            else
                            {
                                // Inform player they can pickup
                                _describeText.text = DESCRIBETEXT_PICKUP;
                            }
                        }

                        // If this is a vent cover
                        else if (pickupHandle.Item == Inventory.Item.VentCover)
                        {
                            // If the player has a screwdriver
                            if (_inventory.GetHeldItem() == Inventory.Item.Screwdriver)
                            {
                                // Inform the player that they can remove the vent
                                _describeText.text = DESCRIBETEXT_PICKUPVENT;
                            }

                            // If they do not have a screwdriver
                            else
                            {
                                // Inform the player that they need a screwdriver
                                _describeText.text = DESCRIBETEXT_NEEDSCREWDRIVER;
                            }
                        }

                        // Otherwise
                        else
                        {
                            // Check inventory capacity for full (given the item is not a coin)
                            if (_inventory.IsFull && pickupHandle.Item != Inventory.Item.Coin)
                            {
                                // Show full text
                                _describeText.text = DESCRIBETEXT_INVENTORYFULL;
                            }
                        }
                    }

                    // Otherwise
                    else
                    {
                        // Do non-looking code
                        LookingAtNothing();

                        // Early out to avoid timer update
                        return;
                    }
                    break;

                // Object is a washing machine
                case TAG_WASHINGMACHINE:
                    // Set describe text
                    _describeText.text = DESCRIBETEXT_MACHINEDOOR;

                    // Try to fetch script for this machine (It's a messed up hierarchy)
                    _targetObject.transform.parent.parent.parent.TryGetComponent(out Machine_Main machineMainScript);

                    // If we found something
                    if (machineMainScript != null)
                    {
                        // If the machine is running
                        if (machineMainScript.Running)
                        {
                            // Change text accordingly
                            _describeText.text = DESCRIBETEXT_MACHINEINUSE;
                        }
                    }
                    break;

                // Object is a washing up spot
                case TAG_GOAL:
                    // Do we have enough coins to wash clothes?
                    if (_coinManager.CoinCount >= GameRules.COINS_TOWIN)
                    {
                        // Are we holding a clothes bag?
                        if (_inventory.GetHeldItem() == Inventory.Item.ClothesBag)
                        {
                            // Inform the player that they can wash their clothes
                            _describeText.text = DESCRIBETEXT_PROMPTWASHCLOTHES;
                        }

                        // We are not holding the clothes bag
                        else
                        {
                            // Inform the player that they need to hold their clothes bag
                            _describeText.text = DESCRIBETEXT_NEEDTOHOLDCLOTHESBAG;
                        }
                    }

                    // We do not have enough coins
                    else
                    {
                        // Generate text for player not having enough coins
                        _describeText.text = $"£{_coinManager.CoinCount}/£{GameRules.COINS_TOWIN}, cannot afford yet";
                    }
                    break;

                // We are not looking at anything important
                default:
                    // Run not-looking code
                    LookingAtNothing();

                    // Early out to avoid timer update
                    return;
            }

            // Increment crosshair animation timer, cap at max size
            _crosshairTimer = Mathf.Min(_crosshairTimer + Time.deltaTime, _crossHairTimerMax);
        }

        // If we are not looking at something
        else
        {
            // Run the method for if we are looking at nothing
            LookingAtNothing();
        }
    }

    // Private methods
    /// <summary>
    /// Updates all relevant values for as if the player was not looking at anything.
    /// </summary>
    private void LookingAtNothing()
    {
        // Reset describe text to be blank
        _describeText.text = string.Empty;

        // Check if we have a reference to an interact bubble
        if (_interactBubbleHandle != null)
        {
            // Hide it
            _interactBubbleHandle.HideBubble();

            // Get rid of the reference
            _interactBubbleHandle = null;
        }

        // Decrement crosshair animation timer, cap at 0
        _crosshairTimer = Mathf.Max(_crosshairTimer - Time.deltaTime, 0);
    }

    /// <summary>
    /// When called, updates the crosshair visual.
    /// </summary>
    private void AnimateCrosshair()
    {
        // Update scale
        _crosshairTransform.localScale = Vector3.Lerp(
                    _crosshairSizeBase, // Start
                    _crosshairSizeMax, // End
                    _crosshairTimer / _crossHairTimerMax // Progress
                    );

        // Update colour
        _crosshairImage.color = Vector4.Lerp(
            _crosshairColourBase, // Start
            _crosshairColourMax, // End
            _crosshairTimer / _crossHairTimerMax // Progress
            );
    }

    // Public methods
    /// <summary>
    /// Attempts to begin a conversation.
    /// </summary>
    /// <param name="characterData">The character data to start the conversation about.</param>
    /// <param name="target">The target transform of the conversation.</param>
    public void StartTalking(in CharacterData characterData, in Transform target)
    {
        if (target != null && characterData.conversations != null)
        {
            // Set required variables
            _talking = true;
            Debug.Log($"Successfully started conversation with {characterData.name}.");

            // Unhide conversation thingy
            _conversationRoot.SetActive(true);

            // Disable movement and looking
            _playerController.enabled = false;

            // Unhide and unlock mouse
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            // Move into conversation
            _conversationManager.StartTalk(in characterData, in target, Inventory.GetItemName(_inventory.GetHeldItem()));
        }

    }
    /// <summary>
    /// Ends the current conversation, if there is one
    /// </summary>
    public void EndTalk()
    {
        // Is there a conversation
        if (!_talking)
        {
            // If not then early out
            Debug.LogWarning("Attempted to exit conversation while not talking!");
            return;
        }

        // Hide mouse again
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Enable movement and looking
        _playerController.enabled = true;

        // Hide conversation box
        _conversationRoot.SetActive(false);

        // Reset talking state and log
        _talking = false;
        Debug.Log("Successfully closed conversation.");
    }
    /// <summary>
    /// When called, attempts to interact with whatever the player is (or isn't looking at). (Does nothing in that second case).
    /// </summary>
    public void Interact()
    {
        // If we are not targetting anything
        if (_targetObject == null)
        {
            // Log and early out
            Debug.LogWarning("Attempted to interact with no target set!");
            return;
        }

        // Otherwise, what're we looking at?
        switch (_targetObject.tag)
        {
            // This is a character we can speak to
            case TAG_CHARACTER:
                // Attempt to fetch speech bubble script reference
                _targetObject.TryGetComponent<HandleInteractBubble>(out HandleInteractBubble speechHandler);

                // Check success
                if (speechHandler != null)
                {
                    // Attempt to start a chat
                    _describeText.text = string.Empty;
                    speechHandler.StartTalk(this);
                }
                // Otherwise
                else
                {
                    // Log error
                    Debug.LogError("Character being interacted with has no script!");
                }
                break;

            // This is something we can pick up
            case TAG_TOOL:
                // Fetch the pickup handler for this tool
                _targetObject.TryGetComponent(out HandlePickup pickupHandler);

                // Check success
                if (pickupHandler != null)
                {
                    // Predefine success
                    bool success;

                    // What type of item is this
                    switch (pickupHandler.Item)
                    {
                        // A coin
                        case Inventory.Item.Coin:
                            // Increment coin counter
                            _coinManager.CoinCount++;

                            // Playback pickup sound
                            _coinAudioSource.clip = _coinPickupSound;
                            _coinAudioSource.Play();

                            // Destroy coin
                            Destroy(pickupHandler.gameObject);

                            // Call rumbler with item type
                            _rumbler.StartHaptics(pickupHandler.Item);
                            break;

                        // Washed clothes
                        case Inventory.Item.WashedClothes:
                            // Are we holding an empty bag
                            if (_inventory.GetHeldItem() == Inventory.Item.EmptyBag)
                            {
                                // Remove bag
                                _inventory.TryRemoveItem();

                                // Add full bag
                                success = _inventory.TryAddItem(Inventory.Item.BagHoldingSamsClothes);

                                // If we were able to add the item
                                if (success)
                                {
                                    // Play pickup sounds
                                    _pickupSoundSource.clip = _pickupSounds[1];
                                    _pickupSoundSource.Play();
                                }

                                // Destroy clothes pile
                                Destroy(pickupHandler.gameObject);

                                // Call rumbler with item type
                                _rumbler.StartHaptics(pickupHandler.Item);
                            }
                            break;

                        // A Vent Cover
                        case Inventory.Item.VentCover:
                            // If we are holding a screwdriver
                            if (_inventory.GetHeldItem() == Inventory.Item.Screwdriver)
                            {
                                // Add screws
                                success = _inventory.TryAddItem(Inventory.Item.Screws);

                                // If succeeded
                                if (success)
                                {
                                    // Then play pickup sound
                                    _pickupSoundSource.clip = _pickupSounds[2];
                                    _pickupSoundSource.Play();
                                }

                                // Remove vent
                                Destroy(pickupHandler.gameObject);

                                // Play vent destroy sound effect
                                _pickupSoundSource.clip = _pickupSounds[3];
                                _pickupSoundSource.Play();

                                // Call rumbler with item type
                                _rumbler.StartHaptics(pickupHandler.Item);
                            }
                            break;

                        // Any other type of item
                        default:
                            // Add item to inventory
                            success = _inventory.TryAddItem(pickupHandler.Item, false);

                            // If we succeeded
                            if (success)
                            {
                                // Destroy object
                                Destroy(pickupHandler.gameObject);

                                // Playback correct sound file
                                switch (pickupHandler.Item)
                                {
                                    case Inventory.Item.Choccy: // Chocolate
                                        _pickupSoundSource.clip = _pickupSounds[0];
                                        _pickupSoundSource.Play();
                                        break;
                                    case Inventory.Item.Screws: // screws
                                    case Inventory.Item.Screwdriver: // Screwdriver
                                        _pickupSoundSource.clip = _pickupSounds[2];
                                        _pickupSoundSource.Play();
                                        break;
                                    default: // Bag
                                        _pickupSoundSource.clip = _pickupSounds[1];
                                        _pickupSoundSource.Play();
                                        break;
                                }

                                // Call rumbler with item type
                                _rumbler.StartHaptics(pickupHandler.Item);
                            }

                            break;
                    }
                }
                // Otherwise
                else
                {
                    // Log error
                    Debug.LogError("Tool does not have pickup script!");
                }
                break;

            // This is a washing machine
            case TAG_WASHINGMACHINE:
                // Try to fetch the machine's script
                _targetObject.transform.parent.parent.parent.TryGetComponent(out Machine_Main machineScript);

                // Check success
                if (machineScript != null)
                {
                    // Toggle the machine's door
                    machineScript.ToggleDoor();
                }
                // Otherwise
                else
                {
                    // Log error
                    Debug.LogError("Washing machine has no main script!");
                }
                break;

            // This is the area to place our clothes in (to win the game)
            case TAG_GOAL:
                // If we have enough coins and that we are holding a bag
                if (_coinManager.CoinCount < GameRules.COINS_TOWIN || _inventory.GetHeldItem() != Inventory.Item.ClothesBag)
                {
                    // Early out
                    return;
                }

                // Rob the player
                _coinManager.CoinCount -= GameRules.COINS_TOWIN;

                // Attempt to
                try
                {
                    // End the scene
                    _endSceneScript.Begin();
                }
                // On error
                catch
                {
                    // Log error
                    Debug.LogError("Failed to run scene end!");

                    // Jump to the menu
                    GLOBAL.LoadTarget = "Menu";
                    SceneManager.LoadScene("Loading");
                }
                break;
        }
    }
    /// <summary>
    /// Subscribed to the interact action <3
    /// </summary>
    /// <param name="obj">Callback params of the input action</param>
    private void DoInteract(InputAction.CallbackContext obj)
    {
        // Are we allowed to do our job?
        if (Enabled)
        {
            // Do interact
            Interact();
        }
    }
}
