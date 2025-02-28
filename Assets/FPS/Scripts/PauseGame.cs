using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button menuButton;
    private bool isPaused = false;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
        menuButton.onClick.AddListener(MenuManager.Instance.GoToMenu);
        LockCursor(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else Pause();
        }
    }

    private void Pause()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        LockCursor(false);
    }

    private void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        LockCursor(true);
    }

    private void LockCursor(bool lockCursor)
    {
        Cursor.visible = !lockCursor;
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
    }
}