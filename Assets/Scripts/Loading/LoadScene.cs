using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    // Editor variables
    [SerializeField]
    private TextMeshProUGUI doneText;
    [SerializeField]
    private Image loadingImage;
    [SerializeField]
    private Image loadingImageBackground;
    [SerializeField]
    private Sprite[] loadingSprites;
    [SerializeField]
    private float timeUntilReady;

    // Internal values
    private float timer = 0f;
    private bool loaded = false;
    private AsyncOperation loadingOperation;

    private void Start()
    {
        // Begin load
        loadingOperation = SceneManager.LoadSceneAsync(GLOBAL.LoadTarget);
        loadingOperation.allowSceneActivation = false;

        // Prepare the sprites
        Sprite loadingIcon = loadingSprites[Random.Range(0, loadingSprites.Length)];
        loadingImage.sprite = loadingIcon;
        loadingImageBackground.sprite = loadingIcon;
    }
    private void Update()
    {
        if (loadingOperation.progress >= 0.89f)
        {
            loadingImage.fillAmount = 1f;
            loaded = true;
        }
        else
        {
            loadingImage.fillAmount = loadingOperation.progress;
        }

        if (loaded)
        {
            timer += Time.deltaTime;

            doneText.color = new Color(
                doneText.color.r,
                doneText.color.g,
                doneText.color.b,
                Mathf.Lerp(0, 1, timer / timeUntilReady)
                );

            if (timer > timeUntilReady)
            {
                if (Input.GetAxis("LeftMouse") > 0)
                {
                    loadingOperation.allowSceneActivation = true;
                }
            }
        }
    }
}
