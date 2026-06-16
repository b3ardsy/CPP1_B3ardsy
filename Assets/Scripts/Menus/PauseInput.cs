using UnityEngine;

public class PauseInput : MonoBehaviour
{
    [SerializeField] private MenuController menuController;

    private bool isPaused = false;

    void Start()
    {
        if (menuController == null)
            menuController = GetComponent<MenuController>();

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        menuController.JumpTo(MenuStates.PauseMenu);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        menuController.HideCurrentMenu();
    }
}