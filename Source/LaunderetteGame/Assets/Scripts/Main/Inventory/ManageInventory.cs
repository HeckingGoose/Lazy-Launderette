using UnityEngine;
using UnityEngine.UI;

public class ManageInventory : MonoBehaviour
{
    // Editor variables
    [SerializeField]
    private InventorySlot[] _slots;
    [SerializeField]
    private Image heldItemImage;
    [SerializeField]
    private TranslateToWorldItem translate;
    [SerializeField]
    private AudioSource dropSource;
    [SerializeField]
    private AudioClip[] dropSounds;

    // Private variables
    public int currentSlot;
    private int gState = 0;

    private void Start()
    {
        // Set first item to be a bag
        TryAddItem(Inventory.Item.ClothesBag);

        SelectSlot(0);
    }

    private void Update()
    {
        // Handle g press
        if (Input.GetAxis("Drop") > 0 && gState < 2)
        {
            gState++;
        }
        else if (Input.GetAxis("Drop") <= 0)
        {
            gState = 0;
        }

        // Poll keys
        if (Input.GetAxis("Inv1") > 0 && currentSlot != 0)
        {
            SelectSlot(0);
            UpdateHand();
        }
        else if (Input.GetAxis("Inv2") > 0 && currentSlot != 1)
        {
            SelectSlot(1);
            UpdateHand();
        }
        else if (Input.GetAxis("Inv3") > 0 && currentSlot != 2)
        {
            SelectSlot(2);
            UpdateHand();
        }

        // Drop current item if g is pressed
        if (gState == 1 && _slots[currentSlot].Item != Inventory.Item.None)
        {
            switch (_slots[currentSlot].Item)
            {
                case Inventory.Item.Choccy: // Chocolate
                    dropSource.clip = dropSounds[0];
                    dropSource.Play();
                    break;
                case Inventory.Item.Screws: // Screws
                case Inventory.Item.Screwdriver: // Screwdriver
                    dropSource.clip = dropSounds[2];
                    dropSource.Play();
                    break;
                default: // Bag
                    dropSource.clip = dropSounds[1];
                    dropSource.Play();
                    break;
            }
            // Drop item
            translate.DropItem(_slots[currentSlot].TryRemoveItem());
            UpdateHand();
        }
    }

    // Other
    public void IncrementSlot()
    {
        // Increment slot
        currentSlot++;

        // Check for wrap
        WrapSlot();

        // Select
        SelectSlot(currentSlot);
    }
    public void DecrementSlot()
    {
        // Increment slot
        currentSlot--;

        // Check for wrap
        WrapSlot();

        // Select
        SelectSlot(currentSlot);
    }
    private void WrapSlot()
    {
        // Check lower bound
        if (currentSlot < 0)
        {
            // Wrap
            currentSlot = _slots.Length - 1;
            return;
        }

        // Check upper bound
        if (currentSlot >= _slots.Length)
        {
            // Wrap
            currentSlot = 0;
            return;
        }
    }
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
        currentSlot = slotIndex;
    }
    public void UpdateHand()
    {
        // There is an item in the hand now
        if (_slots[currentSlot].Item != Inventory.Item.None)
        {
            heldItemImage.sprite = _slots[currentSlot].Sprite;
            heldItemImage.enabled = true;
        }
        // We need to remove an item from the hand
        else
        {
            heldItemImage.enabled = false;
        }
    }
    public bool TryAddItem(Inventory.Item item)
    {
        int pointer = -1;
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].Item == Inventory.Item.None)
            {
                pointer = i;
                break;
            }
        }
        if (pointer < 0)
        {
            Debug.Log("Inventory is full.");
            return false;
        }

        // Update item in slot
        _slots[pointer].SetItem(item);

        UpdateHand();
        return true;
    }
    public bool TryRemoveItem(int index)
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
    /// Returns the enum representation of the currently selected item.
    /// </summary>
    public Inventory.Item GetHeldItem()
    {
        return _slots[currentSlot].Item;
    }
}
