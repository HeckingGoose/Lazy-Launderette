using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    // Const
    private const int SPEED_MODIFIER = 500;

    // Editor variables
    [SerializeField]
    private Transform referenceCamera;

    // Private variables
    private Vector3 initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        // Cache initial rotation to do rotation locking
        initialRotation = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate and set forward vector
        Vector3 forwardVector = referenceCamera.position - transform.position;
        transform.forward = Vector3.Lerp(transform.forward, forwardVector, Time.deltaTime / SPEED_MODIFIER);

        // Lock rotation
        transform.eulerAngles = new Vector3(
            initialRotation.x,
            transform.eulerAngles.y,
            initialRotation.z
            );
    }
}
