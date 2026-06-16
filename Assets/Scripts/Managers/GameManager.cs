using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public bool IsPaused { get { return isPaused; } }

    [Header("Scene Names")]
    [SerializeField] private string titleSceneName = "Title";
    [SerializeField] private string gameSceneName = "SampleScene";

    private bool isGameOver = false;
    private bool isLoadingScene = false;
    private bool isPaused = false;

    public int lives
    {
        get
        {
            PlayerStats stats = FindAnyObjectByType<PlayerStats>();

            if (stats == null)
                return 0;

            return stats.CurrentHealth;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        isGameOver = false;
        isLoadingScene = false;
        isPaused = false;

        Time.timeScale = 1f;

        SceneManager.LoadScene(gameSceneName);
    }

    public void LoadTitle()
    {
        isGameOver = false;
        isLoadingScene = false;
        isPaused = false;

        Time.timeScale = 1f;

        SceneManager.LoadScene(titleSceneName);
    }

    public void LoadGameOverFromAnimation()
    {
        if (isLoadingScene)
            return;

        isGameOver = true;
        isLoadingScene = true;
        isPaused = false;

        Time.timeScale = 1f;

        SceneManager.LoadScene(titleSceneName);
    }

    public void PlayerTakeDamage(int damage)
    {
        if (isGameOver || isLoadingScene || isPaused)
            return;

        PlayerStats stats = FindAnyObjectByType<PlayerStats>();

        if (stats == null)
            return;

        stats.TakeDamage(damage);
    }

    public void HealPlayer(int healAmount)
    {
        if (isGameOver || isLoadingScene || isPaused)
            return;

        PlayerStats stats = FindAnyObjectByType<PlayerStats>();

        if (stats == null)
            return;

        stats.Heal(healAmount);
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        Debug.Log("Game Resumed");
    }
}