using UnityEngine;

public class Machine_Running : MonoBehaviour
{
    // Editor variables
    [Header("Timer Settings")]
    [SerializeField]
    private float _timeToStart = 0.5f;
    [SerializeField]
    private float _minTimeBetweenRolls = 10f;
    [SerializeField]
    private float _maxTimeBetweenRolls = 20f;
    [Header("Volume Settings")]
    [SerializeField]
    private float _volume = 0.7f;
    [Header("Sound Source Files")]
    [SerializeField]
    private AudioClip _rollingAmbient;
    [SerializeField]
    private AudioClip[] _rollingDrop;

    // Internal variables
    private Machine_Main _machineMainScript;
    private AudioSource _machineAudioSource;
    private float _rollTimer;
    private float _nextRollTime;
    private System.Random _random;

    // Unity Methods
    private void Start()
    {
        // Try to fetch the main machine script
        bool success = TryGetComponent(out _machineMainScript);

        // If we fail
        if (!success)
        {
            // Log the error
            Debug.LogError($"Failed to fetch Machine_Main on {name}!");
        }

        // Try to fetch the audio source
        success = TryGetComponent(out _machineAudioSource);

        // If we fail
        if (!success)
        {
            // Log the error
            Debug.LogError($"Failed to fetch AudioSource on {name}!");
        }

        // Setup the time till next roll
        _nextRollTime = _timeToStart + UnityEngine.Random.Range(_minTimeBetweenRolls, _maxTimeBetweenRolls);

        // Setup random
        _random = new System.Random();
    }

    private void Update()
    {
        // If we managed to find main
        if (_machineMainScript != null && _machineAudioSource != null)
        {
            // If the machine is currently running
            if (_machineMainScript.Running)
            {
                // Increment rollTimer
                _rollTimer += Time.deltaTime;

                // Added inertia so sound doesn't play while door is closing
                if (_rollTimer > _timeToStart)
                {
                    // Roll in volume
                    if (_rollTimer <= _timeToStart + 1)
                    {
                        // Do volume maths
                        _machineAudioSource.volume = (_rollTimer - _timeToStart) * _volume;
                    }

                    // If the audio source is not at max volume and we are not fading in
                    else if (_machineAudioSource.volume < _volume)
                    {
                        // Set voluem directly
                        _machineAudioSource.volume = _volume;
                    }

                    // Play ambient sound if there is no sound playing
                    if (!_machineAudioSource.isPlaying)
                    {
                        // Play ambient sound
                        _machineAudioSource.clip = _rollingAmbient;
                        _machineAudioSource.loop = true;
                        _machineAudioSource.Play();
                    }

                    // Play random roll sound when needed
                    if (_rollTimer > _nextRollTime)
                    {
                        // Reset rollTimer, ignoring the initial time to start on door close
                        _rollTimer = _timeToStart;

                        // Play random roll sound effect
                        _machineAudioSource.clip = _rollingDrop[_random.Next(0, _rollingDrop.Length)];
                        _machineAudioSource.loop = false;
                        _machineAudioSource.Play();
                    }
                }
            }
            // If the machine is not running
            else
            {
                // Set rollTimer to 0
                _rollTimer = 0;

                // Reset volume
                _machineAudioSource.volume = 0;
            }
        }
    }
}
