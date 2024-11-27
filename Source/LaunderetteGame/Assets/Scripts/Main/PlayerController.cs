using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Editor variables
    [SerializeField]
    private float velocity = 6;
    [SerializeField]
    private float lookRange = 120;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private float crouchHeight;
    [SerializeField]
    private float timeToCrouch;

    // Private variables
    private CharacterController characterController;
    private float crouchTimer;
    private float baseHeight;
    public bool inVentZone = false;

    // Start is called before the first frame update
    void Start()
    {
        // Make mouse invisible and lock to screen centre
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        // Fetch character controller
        characterController = GetComponent<CharacterController>();
        baseHeight = characterController.height;
    }

    // Update is called once per frame
    void Update()
    {

        // Create objects for movement and rotation
        Vector3 movement = new Vector3();

        Vector3 rotation = new Vector3(
            Input.GetAxis("Mouse Y") * -1,
            Input.GetAxis("Mouse X"),
            0
            );

        // Calculate movement from Z direction
        movement.x += velocity * Time.deltaTime * Input.GetAxis("Vertical") * Mathf.Sin(transform.localEulerAngles.y * Mathf.Deg2Rad);
        movement.z += velocity * Time.deltaTime * Input.GetAxis("Vertical") * Mathf.Sin((90 - transform.localEulerAngles.y) * Mathf.Deg2Rad);

        // Calculate movement from X direction
        movement.x += velocity * Time.deltaTime * Input.GetAxis("Horizontal") * Mathf.Sin((90 - transform.localEulerAngles.y) * Mathf.Deg2Rad);
        movement.z += -velocity * Time.deltaTime * Input.GetAxis("Horizontal") * Mathf.Sin(transform.localEulerAngles.y * Mathf.Deg2Rad);

        if (crouchTimer > 0)
        {
            movement /= 2;
        }
        
        // Apply movement and rotation
        characterController.Move(movement);
        transform.eulerAngles += new Vector3(0, rotation.y, 0);

        // Handle looking up and down
        float newAngle = cameraTransform.eulerAngles.x + rotation.x;

        // Patch
        if (newAngle < 0)
        {
            newAngle += 360;
        }
        
        // If unity refuses the existence of negative numbers
        if (newAngle > 180)
        {
            newAngle = Mathf.Clamp(newAngle, 360 - lookRange / 2, 360);
        }
        else
        {
            newAngle = Mathf.Clamp(newAngle, 0, lookRange / 2);
        }

        // Apply camera transform
        cameraTransform.eulerAngles = new Vector3(
            newAngle,
            cameraTransform.eulerAngles.y,
            rotation.z
            );

        // Handle crouching
        if (!inVentZone)
        {
            if (Input.GetAxis("Crouch") > 0)
            {
                crouchTimer += Time.deltaTime;
                if (crouchTimer > timeToCrouch)
                {
                    crouchTimer = timeToCrouch;
                }
            }
            else
            {
                crouchTimer -= Time.deltaTime;
                if (crouchTimer < 0)
                {
                    crouchTimer = 0;
                }
            }

            float currentHeight = Mathf.Lerp(baseHeight, crouchHeight, crouchTimer / timeToCrouch);
            float currentOffset = currentHeight / -2f;

            characterController.height = currentHeight;
            characterController.center = new Vector3(
                characterController.center.x,
                currentOffset,
                characterController.center.z
                );

            transform.position = new Vector3(
                transform.position.x,
                currentHeight,
                transform.position.z
                );
        }
    }
}
