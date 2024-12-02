using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveOnClick : MonoBehaviour
{
    // Editor variables
    [Header("Scene References")]
    [SerializeField]
    private TextMeshProUGUI _goText;
    [SerializeField]
    private Image _goImage;
    [Header("Timer Values")]
    [SerializeField]
    private float _fadeDoneAt = 3;
    [SerializeField]
    private float _beginFadeAt = 2;

    // Private variables
    private float _timer = 0;
    private InputActionMap _menuActionMap;
    private InputAction _goToMainAction;

    // Unity methods
    private void Start()
    {
        // Fetch a reference to the MainMenu action map
        _menuActionMap = InputSystem.actions.FindActionMap(InputDefinitions.ACTIONMAP_MENU);

        // Fetch a reference to the GoToMain action
        _goToMainAction = _menuActionMap.FindAction(InputDefinitions.MMAM_GOTOMAIN);
    }
    private void Update()
    {
        // Wait for left mouse axis, and text starting to fade in
        if (_goToMainAction.WasPressedThisFrame() && _timer > _beginFadeAt)
        {
            // Set the load target to the main scene and begin loading
            GLOBAL.LoadTarget = "Main";
            SceneManager.LoadScene("Loading");
        }

        // Lerp in the go text
        _goText.color = new Color(
            _goText.color.r,
            _goText.color.g,
            _goText.color.b,
            Mathf.Lerp(0, 1, Mathf.Clamp((_timer - _beginFadeAt) / (_fadeDoneAt - _beginFadeAt), 0, 1))
            );

        // Lerp in the go image
        _goImage.color = new Color(
            _goImage.color.r,
            _goImage.color.g,
            _goImage.color.b,
            Mathf.Lerp(0, 1, Mathf.Clamp((_timer - _beginFadeAt) / (_fadeDoneAt - _beginFadeAt), 0, 1))
            );

        // Increment timer
        _timer += Mathf.Clamp(Time.deltaTime, 0, _fadeDoneAt);
    }
}
