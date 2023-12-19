using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauzeScript : MonoBehaviour
{
    private bool isPaused = false;

    [SerializeField] private GameObject pauseScreen;
    public InputAction pauseAction;

    private void OnEnable()
    {
        pauseAction.Enable();
        pauseAction.performed += _ => TogglePause();
    }

    private void OnDisable()
    {
        pauseAction.Disable();
        pauseAction.performed -= _ => TogglePause();
    }
    void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        if (isPaused)
        {
            pauseScreen.SetActive(true);
        }
        else
        {
            pauseScreen.SetActive(false);
        }
    }

    public void ResumeButton()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void mainmenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(0);
    }

}
