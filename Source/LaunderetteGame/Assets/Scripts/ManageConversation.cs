using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageConversation : MonoBehaviour
{
    // Editor variables
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI speechText;
    [SerializeField]
    private Image nameBackground;
    [SerializeField]
    private PlayerInteractor interactor;
    [SerializeField]
    private GameObject crosshair;
    [SerializeField]
    private float timeBetweenLetters;
    [SerializeField]
    private AudioSource speechSource;
    [SerializeField]
    private AudioClip[] speechSounds;
    [SerializeField]
    private AudioClip coinSound;
    [SerializeField]
    private ManageCoins manageCoins;
    [SerializeField]
    private ManageInventory manageInventory;
    [SerializeField]
    private TranslateToWorldItem translateToWorldItem;
    [SerializeField]
    private AudioSource altSource;
    [SerializeField]
    private AudioClip[] altSounds;

    // Private variables
    private bool talking = false;
    private int lineNumber = -1;
    private CharacterData characterData;
    private bool released = true;
    private bool doneSaying = false;
    private bool lineRead = false;
    private float timer = 0;
    private string lettersToAdd = string.Empty;
    private int wait = 0;
    private Transform character;
    private Material characterMat;

    private void Update()
    {
        // If we are currently talking
        if (talking)
        {

            // Check if we actually have data to work with
            if (characterData != null && lineNumber < characterData.conversations.Length)
            {
                // Check if we are interpreting raw text or a command
                if (characterData.conversations[lineNumber][0] == '!')
                {
                    // Command
                    string command = characterData.conversations[lineNumber].Split(' ')[0];
                    string data = characterData.conversations[lineNumber].Remove(0, characterData.conversations[lineNumber].IndexOf(' ') + 1);

                    // Figure out what needs to be done
                    switch (command)
                    {
                        case "!MOVE":
                            characterData.conversationName = data;
                            lineNumber++;
                            break;
                        case "!NEED":
                            characterData.needs.Add(data, false);
                            lineNumber++;
                            break;
                        case "!STRIPHELD":
                            manageInventory.TryRemoveItem(manageInventory.currentSlot);
                            lineNumber++;
                            break;
                        case "!SATISFY":
                            try
                            {
                                characterData.needs[data] = true;
                                lineNumber++;
                            }
                            catch
                            {
                                Debug.Log($"Need '{data}' does not exist.");
                                EndTalk();
                            }
                            break;
                        case "!GIVE":
                            bool success;
                            switch (data)
                            {
                                case "Coin":
                                    manageCoins.numCoins++;
                                    altSource.clip = coinSound;
                                    altSource.Play();
                                    while (speechSource.isPlaying) { }
                                    lineNumber++;
                                    break;
                                case "Empty Bag":
                                    success = manageInventory.TryAddItem(Inventory.Item.EmptyBag);
                                    if (!success)
                                    {
                                        translateToWorldItem.DropItem(Inventory.Item.EmptyBag);
                                        altSource.clip = altSounds[0];
                                        altSource.Play();
                                    }
                                    else
                                    {
                                        altSource.clip = altSounds[2];
                                        altSource.Play();
                                    }
                                    lineNumber++;
                                    break;
                                case "Choccy":
                                    success = manageInventory.TryAddItem(Inventory.Item.Choccy);
                                    if (!success)
                                    {
                                        translateToWorldItem.DropItem(Inventory.Item.Choccy);
                                        altSource.clip = altSounds[1];
                                        altSource.Play();
                                    }
                                    else
                                    {
                                        altSource.clip = altSounds[3];
                                        altSource.Play();
                                    }
                                    lineNumber++;
                                    break;
                            }
                            break;
                        case "!NAME":
                            nameText.text = data;
                            lineNumber++;
                            break;
                        case "!STANAME":
                            characterData.characterName = data;
                            nameText.text = data;
                            lineNumber++;
                            break;
                        case "!POSE":
                            try
                            {
                                characterMat.SetTexture("_MainTex", characterData.poses[int.Parse(data)]);
                            }
                            catch { Debug.Log($"Failed to set pose to {data}"); }
                            lineNumber++;
                            break;
                        case "!END":
                            EndTalk();
                            break;
                    }
                }
                else
                {
                    // If we are yet to set up speaking
                    if (!lineRead && lettersToAdd == string.Empty)
                    {
                        speechText.text = "";
                        lettersToAdd = characterData.conversations[lineNumber];
                        doneSaying = false;
                        lineRead = true;
                    }

                    timer += Time.deltaTime;

                    if (timer > timeBetweenLetters)
                    {
                        timer = 0;
                        if (lettersToAdd.Length > 0)
                        {
                            speechText.text += lettersToAdd[0];
                            lettersToAdd = lettersToAdd.Remove(0, 1);

                            wait++;

                            if (wait > characterData.skip)
                            {
                                // Play sound
                                speechSource.clip = speechSounds[Random.Range(0, speechSounds.Length)];
                                speechSource.Play();
                                wait = 0;
                            }
                        }
                        else
                        {
                            doneSaying = true;
                        }
                    }

                    if (Input.GetAxis("LeftMouse") > 0 && released)
                    {
                        if (doneSaying)
                        {
                            lineNumber++;
                            lineRead = false;
                            timer = 0;
                        }
                        else
                        {
                            doneSaying = true;

                            // dump rest of text
                            speechText.text += lettersToAdd;
                            lettersToAdd = string.Empty;
                        }
                        released = false;
                    }
                    else if (Input.GetAxis("LeftMouse") <= 0)
                    {
                        released = true;
                    }
                }
            }
            else
            {
                EndTalk();
            }
        }
    }

    // External functions
    public void StartTalk(in CharacterData _characterData, in Transform target, string heldItemName)
    {
        Debug.Log($"Received conversation {_characterData.conversationName}.");
        // Set up boxes
        nameText.text = _characterData.characterName;
        nameBackground.color = _characterData.characterColour;
        speechText.text = "";
        crosshair.SetActive(false);
        speechSource.pitch = _characterData.pitch;
        character = target;
        try
        {
            characterMat = character.parent.gameObject.GetComponent<Renderer>().materials[1];
        }
        catch { Debug.Log("Failed to fetch character material."); }

        // Find conversation
        lineNumber = -1;
        for (int i = 0; i < _characterData.conversations.Length; i++)
        {
            // Is the current line number our conversation name?
            if (_characterData.conversations[i] == _characterData.conversationName)
            {
                // Start talking
                lineNumber = i + 1;
                talking = true;
                characterData = _characterData;
                Debug.Log("Entering conversation...");
            }

            try
            {
                if (_characterData.conversations[i] == heldItemName && _characterData.needs[heldItemName] == false)
                {
                    lineNumber = i + 1;
                    talking = true;
                    characterData = _characterData;
                    return;
                }
            }
            catch { }
        }
        Debug.Log($"Conversation {_characterData.conversationName} not found!");
    }
    private void EndTalk()
    {
        talking = false;
        crosshair.SetActive(true);
        interactor.EndTalk();
    }
}
