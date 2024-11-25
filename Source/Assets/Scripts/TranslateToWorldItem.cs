using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TranslateToWorldItem : MonoBehaviour
{
    // Editor variables
    [SerializeField]
    private Transform player;
    [SerializeField]
    private CharacterController playerController;
    [SerializeField]
    private GameObject[] itemPrefabs;

    // External method
    public void DropItem(int itemID)
    {
        Instantiate
        (
            itemPrefabs[itemID],
            new Vector3(player.position.x, player.position.y - playerController.height + 0.01f, player.position.z),
            itemPrefabs[itemID].transform.rotation
        );
    }
}
