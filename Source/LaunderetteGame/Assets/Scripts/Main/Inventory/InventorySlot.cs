using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    // Editor values
    [Header("Inventory")]
    [SerializeField]
    private Inventory _inventory;
    [Header("Item Display")]
    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private TextMeshProUGUI _itemText;
    [Header("Drop Prompt")]
    [SerializeField]
    private GameObject _dropPrompt;
    [SerializeField]
    private GameObject _nextPrompt;
    [Header("Self Transform")]
    [SerializeField]
    private RectTransform _selfTransform;
    [Header("Animation")]
    [SerializeField]
    private float _scaleMultiplier = 1.2f;
    [SerializeField]
    private float _rotationAmount = 10;
    [SerializeField]
    private float _animationTime = 0.1f;

    // Private values
    private Inventory.Item _item = Inventory.Item.None;
    private bool _selected = false;
    private float _animationTimer = 0;
    private Vector3 _baseScale;
    private Vector3 _baseRotation;
    private Vector3 _targetScale;
    private Vector3 _targetRotation;

    // Unity methods
    private void Start()
    {
        // Cache base values
        _baseScale = transform.localScale;
        _baseRotation = transform.eulerAngles;

        // Generate target scale
        _targetScale = _baseScale * _scaleMultiplier;

        // Generate target rotation
        _targetRotation = new Vector3(
            _baseRotation.x,
            _baseRotation.y,
            _baseRotation.z + _rotationAmount
            );

        // Update drop prompt
        ManageDropPrompt();
    }
    private void Update()
    {
        // Cache timer
        float timerCache = _animationTimer;

        // Check whether we are selected
        switch (_selected)
        {
            // We are selected
            case true:
                // Increment timer
                _animationTimer = Mathf.Clamp(_animationTimer + Time.deltaTime, 0, _animationTime);
                break;

            // We are not selected
            case false:
                // Decrement timer
                _animationTimer = Mathf.Clamp(_animationTimer - Time.deltaTime, 0, _animationTime);
                break;
        }

        // If the timer has changed (prevent more maths when not animating)
        if (timerCache != _animationTimer)
        {
            // Do the LERP
            _selfTransform.localScale = Vector3.Lerp(_baseScale, _targetScale, _animationTimer / _animationTime);
            _selfTransform.localEulerAngles = Vector3.Lerp(_baseRotation, _targetRotation, _animationTimer / _animationTime);
        }
    }

    // Methods
    /// <summary>
    /// Given an itemID, set's this slot to represent that ID. No error handling.
    /// </summary>
    /// <param name="itemID">The ItemID to set to.</param>
    public void SetItem(Inventory.Item item)
    {
        // Attempt to update sprite and text
        (_itemText.text, _itemImage.sprite) = _inventory.GetInventoryItem(item);

        // Set image colour to white
        _itemImage.color = Color.white;

        // Set itemID
        _item = item;

        // Update drop prompt
        ManageDropPrompt();
    }
    /// <summary>
    /// Attempts to remove any held item from this slot.
    /// </summary>
    /// <returns>A valid inventory item on success, None on fail.</returns>
    public Inventory.Item TryRemoveItem()
    {
        // Cache item
        Inventory.Item temp = _item;

        // Set item to be invalid
        _item = Inventory.Item.None;

        // Reset item text
        _itemText.text = string.Empty;

        // Hide item sprite
        _itemImage.color = Color.clear;

        // Update drop prompt
        ManageDropPrompt();

        // Return old item (can be None if there never was an item).
        return temp;
    }
    /// <summary>
    /// Tells this slot that it is highlighted.
    /// </summary>
    public void Select()
    {
        // Set slot to be selected
        _selected = true;

        // Update drop prompt
        ManageDropPrompt();
        _nextPrompt.SetActive(true);
    }
    /// <summary>
    /// Tells this slot that it is not highlighted.
    /// </summary>
    public void DeSelect()
    {
        // Set slot to be selected
        _selected = false;

        // Update drop prompt
        ManageDropPrompt();
        _nextPrompt.SetActive(false);
    }
    /// <summary>
    /// Toggles the drop prompt text based on whether this slot contains an item and if it is selected.
    /// </summary>
    private void ManageDropPrompt()
    {
        // If we are not holding an item, or are not selected
        if (_item == Inventory.Item.None || !_selected)
        {
            // Disable drop prompt
            _dropPrompt.SetActive(false);

            // Early out
            return;
        }

        // Otherwise enable drop prompt
        _dropPrompt.SetActive(true);
    }

    // Accessors
    /// <summary>
    /// Fetches the item currently in this slot.
    /// </summary>
    public Inventory.Item Item
    {
        get { return _item; }
    }
    /// <summary>
    /// Fetches the text related to the item currently in this slot.
    /// </summary>
    public string ItemName
    {
        get { return _itemText.text; }
    }
    /// <summary>
    /// Fetches the sprite related to the item currently in this slot.
    /// </summary>
    public Sprite Sprite
    {
        get { return _itemImage.sprite; }
    }
}
