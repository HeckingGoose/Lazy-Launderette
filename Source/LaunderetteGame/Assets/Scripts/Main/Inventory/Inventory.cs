using UnityEngine;

public class Inventory : MonoBehaviour
{
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

    // Methods
    /// <summary>
    /// Given an itemID, attempts to fetch the name and sprite of the item.
    /// </summary>
    /// <param name="ID">The ID to check against.</param>
    /// <returns>The item's name and sprite on success, error text and sprite on fail.</returns>
    public (string name, Sprite sprite) GetInventoryItem(int ID)
    {
        // Check ID in range
        if (ID >= 0 &&
            ID < _names.Length &&
            ID < _sprites.Length
            )
        {
            // Then return
            return (_names[ID], _sprites[ID]);
        }

        // Otherwise
        else
        {
            // Return fail
            return (_errorText, _errorSprite);
        }
    }
}
