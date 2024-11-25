using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveOnClickj : MonoBehaviour
{
    // Editor variables
    [SerializeField]
    private TextMeshProUGUI clickText;
    [SerializeField]
    private float doneAt = 6;
    [SerializeField]
    private float beginFadeAt = 5;

    // Internal variables
    private float timer = 0;
    private void Update()
    {
        if (Input.GetAxis("LeftMouse") > 0 && timer > beginFadeAt)
        {
            GLOBAL.LoadTarget = "Main";
            SceneManager.LoadScene("Loading");
        }

        clickText.color = new Color(
            clickText.color.r,
            clickText.color.g,
            clickText.color.b,
            Mathf.Lerp(0, 1, Mathf.Clamp((timer - beginFadeAt) / (doneAt - beginFadeAt), 0, 1))
            );

        timer += Time.deltaTime;
    }
}
