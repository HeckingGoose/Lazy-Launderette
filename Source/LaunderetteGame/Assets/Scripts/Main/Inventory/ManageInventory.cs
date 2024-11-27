using UnityEngine;
using UnityEngine.UI;

public class ManageInventory : MonoBehaviour
{
    // Editor variables
    [SerializeField]
    private InventorySlot[] _slots;
    [SerializeField]
    private Image _heldItemDisplay;
    [SerializeField]
    private TranslateToWorldItem _toWorldItemHandler;
    [SerializeField]
    private AudioSource _itemDropAudioPlayer;
    [SerializeField]
    private AudioClip[] _itemDropSoundClips;

    // Private variables
    private int _currentSlot;
    private int _dropInputState = 0;

    // Unity methods
    private void Start()
    {
        // Select slot as 0
        SelectSlot(0);

        // Set this item to be a washing up bag
        TryAddItem(Inventory.Item.ClothesBag);
    }

    private void Update()
    {
        // Is drop axis being pressed, and is not already held
        if (Input.GetAxis("Drop") > 0 && _dropInputState < 2)
        {
            // Increment counter for tracking how long it has been held
            _dropInputState++;
        }
        // Has drop axis been released
        else if (Input.GetAxis("Drop") <= 0)
        {
            // Reset counter to 0
            _dropInputState = 0;
        }

        // Poll keys
        if (Input.GetAxis("Inv1") > 0 && _currentSlot != 0)
        {
            // Select 1st inventory slot
            SelectSlot(0);
        }
        else if (Input.GetAxis("Inv2") > 0 && _currentSlot != 1)
        {
            // Select 2nd inventory slot
            SelectSlot(1);
        }
        else if (Input.GetAxis("Inv3") > 0 && _currentSlot != 2)
        {
            // Select 3rd inventory slot
            SelectSlot(2);
        }

        // Drop current item if drop axis is pressed, but not held
        if (_dropInputState == 1 && _slots[_currentSlot].Item != Inventory.Item.None)
        {
            // What is the currently held item?
            switch (_slots[_currentSlot].Item)
            {
                // Play choccy drop sound
                case Inventory.Item.Choccy:
                    _itemDropAudioPlayer.clip = _itemDropSoundClips[0];
                    _itemDropAudioPlayer.Play();
                    break;

                // Play screws drop sound
                case Inventory.Item.Screws:
                // Play screwdriver drop sound
                case Inventory.Item.Screwdriver:
                    _itemDropAudioPlayer.clip = _itemDropSoundClips[2];
                    _itemDropAudioPlayer.Play();
                    break;

                // Play bag drop sound effect
                default:
                    _itemDropAudioPlayer.clip = _itemDropSoundClips[1];
                    _itemDropAudioPlayer.Play();
                    break;
            }

            // Drop item
            _toWorldItemHandler.DropItem(_slots[_currentSlot].TryRemoveItem());

            // Update visuals
            UpdateHand();
        }
    }

    // Public methods
    /// <summary>
    /// Increment the currently selected slot, automatically manages wrapping.
    /// </summary>
    public void IncrementSlot()
    {
        // Increment slot
        SlotIndex++;
    }
    /// <summary>
    /// Decrement the currently selected slot, automatically manages wrapping.
    /// </summary>
    public void DecrementSlot()
    {
        // Decrement slot
        SlotIndex--;
    }
    /// <summary>
    /// Given an item, attempts to add it to the inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="dropOnFail">When true, creates a worlditem on fail, when false, does nothing.</param>
    /// <returns>True on the item being added to the inventory, false on the item being spawned as a world item.</returns>
    public bool TryAddItem(Inventory.Item item, bool dropOnFail = true)
    {
        // Check if current slot is empty
        if (_slots[SlotIndex].Item == Inventory.Item.None)
        {
            // Add to this slot
            _slots[SlotIndex].SetItem(item);

            // Update the hand visuals and return true
            UpdateHand();
            return true;
        }

        // Set pointer to invalid value
        int pointer = -1;

        // Loop through all slots
        for (int i = 0; i < _slots.Length; i++)
        {
            // If this slot contains no item
            if (_slots[i].Item == Inventory.Item.None)
            {
                // Set this as the target slot and early out
                pointer = i;
                break;
            }
        }

        // If there are no valid slots to place an item in
        if (pointer < 0)
        {
            // Log that we have no space
            Debug.Log("Inventory is full.");

            // If we are to do nothing then early out
            if (!dropOnFail) { return false; }

            // Create a new item that would have otherwise been placed in our inventory
            _toWorldItemHandler.DropItem(item);

            // Return fail
            return false;
        }

        // Update item in slot
        _slots[pointer].SetItem(item);

        // Update the hand visuals and return true
        UpdateHand();
        return true;
    }
    /// <summary>
    /// Attempts to remove an item from the player's currently selected inventory slot.
    /// </summary>
    /// <returns>True on an item being removed, false if there is no item to remove.</returns>
    public bool TryRemoveItem()
    {
        // Call main method using current slot.
        return TryRemoveItem(SlotIndex);
    }
    /// <summary>
    /// Returns the enum representation of the currently selected item.
    /// </summary>
    public Inventory.Item GetHeldItem()
    {
        return _slots[_currentSlot].Item;
    }

    // Private methods
    /// <summary>
    /// Ensures that slot index remains within range.
    /// </summary>
    private void WrapSlot()
    {
        // Check lower bound
        if (_currentSlot < 0)
        {
            // Wrap
            _currentSlot = _slots.Length - 1;
            return;
        }

        // Check upper bound
        if (_currentSlot >= _slots.Length)
        {
            // Wrap
            _currentSlot = 0;
            return;
        }
    }
    /// <summary>
    /// Marks the slot at the given index as selected, and tells all other slots to be unselected.
    /// </summary>
    /// <param name="slotIndex">The index to mark as selected.</param>
    private void SelectSlot(int slotIndex)
    {
        // Ensure slot index is in range
        if (slotIndex >= _slots.Length ||
            slotIndex < 0
            )
        {
            // Early out
            return;
        }

        // Loop through all slots
        for (int i = 0; i < _slots.Length; i++)
        {
            // If the slot being selected is this one
            if (i == slotIndex)
            {
                _slots[i].Select();
            }

            // Otherwise
            else
            {
                // Deselect
                _slots[i].DeSelect();
            }
        }

        // Set slot
        _currentSlot = slotIndex;

        // Update visuals
        UpdateHand();
    }
    /// <summary>
    /// Attempts to remove the item at the provided index from the inventory.
    /// </summary>
    /// <param name="index">The index of the item to remove.</param>
    /// <returns>True on an item being removed, false if there is no item to remove.</returns>
    private bool TryRemoveItem(int index)
    {
        if (_slots[index].Item == Inventory.Item.None)
        {
            Debug.Log("Inventory slot is currently empty! Cannot remove item.");
            return false;
        }

        // Remove item
        _slots[index].TryRemoveItem();

        UpdateHand();
        return true;
    }
    /// <summary>
    /// Updates the visual representing the player's hand.
    /// </summary>
    private void UpdateHand()
    {
        // There is an item in the hand now
        if (_slots[_currentSlot].Item != Inventory.Item.None)
        {
            // Set the sprite and enable the image
            _heldItemDisplay.sprite = _slots[_currentSlot].Sprite;
            _heldItemDisplay.enabled = true;
        }
        // We need to remove an item from the hand
        else
        {
            // Hide the image
            _heldItemDisplay.enabled = false;
        }
    }

    // Accessors
    /// <summary>
    /// Gets or sets the index of the currently selected inventory slot.
    /// </summary>
    public int SlotIndex
    {
        get { return _currentSlot; }
        private set
        {
            // Set value
            _currentSlot = value;

            // Ensure valid
            WrapSlot();

            // Update visuals
            SelectSlot(_currentSlot);
        }
    }
}
