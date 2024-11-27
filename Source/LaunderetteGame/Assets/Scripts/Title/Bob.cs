using UnityEngine;

public class Bob : MonoBehaviour
{
    // Editor variables
    [Header("Bob Control Values")]
    [SerializeField]
    private float _amplitude = 20;
    [SerializeField]
    private float _speed = 7;

    // Internal variables
    private Vector3 _basePosition;
    private RectTransform _selfRectTransformCache;
    private float _timer = 0;
    void Start()
    {
        // Cache self rect transform
        _selfRectTransformCache = GetComponent<RectTransform>();

        // Cache starting position
        _basePosition = _selfRectTransformCache.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Update position relative to start, using a Sine wave and time
        _selfRectTransformCache.position = _basePosition + new Vector3(0, _amplitude * Mathf.Sin(_timer), 0);

        // Increment timer by speed relative to time
        _timer += Time.deltaTime * _speed;
    }
}
