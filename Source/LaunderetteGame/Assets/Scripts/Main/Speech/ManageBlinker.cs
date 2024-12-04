using UnityEngine;

public class ManageBlinker : MonoBehaviour
{
    // Editor variables
    [Header("References")]
    [SerializeField]
    private GameObject _target;
    [Header("Config")]
    [SerializeField]
    private float _timeBetweenToggles = 0.65f;

    // Private variables
    private float _timer;

    // Unity Methods
    void Update()
    {
        // Is it time to swap?
        if (_timer > _timeBetweenToggles)
        {
            // Swap
            _target.SetActive(!_target.activeSelf);

            // Reset timer
            _timer = 0;
        }

        // Increment timer
        _timer += Time.deltaTime;
    }
}
