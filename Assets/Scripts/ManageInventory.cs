using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ManageInventory : MonoBehaviour
{
    // Editor variables
    [SerializeField]
    private RectTransform[] inventorySlots;
    [SerializeField]
    private Image[] inventoryImages;
    [SerializeField]
    private TextMeshProUGUI[] inventoryText;
    [SerializeField]
    private string[] itemNames;
    [SerializeField]
    private Sprite[] itemSprites;
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
    private int[] inventoryItems;
    private Vector3[] baseScales;
    private Vector3[] maxScales;
    private Vector3[] baseRotations;
    private Vector3[] maxRotations;
    public int selectedItem;
    private float[] timers;
    private int gState = 0;

    private void Start()
    {
        // Initialise inventory items
        inventoryItems = new int[inventorySlots.Length];
        baseScales = new Vector3[inventorySlots.Length];
        maxScales = new Vector3[inventorySlots.Length];
        baseRotations = new Vector3[inventorySlots.Length];
        maxRotations = new Vector3[inventorySlots.Length];
        timers = new float[inventorySlots.Length];

        for(int i = 0; i < inventoryItems.Length; i++)
        {
            inventoryItems[i] = -1;

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

        selectedItem = 0;
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
            if (selectedItem == i)
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
        if (Input.GetAxis("Inv1") > 0 && selectedItem != 0)
        {
            selectedItem = 0;
            UpdateHand();
        }
        else if (Input.GetAxis("Inv2") > 0 && selectedItem != 1)
        {
            selectedItem = 1;
            UpdateHand();
        }
        else if (Input.GetAxis("Inv3") > 0 && selectedItem != 2)
        {
            selectedItem = 2;
            UpdateHand();
        }

        // Drop current item if g is pressed
        if (gState == 1 && inventoryItems[selectedItem] != -1)
        {
            switch (inventoryItems[selectedItem])
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
            translate.DropItem(inventoryItems[selectedItem]);
            TryRemoveItem(selectedItem);
            UpdateHand();
        }
    }

    // Other
    public void UpdateHand()
    {
        // There is an item in the hand now
        if (inventoryItems[selectedItem] != -1)
        {
            heldItemImage.sprite = itemSprites[inventoryItems[selectedItem]];
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
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i] == -1)
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
        else if (itemID > itemNames.Length)
        {
            Debug.Log($"Item ID {itemID} does not exist.");
            return false;
        }
        else if (itemID > itemSprites.Length)
        {
            Debug.Log($"Item ID {itemID} has no sprite.");
            return false;
        }

        inventoryItems[pointer] = itemID;
        inventoryImages[pointer].sprite = itemSprites[itemID];
        inventoryImages[pointer].enabled = true;
        inventoryText[pointer].text = itemNames[itemID];
        UpdateHand();
        return true;
    }
    public bool TryRemoveItem(int index)
    {
        if (inventoryItems[index] == -1)
        {
            Debug.Log("Inventory slot is currently empty! Cannot remove item.");
            return false;
        }

        inventoryItems[index] = -1;
        inventoryImages[index].enabled = false;
        inventoryText[index].text = "";
        UpdateHand();
        return true;
    }
    public string GetHeldItemName()
    {
        if (inventoryItems[selectedItem] == -1)
        {
            return "Empty";
        }
        else
        {
            return itemNames[inventoryItems[selectedItem]];
        }
    }
}
