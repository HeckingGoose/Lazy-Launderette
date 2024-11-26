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
    public void DropItem(int itemID)
    {
        // Ensure script is even ready
        if (ready)
        {
            // Ensure itemID is in range
            if (itemID >= itemPrefabs.Length ||
                itemID < 0
                )
            {
                // Not in length, so log error and skip
                Debug.LogWarning($"Failed to translate object to worldItem (Given ID: {itemID}, valid range is IDs: 0 - {itemPrefabs.Length - 1}).");
                return;
            }

            // Create a new item matching the ID provided
            Instantiate
            (
                itemPrefabs[itemID],
                new Vector3(owner.position.x, owner.position.y - ownerController.height + 0.01f, owner.position.z),
                itemPrefabs[itemID].transform.rotation
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
