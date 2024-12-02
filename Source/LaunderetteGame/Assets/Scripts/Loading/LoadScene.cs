using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    // Editor variables
    [Header("Scene Object References")]
    [SerializeField]
    private TextMeshProUGUI _doneText;
    [SerializeField]
    private Image _doneImage;
    [SerializeField]
    private Image _loadingImage;
    [SerializeField]
    private Image _loadingImageBackground;
    [Header("Potential Loading Screen Sprites")]
    [SerializeField]
    private Sprite[] _loadingSprites;
    [Header("Text Fade In Timer")]
    [SerializeField]
    private float _textFadeTime = 1;

    // Internal values
    private float _textFadeTimer = 0f;
    private bool _loaded = false;
    private AsyncOperation _sceneLoadingHandle;

    // Action maps
    private InputActionMap _mainMenuActionMap;

    // Actions
    private InputAction _goToNextScene;

    private void Start()
    {
        // Begin load
        _sceneLoadingHandle = SceneManager.LoadSceneAsync(GLOBAL.LoadTarget);
        _sceneLoadingHandle.allowSceneActivation = false;

        // Prepare the sprites
        Sprite loadingIcon = _loadingSprites[Random.Range(0, _loadingSprites.Length)];
        _loadingImage.sprite = loadingIcon;
        _loadingImageBackground.sprite = loadingIcon;

        // Fetch action map
        _mainMenuActionMap = InputSystem.actions.FindActionMap(InputDefinitions.ACTIONMAP_MENU);

        // Fetch actions
        _goToNextScene = _mainMenuActionMap.FindAction(InputDefinitions.MMAM_GOTOMAIN);
    }
    private void Update()
    {
        // If we are done loading
        if (_sceneLoadingHandle.progress >= 0.89f)
        {
            // Set sprites to show we are done
            _loadingImage.fillAmount = 1f;
            _loaded = true;
        }
        // Otherwise
        else
        {
            // Show loading progress
            _loadingImage.fillAmount = _sceneLoadingHandle.progress;
        }

        // If we are done loading
        if (_loaded)
        {
            // Begin incrementing text fade timer
            _textFadeTimer += Time.deltaTime;

            // Fade in text, according to time
            _doneText.color = new Color(
                _doneText.color.r,
                _doneText.color.g,
                _doneText.color.b,
                Mathf.Lerp(0, 1, _textFadeTimer / _textFadeTime)
                );

            // Fade in image, according to time
            _doneImage.color = new Color(
                _doneImage.color.r,
                _doneImage.color.g,
                _doneImage.color.b,
                Mathf.Lerp(0, 1, _textFadeTimer / _textFadeTime)
                );

            // If text has faded in fully
            if (_textFadeTimer > _textFadeTime)
            {
                // If next scene is to be triggered
                if (_goToNextScene.WasPressedThisFrame())
                {
                    // Switch to next scene
                    _sceneLoadingHandle.allowSceneActivation = true;
                }
            }
        }
    }
}
