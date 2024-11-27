using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveOnClick : MonoBehaviour
{
    // Editor variables
    [Header("Scene References")]
    [SerializeField]
    private TextMeshProUGUI _clickText;
    [Header("Timer Values")]
    [SerializeField]
    private float _fadeDoneAt = 3;
    [SerializeField]
    private float _beginFadeAt = 2;

    // Private variables
    private float _timer = 0;

    // Unity methods
    private void Update()
    {
        // Wait for left mouse axis, and text starting to fade in
        if (Input.GetAxis("LeftMouse") > 0 && _timer > _beginFadeAt)
        {
            // Set the load target to the main scene and begin loading
            GLOBAL.LoadTarget = "Main";
            SceneManager.LoadScene("Loading");
        }

        // Lerp in the fade text
        _clickText.color = new Color(
            _clickText.color.r,
            _clickText.color.g,
            _clickText.color.b,
            Mathf.Lerp(0, 1, Mathf.Clamp((_timer - _beginFadeAt) / (_fadeDoneAt - _beginFadeAt), 0, 1))
            );

        // Increment timer
        _timer += Mathf.Clamp(Time.deltaTime, 0, _fadeDoneAt);
    }
}
