using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    // Const
    private const int SPEED_MODIFIER = 5;

    // Editor variables
    [SerializeField]
    private Transform referenceCamera;

    // Update is called once per frame
    void Update()
    {
        // Cache facing angle
        Vector3 cache = transform.eulerAngles;

        // Get direction between this and camera
        Vector3 directionBetween = referenceCamera.transform.position - transform.position;

        // Set it
        transform.forward = directionBetween;

        // Lerp it
        cache.y = Mathf.Lerp(cache.y, transform.eulerAngles.y, Mathf.Min(Time.deltaTime * SPEED_MODIFIER, 1));

        // Set it
        transform.eulerAngles = cache;
    }
}
