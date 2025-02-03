using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    // Const
    private const int ANIM_START_TIME = 0;
    private const float CURSOR_MAX_ALPHA = 1;

    // Editor Variables
    [Header("Player References")]
    [SerializeField]
    private PlayerInteractor _player;
    [Header("Crosshair Config")]
    [SerializeField]
    private float _animationTime = 0.15f;
    [SerializeField]
    private float _maxScale = 1.5f;
    [Header("Crosshair Objects")]
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private Image _image;

    // Private Variables
    private Vector3 _sizeBase;
    private Vector3 _sizeMax;
    private Vector4 _colourBase;
    private Vector4 _colourMax;
    private float _timer;

    // Unity Methods
    private void Start()
    {
        // Fetch crosshair base state
        _sizeBase = _rectTransform.localScale;
        _colourBase = _image.color;

        // Calculate values for max states
        _sizeMax = _sizeBase * _maxScale;
        _colourMax = _image.color;
        _colourMax.w = CURSOR_MAX_ALPHA;
    }
    private void Update()
    {
        // We are looking at something
        if (_player.IsLookingAtSomething)
        {
            // Increment timer
            _timer = Mathf.Clamp(_timer + Time.deltaTime, ANIM_START_TIME, _animationTime);
        }

        // We are not looking at something
        else
        {
            // Decrement timer
            _timer = Mathf.Clamp(_timer - Time.deltaTime, ANIM_START_TIME, _animationTime);
        }

        // Update scale and colour
        _rectTransform.localScale = Vector3.Lerp(
                    _sizeBase, // Start
                    _sizeMax, // End
                    _timer / _animationTime // Progress
                    );
        _image.color = Vector4.Lerp(
            _colourBase, // Start
            _colourMax, // End
            _timer / _animationTime // Progress
            );
    }
}
