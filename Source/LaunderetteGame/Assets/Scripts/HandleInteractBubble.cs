using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleInteractBubble : MonoBehaviour
{
    // Editor variables
    [SerializeField]
    private GameObject bubble;
    [SerializeField]
    private CharacterData characterData;
    [SerializeField]
    private Transform eyeLevel;
    [SerializeField]
    private float rotationRange = 5f;
    [SerializeField]
    private float timeBetweenSwitches = 0.5f;

    // Private variables
    private float timer;
    private float baseRotation;

    // Style functions
    private void Start()
    {
        baseRotation = transform.localRotation.z;
    }
    private void Update()
    {
        if (bubble.activeInHierarchy)
        {
            // Reset timer if needed
            if (timer > timeBetweenSwitches)
            {
                timer = 0;

                // Generate new rotation
                float offset = Random.value * ((rotationRange / 2) - rotationRange);

                // Apply new rotation
                bubble.transform.eulerAngles = new Vector3(
                    bubble.transform.eulerAngles.x,
                    bubble.transform.eulerAngles.y,
                    baseRotation + offset
                    );
            }

            // Increment timer
            timer += Time.deltaTime;
        }
    }

    // External control functions
    public void ShowBubble()
    {
        bubble.SetActive(true);
    }
    public void HideBubble()
    {
        bubble.SetActive(false);
    }
    public void StartTalk(in PlayerInteractor caller)
    {
        Debug.Log($"Conversation requested by {caller.gameObject.name}");

        // Tell caller that we're done here
        caller.StartTalking(in characterData, in eyeLevel);
    }
}
