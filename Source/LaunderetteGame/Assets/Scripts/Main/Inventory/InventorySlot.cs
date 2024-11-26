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

    // Private values
    private Inventory.Item _item = Inventory.Item.None;

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
    }

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

        // Return old item (can be -1 if there never was an item).
        return temp;
    }

    // Accessors
    public Inventory.Item Item
    {
        get { return _item; }
    }
    public string ItemName
    {
        get { return _itemText.text; }
    }
    public Sprite Sprite
    {
        get { return _itemImage.sprite; }
    }
}
