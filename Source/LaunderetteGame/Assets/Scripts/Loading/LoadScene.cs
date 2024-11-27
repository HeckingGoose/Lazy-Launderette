using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    // Editor variables
    [Header("Scene Object References")]
    [SerializeField]
    private TextMeshProUGUI _doneText;
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

    private void Start()
    {
        // Begin load
        _sceneLoadingHandle = SceneManager.LoadSceneAsync(GLOBAL.LoadTarget);
        _sceneLoadingHandle.allowSceneActivation = false;

        // Prepare the sprites
        Sprite loadingIcon = _loadingSprites[Random.Range(0, _loadingSprites.Length)];
        _loadingImage.sprite = loadingIcon;
        _loadingImageBackground.sprite = loadingIcon;
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

            // If text has faded in fully
            if (_textFadeTimer > _textFadeTime)
            {
                // If mouse is held
                if (Input.GetAxis("LeftMouse") > 0)
                {
                    // Switch to next scene
                    _sceneLoadingHandle.allowSceneActivation = true;
                }
            }
        }
    }
}
