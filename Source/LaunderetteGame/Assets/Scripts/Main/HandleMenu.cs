using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HandleMenu : MonoBehaviour
{
    // Editor variables
    [SerializeField]
    private PlayerController controller;
    [SerializeField]
    private GameObject menuRoot;

    // Private variables
    private bool paused = false;

    // Action Maps
    private InputActionMap _freeRoamActionMap;

    // Actions
    private InputAction _pauseAction;

    // Unity Methods
    private void Start()
    {
        // Fetch action map
        _freeRoamActionMap = InputSystem.actions.FindActionMap(InputDefinitions.ACTIONMAP_FREEROAM);

        // Fetch actions
        _pauseAction = _freeRoamActionMap.FindAction(InputDefinitions.FRAM_MIDGAMEMENU);
    }
    private void Update()
    {
        // If pause is pressed this frame
        if (_pauseAction.WasPressedThisFrame())
        {
            // Switch between being paused and unpaused
            TogglePaused();
        }
    }

    // Public methods
    /// <summary>
    /// Toggles whether the game is paused or not.
    /// </summary>
    public void TogglePaused()
    {
        // Invert paused
        paused = !paused;

        // Whether we should now be paused or unpaused
        switch (paused)
        {
            // We need to pause
            case true:
                controller.enabled = false;
                menuRoot.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                break;

            // We need to unpause
            case false:
                controller.enabled = true;
                menuRoot.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
        }
    }
    /// <summary>
    /// Causes the game to jump back to the main menu.
    /// </summary>
    public void ReturnToMenu()
    {
        GLOBAL.LoadTarget = "Menu";
        SceneManager.LoadScene("Loading");
    }
    /// <summary>
    /// Closes the game.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
