using UnityEngine;
using System;

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
    private float nextRollTime;
    private System.Random rand;

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

        // Setup the time till next roll
        nextRollTime = timeToStart + UnityEngine.Random.Range(minTimeBetweenRolls, maxTimeBetweenRolls);

        // Setup random
        rand = new System.Random();
    }

    private void Update()
    {
        // If we managed to find main
        if (main != null && source != null)
        {
            // If the machine is currently running
            if (main.Running)
            {
                // Increment timer
                timer += Time.deltaTime;

                // Added inertia so sound doesn't play while door is closing
                if (timer > timeToStart)
                {
                    // Play ambient sound if there is no sound playing
                    if (!source.isPlaying)
                    {
                        // Play ambient sound
                        source.clip = rollingAmbient;
                        source.loop = true;
                        source.Play();
                    }

                    // Play random roll sound when needed
                    if (timer > nextRollTime)
                    {
                        // Reset timer, ignoring the initial time to start on door close
                        timer = timeToStart;

                        // Play random roll sound effect
                        source.clip = rollingDrop[rand.Next(0, rollingDrop.Length)];
                        source.loop = false;
                        source.Play();
                    }
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
