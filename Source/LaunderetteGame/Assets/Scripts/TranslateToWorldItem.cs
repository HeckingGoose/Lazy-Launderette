using System.Collections.Generic;
using UnityEngine;

public class TranslateToWorldItem : MonoBehaviour
{
    // Editor variables
    [Header("Dropper Location References")]
    [SerializeField]
    private Transform owner;
    [SerializeField]
    private CharacterController ownerController;
    [Header("Item Prefab References")]
    [SerializeField]
    private GameObject[] itemPrefabs;
    [Header("Sound Effect References")]
    [SerializeField]
    private AudioSource _itemDropAudioPlayer;
    [SerializeField]
    private AudioClip[] _itemDropSoundClips;

    // Private stuff
    private bool _ready = true;
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
            _ready = false;
        }
    }
    /// <summary>
    /// Given an ItemID, attempts to generate a worldItem as the owner's root.
    /// </summary>
    /// <param name="item">The item to generate.</param>
    /// <param name="playSound">Whether we should play back a sound effect.</param>
    public void DropItem(Inventory.Item item, bool playSound = true) // Add this playing back the sounds for dropping
    {
        // Ensure script is even ready
        if (_ready)
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

            // If we should play a sound
            if (playSound)
            {
                // What is the item we're dropping?
                switch (item)
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
            }

        }
        // Otherwise
        else
        {
            // Log error
            Debug.LogWarning("TranslateToWorldItem does not have all references set-up!");
        }
    }
}
