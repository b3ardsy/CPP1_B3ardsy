using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button returnToMenuButton;
    [SerializeField] private Button resumeButton;

    [Header("In Game UI References")]
    [SerializeField] private TMP_Text livesText;

    [Header("Menu References")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject pauseMenu;

    void Start()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (startButton != null)
            startButton.onClick.AddListener(GameManager.Instance.StartGame);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(() => SetMenu(settingsMenu, mainMenu));

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

        if (backButton != null)
            backButton.onClick.AddListener(() => SetMenu(mainMenu, settingsMenu));

        if (returnToMenuButton != null)
            returnToMenuButton.onClick.AddListener(GameManager.Instance.LoadTitle);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
    }

    void Update()
    {
        if (livesText != null && GameManager.Instance != null)
            livesText.text = "Lives: " + GameManager.Instance.lives;

        if (pauseMenu == null || GameManager.Instance == null)
            return;

        if (Input.GetKeyDown(KeyCode.P))
            TogglePauseMenu();
    }

    void TogglePauseMenu()
    {
        GameManager.Instance.TogglePause();
        pauseMenu.SetActive(GameManager.Instance.IsPaused);
    }

    void ResumeGame()
    {
        GameManager.Instance.ResumeGame();

        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    void SetMenu(GameObject menuToActivate, GameObject menuToDeactivate)
    {
        if (menuToActivate != null) menuToActivate.SetActive(true);
        if (menuToDeactivate != null) menuToDeactivate.SetActive(false);
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}