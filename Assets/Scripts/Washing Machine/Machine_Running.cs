using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine_Running : MonoBehaviour
{
    // Editor variables
    [SerializeField]
    private float timeToStart;
    [SerializeField]
    private float minTimeBetweenRolls = 1f;
    [SerializeField]
    private float maxTimeBetweenRolls = 5f;
    [SerializeField]
    private AudioClip rollingAmbient;
    [SerializeField]
    private AudioClip[] rollingDrop;

    // Internal variables
    private Machine_Main main;
    private AudioSource source;
    private float timer;

    private void Start()
    {
        // Try to fetch the main machine script
        bool success = TryGetComponent(out main);

        // If we fail
        if (!success)
        {
            Debug.LogError($"Failed to fetch Machine_Main on {name}!");
        }

        // Try to fetch the audio source
        success = TryGetComponent(out source);

        // If we fail
        if (!success)
        {
            Debug.LogError($"Failed to fetch AudioSource on {name}!");
        }
    }

    private void Update()
    {
        // If we managed to find main
        if (main != null && source != null)
        {
            // If the machine is currently running
            if (main.Running)
            {
                // Added inertia so sound doesn't play while door is closing
                if (timer > timeToStart)
                {

                }
            }
            // If the machine is not running
            else
            {
                // Set timer to 0
                timer = 0;
            }
        }
    }
}
