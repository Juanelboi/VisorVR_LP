using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.Timeline.DirectorControlPlayable;

public class Codigo_Pausa : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject ExitMenu;
    public GameObject PauseMenu;
    public GameObject _TeleportMenu;

    [SerializeField] private InputActionReference pauseAction; // asignar en el inspector

    void OnEnable() => pauseAction.action.performed += OnPause;
    void OnDisable() => pauseAction.action.performed -= OnPause;

    private void OnPause(InputAction.CallbackContext ctx)
    {
        if (isPaused == false)
        {
            PauseMenu.SetActive(true);
            isPaused = true;
        }
        else if (isPaused == true)
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        ExitMenu.SetActive(false);
        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    private bool TeleportMenuActive = false;
    public void TeleportMenu()
    {
        if(TeleportMenuActive == false)
        {
            TeleportMenuActive = true;
        }
        else if (TeleportMenuActive == true)
        {
            TeleportMenuActive = false;
        }
        _TeleportMenu.SetActive(TeleportMenuActive);
    }

    public void BackToPauseMenu()
    {
        _TeleportMenu.SetActive(false);
        PauseMenu.SetActive(true);
    }
}