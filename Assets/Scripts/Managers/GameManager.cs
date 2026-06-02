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
    [SerializeField] private string gameOverSceneName = "GameOver";

    [Header("Player Health")]
    [SerializeField] private int maxHealthTanks = 9;
    [SerializeField] private int startingHealthTanks = 3;

    private int _healthTanks;
    private bool isGameOver = false;
    private bool isLoadingScene = false;
    private bool isPaused = false;

    
    public int lives
    {
        get { return healthTanks; }
    }

    public int healthTanks
    {
        get { return _healthTanks; }

        set
        {
            if (isGameOver || isLoadingScene)
                return;

            _healthTanks = Mathf.Clamp(value, 0, maxHealthTanks);

            Debug.Log($"Health Tanks: {_healthTanks}/{maxHealthTanks}");
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

        ResetPlayerStats();
    }

    public void StartGame()
    {
        isGameOver = false;
        isLoadingScene = false;
        isPaused = false;

        Time.timeScale = 1f;

        ResetPlayerStats();

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

        healthTanks -= damage;
    }

    public void HealPlayer(int healAmount)
    {
        if (isGameOver || isLoadingScene || isPaused)
            return;

        healthTanks += healAmount;
    }

    public void ResetPlayerStats()
    {
        _healthTanks = startingHealthTanks;

        Debug.Log($"Player stats reset. Health Tanks: {_healthTanks}/{maxHealthTanks}");
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