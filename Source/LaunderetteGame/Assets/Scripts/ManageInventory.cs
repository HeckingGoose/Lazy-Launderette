using UnityEngine;
using UnityEngine.UI;

public class ManageInventory : MonoBehaviour
{
    // Editor variables
    [SerializeField]
    private RectTransform[] inventorySlots;
    [SerializeField]
    private InventorySlot[] _slots;
    [SerializeField]
    private Image heldItemImage;
    [SerializeField]
    private float scaleAmount;
    [SerializeField]
    private float rotationAmount;
    [SerializeField]
    private float maxTimer;
    [SerializeField]
    private TranslateToWorldItem translate;
    [SerializeField]
    private AudioSource dropSource;
    [SerializeField]
    private AudioClip[] dropSounds;

    // Private variables
    private Vector3[] baseScales;
    private Vector3[] maxScales;
    private Vector3[] baseRotations;
    private Vector3[] maxRotations;
    public int currentSlot;
    private float[] timers;
    private int gState = 0;

    private void Start()
    {
        // Initialise inventory items
        baseScales = new Vector3[inventorySlots.Length];
        maxScales = new Vector3[inventorySlots.Length];
        baseRotations = new Vector3[inventorySlots.Length];
        maxRotations = new Vector3[inventorySlots.Length];
        timers = new float[inventorySlots.Length];

        for (int i = 0; i < _slots.Length; i++)
        {
            // Setup visual scaling stuff
            baseScales[i] = inventorySlots[i].localScale;
            maxScales[i] = baseScales[i] * scaleAmount;
            baseRotations[i] = inventorySlots[i].localEulerAngles;
            maxRotations[i] = new Vector3(
                baseRotations[i].x,
                baseRotations[i].y,
                baseRotations[i].z + rotationAmount
                );
            timers[i] = 0;
        }

        // Set first item to be a bag
        TryAddItem(0);

        currentSlot = 0;
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

        // Iterate through timers
        for (int i = 0; i < timers.Length; i++)
        {
            // Update timer value
            if (currentSlot == i)
            {
                timers[i] += Time.deltaTime;
                if (timers[i] > maxTimer)
                {
                    timers[i] = maxTimer;
                }
            }
            else
            {
                timers[i] -= Time.deltaTime;
                if (timers[i] < 0)
                {
                    timers[i] = 0;
                }
            }

            // Update scale and rotation of relevant inventory slot
            inventorySlots[i].localScale = Vector3.Lerp(baseScales[i], maxScales[i], timers[i] / maxTimer);
            inventorySlots[i].localEulerAngles = Vector3.Lerp(baseRotations[i], maxRotations[i], timers[i] / maxTimer);
        }

        // Poll keys
        if (Input.GetAxis("Inv1") > 0 && currentSlot != 0)
        {
            currentSlot = 0;
            UpdateHand();
        }
        else if (Input.GetAxis("Inv2") > 0 && currentSlot != 1)
        {
            currentSlot = 1;
            UpdateHand();
        }
        else if (Input.GetAxis("Inv3") > 0 && currentSlot != 2)
        {
            currentSlot = 2;
            UpdateHand();
        }

        // Drop current item if g is pressed
        if (gState == 1 && _slots[currentSlot].Item != InventorySlot.NO_ITEM)
        {
            switch (_slots[currentSlot].Item)
            {
                case 1: // Chocolate
                    dropSource.clip = dropSounds[0];
                    dropSource.Play();
                    break;
                case 5: // Screws
                case 4: // Screwdriver
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
    public void UpdateHand()
    {
        // There is an item in the hand now
        if (_slots[currentSlot].Item != InventorySlot.NO_ITEM)
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
    public bool TryAddItem(int itemID)
    {
        int pointer = -1;
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].Item == InventorySlot.NO_ITEM)
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
        _slots[pointer].SetItem(itemID);

        UpdateHand();
        return true;
    }
    public bool TryRemoveItem(int index)
    {
        if (_slots[index].Item == InventorySlot.NO_ITEM)
        {
            Debug.Log("Inventory slot is currently empty! Cannot remove item.");
            return false;
        }

        UpdateHand();
        return true;
    }
    /// <summary>
    /// Returns item name in lowercase.
    /// </summary>
    public string GetHeldItemName()
    {
        if (_slots[currentSlot].Item == InventorySlot.NO_ITEM)
        {
            return "empty";
        }
        else
        {
            return _slots[currentSlot].ItemName.ToLower();
        }
    }
}
