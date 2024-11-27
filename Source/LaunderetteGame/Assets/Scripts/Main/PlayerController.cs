using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Editor variables
    [Header("Reference Camera Transform")]
    [SerializeField]
    private Transform _cameraTransform;
    [Header("Move & Look Config")]
    [SerializeField]
    private float _velocity = 6;
    [SerializeField]
    private float _lookRange = 120;
    [SerializeField]
    private float _lookSensitivity = 0.25f;
    [SerializeField]
    private float _lookDeadZone = 0.1f;
    [Header("Crouching Settings")]
    [SerializeField]
    private float _crouchHeight = 0.9f;
    [SerializeField]
    private float _timeToCrouch = 0.3f;

    // Private variables
    private CharacterController _characterController;
    private float _crouchTimer;
    [SerializeField]
    private bool _crouching;
    private float _baseHeight;
    private bool _inVentZone = false;

    // Action map
    private InputActionMap _freeRoamActionMap;

    // Free Roam Actions
    private InputAction _walkAction;
    private InputAction _lookAction;
    private InputAction _crouchAction;

    // Unity Methods
    void Start()
    {
        // Make mouse invisible and lock to screen centre
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        // Fetch character controller
        _characterController = GetComponent<CharacterController>();
        _baseHeight = _characterController.height;

        // Fetch action map
        _freeRoamActionMap = InputSystem.actions.FindActionMap("FreeRoam");

        // Fetch relevant actions
        _walkAction = _freeRoamActionMap.FindAction("Walk");
        _lookAction = _freeRoamActionMap.FindAction("Look");
        _crouchAction = _freeRoamActionMap.FindAction("Crouch");
    }
    void Update()
    {

        // Create objects for movement and rotation
        Vector3 movement = new Vector3();

        // Read look action
        Vector2 look = _lookAction.ReadValue<Vector2>();

        // Deadzone it
        if (look.magnitude < _lookDeadZone)
        {
            // Zero it
            look = Vector2.zero;
        }

        // Apply sensitivity settings
        look *= _lookSensitivity;

        // Read walk action
        Vector2 walk = _walkAction.ReadValue<Vector2>();

        // Calculate movement from Z direction
        movement.x += _velocity * Time.deltaTime * walk.y * Mathf.Sin(transform.localEulerAngles.y * Mathf.Deg2Rad);
        movement.z += _velocity * Time.deltaTime * walk.y * Mathf.Sin((90 - transform.localEulerAngles.y) * Mathf.Deg2Rad);

        // Calculate movement from X direction
        movement.x += _velocity * Time.deltaTime * walk.x * Mathf.Sin((90 - transform.localEulerAngles.y) * Mathf.Deg2Rad);
        movement.z += -_velocity * Time.deltaTime * walk.x * Mathf.Sin(transform.localEulerAngles.y * Mathf.Deg2Rad);

        // Read crouch action
        bool crouched = _crouchAction.IsPressed();

        // If we are crouching (or in a vent zone)
        if (crouched || InVentZone)
        {
            // Half movement speed
            movement /= 2;
        }

        // Apply movement and rotation
        _characterController.Move(movement);
        transform.eulerAngles += new Vector3(0, look.x, 0);

        // Handle looking up and down
        float newAngle = _cameraTransform.eulerAngles.x + look.y;

        // Patch
        if (newAngle < 0)
        {
            newAngle += 360;
        }

        // If unity refuses the existence of negative numbers
        if (newAngle > 180)
        {
            newAngle = Mathf.Clamp(newAngle, 360 - _lookRange / 2, 360);
        }
        else
        {
            newAngle = Mathf.Clamp(newAngle, 0, _lookRange / 2);
        }

        // Apply camera transform
        _cameraTransform.eulerAngles = new Vector3(
            newAngle,
            _cameraTransform.eulerAngles.y,
            0
            );

        // As long as we are not in a vent zone
        if (!_inVentZone)
        {
            // If we are crouching
            if (crouched)
            {
                // Increment crouch timer
                _crouchTimer += Time.deltaTime;

                // Should we have finished crouching?
                if (_crouchTimer > _timeToCrouch)
                {
                    // Cap timer
                    _crouchTimer = _timeToCrouch;
                }
            }
            // If we are not crouching
            else
            {
                // Decrement timer
                _crouchTimer -= Time.deltaTime;

                // If the timer is below 0
                if (_crouchTimer < 0)
                {
                    // Cap it
                    _crouchTimer = 0;
                }
            }

            // Do maths to figure height stuff out
            float currentHeight = Mathf.Lerp(_baseHeight, _crouchHeight, _crouchTimer / _timeToCrouch);
            float currentOffset = currentHeight / -2f;

            // Do some crazy stuff that I don't want to change or read over
            _characterController.height = currentHeight;
            _characterController.center = new Vector3(
                _characterController.center.x,
                currentOffset,
                _characterController.center.z
                );

            transform.position = new Vector3(
                transform.position.x,
                currentHeight,
                transform.position.z
                );
        }
    }

    // Accessors
    /// <summary>
    /// Gets and sets whether the player is in a vent zone
    /// </summary>
    public bool InVentZone
    {
        get { return _inVentZone; }
        set { _inVentZone = value; }
    }
}
