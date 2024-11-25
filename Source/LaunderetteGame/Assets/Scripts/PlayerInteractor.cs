using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInteractor : MonoBehaviour
{
    // Editor variables
    [SerializeField]
    private PlayerController controller;
    [SerializeField]
    private GameObject conversationRoot;
    [SerializeField]
    private ManageConversation conversationManager;
    [SerializeField]
    private Camera view;
    [SerializeField]
    private float rayDistance = 2f;
    [SerializeField]
    private float timeToMaxSize;
    [SerializeField]
    private float maxScale;
    [SerializeField]
    private RectTransform crosshair;
    [SerializeField]
    private Texture2D cursor;
    [SerializeField]
    private Image crosshairImage;
    [SerializeField]
    private TextMeshProUGUI describeText;
    [SerializeField]
    private ManageCoins manageCoins;
    [SerializeField]
    private AudioClip coinPickup;
    [SerializeField]
    private AudioSource coinSource;
    [SerializeField]
    private ManageInventory inventory;
    [SerializeField]
    private AudioSource pickupSource;
    [SerializeField]
    private AudioClip[] pickupSounds;
    [SerializeField]
    private TranslateToWorldItem translate;
    [SerializeField]
    private EndScene endScene;

    // Private variables
    private int mask;
    private HandleInteractBubble h;
    private RaycastHit hit;
    private bool talking = false;
    private bool hovering = false;
    private float timer = 0;
    private Vector3 baseSize;
    private Vector3 maxSize;
    private Vector4 baseOpacity;
    private Vector4 maxOpacity;
    private int eState = 0;

    private void Start()
    {
        mask = LayerMask.GetMask(new string[] { "PickupRaycast", "SpeechRaycast" });
        baseSize = crosshair.localScale;
        maxSize = baseSize * maxScale;
        baseOpacity = crosshairImage.color;
        maxOpacity = crosshairImage.color;
        maxOpacity.w = 1;
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    // Raycast is done here
    private void FixedUpdate()
    {
        // Handle e press
        if (Input.GetAxis("Talk") > 0 && eState < 2)
        {
            eState++;
        }
        else if (Input.GetAxis("Talk") <= 0)
        {
            eState = 0;
        }

        // Do raycast
        Ray ray = view.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.transform != null)
            {
                switch (hit.transform.gameObject.tag)
                {
                    case "Talkable":
                        if (!talking)
                        {
                            describeText.text = "Talk";
                            hovering = true;
                            // Attempt to show speech bubble on hit object
                            hit.transform.gameObject.TryGetComponent<HandleInteractBubble>(out h);
                            try
                            {
                                h.ShowBubble();

                                // If player then attempts to speak to the hit object
                                if (eState == 1)
                                {
                                    describeText.text = "";
                                    h.StartTalk(this);
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            try
                            {
                                h.HideBubble();
                            }
                            catch { }
                        }
                        break;
                    case "Pickup":
                        describeText.text = "Pickup";
                        hit.transform.gameObject.TryGetComponent(out HandlePickup p);

                        if (p != null)
                        {
                            if (p.itemID == -2 && inventory.GetHeldItemName() != "Empty Bag")
                            {
                                describeText.text = "Need empty bag";
                            }
                            else if (p.itemID == -3)
                            {
                                if (inventory.GetHeldItemName() == "Screwdriver")
                                {
                                    describeText.text = "Remove";
                                }
                                else
                                {
                                    describeText.text = "Needs a screwdriver";
                                }
                            }

                            if (eState == 1)
                            {
                                bool success;
                                switch (p.itemID)
                                {
                                    // coin
                                    case -1:
                                        manageCoins.numCoins++;
                                        coinSource.clip = coinPickup;
                                        coinSource.Play();
                                        Destroy(p.gameObject);
                                        break;

                                    // clothes without bag
                                    case -2:
                                        if (inventory.GetHeldItemName() == "Empty Bag")
                                        {
                                            inventory.TryRemoveItem(inventory.selectedItem);
                                            success = inventory.TryAddItem(2);

                                            if (success)
                                            {
                                                Destroy(p.gameObject);
                                                pickupSource.clip = pickupSounds[1];
                                                pickupSource.Play();
                                            }
                                        }
                                        break;

                                    // vent
                                    case -3:
                                        if (inventory.GetHeldItemName() == "Screwdriver")
                                        {
                                            // Add screws
                                            success = inventory.TryAddItem(5);

                                            // If inventory is full then drop screws
                                            if (!success)
                                            {
                                                translate.DropItem(5);
                                            }
                                            else
                                            {
                                                pickupSource.clip = pickupSounds[2];
                                                pickupSource.Play();
                                            }

                                            // Remove vent
                                            Destroy(p.gameObject);
                                            pickupSource.clip = pickupSounds[3];
                                            pickupSource.Play();

                                        }
                                        break;

                                    // regular item
                                    default:
                                        success = inventory.TryAddItem(p.itemID);

                                        if (success)
                                        {
                                            Destroy(p.gameObject);
                                        }

                                        switch (p.itemID)
                                        {
                                            case 1: // Chocolate
                                                pickupSource.clip = pickupSounds[0];
                                                pickupSource.Play();
                                                break;
                                            case 5: // screws
                                            case 4: // Screwdriver
                                                pickupSource.clip = pickupSounds[2];
                                                pickupSource.Play();
                                                break;
                                            default: // Bag
                                                pickupSource.clip = pickupSounds[1];
                                                pickupSource.Play();
                                                break;
                                        }
                                        break;
                                }
                            }
                        }
                        hovering = true;
                        break;
                    case "Machine":
                        // Communicate to player
                        describeText.text = "Toggle Door";
                        hovering = true;

                        // Find Machine script
                        hit.transform.parent.parent.parent.TryGetComponent(out Machine_Main m);
                        if (m != null)
                        {
                            // Wait for input
                            if (eState == 1)
                            {
                                m.ToggleDoor();
                            }
                        }
                        else
                        {
                            Debug.Log("Ha");
                        }
                        break;
                    case "WashSpot":
                        if (manageCoins.numCoins >= 5)
                        {
                            if (inventory.GetHeldItemName() == "Clothes Bag")
                            {
                                describeText.text = "Wash clothes?";

                                if (eState == 1)
                                {
                                    manageCoins.numCoins -= 5;
                                    try
                                    {
                                        endScene.Begin();
                                    }
                                    catch { GLOBAL.LoadTarget = "Menu"; SceneManager.LoadScene("Loading"); }
                                }
                            }
                            else
                            {
                                describeText.text = "Select clothes before using machine.";
                            }
                        }
                        else
                        {
                            describeText.text = $"£{manageCoins.numCoins}/£5, cannot afford yet";
                        }
                        break;
                    default:
                        describeText.text = "";
                        hovering = false;
                        if (h != null)
                        {
                            h.HideBubble();
                            h = null;
                        }
                        break;
                }
            }
        }
        else
        {
            describeText.text = "";
            hovering = false;
            if (h != null)
            {
                h.HideBubble();
                h = null;
            }
        }

        if (hovering)
        {
            timer += Time.deltaTime;

            if (timer > timeToMaxSize)
            {
                timer = timeToMaxSize;
            }
        }
        else
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                timer = 0;
            }
        }

        crosshair.localScale = Vector3.Lerp(baseSize, maxSize, timer / timeToMaxSize);
        crosshairImage.color = Vector4.Lerp(baseOpacity, maxOpacity, timer / timeToMaxSize);
    }


    // External functions
    public void StartTalking(in CharacterData characterData, in Transform target)
    {
        if (target != null && characterData.conversations != null)
        {
            // Set required variables
            talking = true;
            Debug.Log($"Successfully started conversation with {characterData.name}.");

            // Unhide conversation thingy
            conversationRoot.SetActive(true);

            // Disable movement and looking
            controller.enabled = false;

            // Unhide and unlock mouse
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            // Move into conversation
            conversationManager.StartTalk(in characterData, in target, inventory.GetHeldItemName());
        }
    }
    public void EndTalk()
    {
        // Hide mouse again
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Enable movement and looking
        controller.enabled = true;

        // Hide conversation box
        conversationRoot.SetActive(false);

        talking = false;
        Debug.Log("Successfully closed conversation.");
    }
}
