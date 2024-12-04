using UnityEngine;

public class HandlePickup : MonoBehaviour
{
    // Editor Variables
    [SerializeField]
    private Inventory.Item _item;

    // Accessors
    /// <summary>
    /// Fetches the item type of this script.
    /// </summary>
    public Inventory.Item Item
    {
        get { return _item; }
    }
}
