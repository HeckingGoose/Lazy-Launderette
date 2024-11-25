using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
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
        transform.forward = forwardVector;

        // Lock rotation
        transform.eulerAngles = new Vector3(
            initialRotation.x,
            transform.eulerAngles.y,
            initialRotation.z
            );
    }
}
