using System.Collections.Generic;
using UnityEngine;

public class TranslateToWorldItem : MonoBehaviour
{
    // Editor variables
    [SerializeField]
    private Transform owner;
    [SerializeField]
    private CharacterController ownerController;
    [SerializeField]
    private GameObject[] itemPrefabs;

    // Private stuff
    private bool ready = true;
    private Dictionary<Inventory.Item, int> _itemToObjectIndexMap = new Dictionary<Inventory.Item, int>
    {
        { Inventory.Item.ClothesBag, 0 },
        { Inventory.Item.Choccy, 1 },
        { Inventory.Item.BagHoldingSamsClothes, 2 },
        { Inventory.Item.EmptyBag, 3 },
        { Inventory.Item.Screwdriver, 4 },
        { Inventory.Item.Screws, 5 }
    };

    // Methods
    public void Start()
    {
        // Check ready
        if (owner == null ||
            ownerController == null ||
            itemPrefabs == null
            )
        {
            // Tell the script we're not ready
            ready = false;
        }
    }
    /// <summary>
    /// Given an ItemID, attempts to generate a worldItem as the owner's root.
    /// </summary>
    /// <param name="itemID">The ItemID to generate.</param>
    public void DropItem(Inventory.Item item)
    {
        // Ensure script is even ready
        if (ready)
        {
            // Ensure itemID is in range
            if (!_itemToObjectIndexMap.ContainsKey(item))
            {
                // Not in length, so log error and skip
                Debug.LogWarning($"Failed to translate object to worldItem (Given item '{item.ToString()}' is not a valid worldItem).");
                return;
            }

            // Create a new item matching the ID provided
            Instantiate
            (
                itemPrefabs[_itemToObjectIndexMap[item]],
                new Vector3(owner.position.x, owner.position.y - ownerController.height + 0.01f, owner.position.z),
                itemPrefabs[_itemToObjectIndexMap[item]].transform.rotation
            );
        }
        // Otherwise
        else
        {
            // Log error
            Debug.LogWarning("TranslateToWorldItem does not have all references set-up!");
        }
    }
}
