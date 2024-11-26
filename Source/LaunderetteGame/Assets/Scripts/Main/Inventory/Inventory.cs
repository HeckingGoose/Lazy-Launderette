using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Definitions
    public enum Item
    {
        None,
        ClothesBag,
        Choccy,
        BagHoldingSamsClothes,
        EmptyBag,
        Screwdriver,
        Screws,
        Coin,
        WashedClothes,
        VentCover
    }

    // Editor variables
    [Header("Inventory Items")]
    [SerializeField]
    private string[] _names;
    [SerializeField]
    private Sprite[] _sprites;

    [Header("Error handling stuff")]
    [SerializeField]
    private string _errorText;
    [SerializeField]
    private Sprite _errorSprite;

    // Private variables
    private static Dictionary<Item, int> _itemToSpriteIndexMap = new Dictionary<Item, int>
    {
        { Item.ClothesBag, 0 },
        { Item.Choccy, 1 },
        { Item.BagHoldingSamsClothes, 2 },
        { Item.EmptyBag, 3 },
        { Item.Screwdriver, 4 },
        { Item.Screws, 5 }
    };
    private static Dictionary<Item, string> _itemToName = new Dictionary<Item, string>
    {
        { Item.ClothesBag, "Clothes Bag" },
        { Item.Choccy, "Choccy" },
        { Item.BagHoldingSamsClothes, "Sam's Clothes" },
        { Item.EmptyBag, "Empty Bag" },
        { Item.Screwdriver, "Screwdriver" },
        { Item.Screws, "Screws" }
    };

    // Methods
    /// <summary>
    /// Given an itemID, attempts to fetch the name and sprite of the item.
    /// </summary>
    /// <param name="ID">The ID to check against.</param>
    /// <returns>The item's name and sprite on success, error text and sprite on fail.</returns>
    public (string name, Sprite sprite) GetInventoryItem(Item item)
    {
        // If we do not have a corresponding sprite
        if (!_itemToSpriteIndexMap.ContainsKey(item))
        {
            return (_errorText, _errorSprite);
        }

        // Otherwise return
        return (GetItemName(item), _sprites[_itemToSpriteIndexMap[item]]);
    }
    /// <summary>
    /// Given an item, attempts to fetch its name.
    /// </summary>
    /// <param name="item">The item to get the name of.</param>
    /// <returns>A human readable string representing the item on success, a literal translation of the enum name on fail.</returns>
    public static string GetItemName(Item item)
    {
        // If we do not have a corresponding name
        if (!_itemToName.ContainsKey(item))
        {
            return item.ToString();
        }

        // Otherwise return
        return _itemToName[item];
    }
}
