using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleMenu : MonoBehaviour
{
    // Editor variables
    [SerializeField]
    private PlayerController controller;
    [SerializeField]
    private GameObject menuRoot;

    // Internal variables
    private bool paused = false;
    private int cState = 0;

    private void Update()
    {
        // Handle esc press
        if (Input.GetAxis("Cancel") > 0 && cState < 2)
        {
            cState++;
        }
        else if (Input.GetAxis("Cancel") <= 0)
        {
            cState = 0;
        }

        if (cState == 1)
        {
            TogglePaused();
        }
    }

    // Public methods
    public void TogglePaused()
    {
        paused = !paused;

        switch (paused)
        {
            case true:
                controller.enabled = false;
                menuRoot.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                break;
            case false:
                controller.enabled = true;
                menuRoot.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
        }
    }

    public void ReturnToMenu()
    {
        GLOBAL.LoadTarget = "Menu";
        SceneManager.LoadScene("Loading");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
