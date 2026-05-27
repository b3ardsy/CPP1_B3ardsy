using System.Collections;
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
    

    public int healthTanks
    {
        get { return _healthTanks; }
        set
        {
            if (isGameOver || isLoadingScene)
                return;

            if (value > maxHealthTanks)
                _healthTanks = maxHealthTanks;
            else if (value < 0)
                _healthTanks = 0;
            else
                _healthTanks = value;

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

        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == titleSceneName || currentSceneName == gameSceneName)
        {
            ResetPlayerStats();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            if (currentSceneName == titleSceneName)
            {
                StartGame();
            }
            else if (currentSceneName == gameOverSceneName)
            {
                LoadTitle();
            }
            else if (currentSceneName == gameSceneName)
            {
                TogglePause();
            }
        }
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

    public void PlayerTakeDamage(int damage)
    {
        if (isGameOver || isLoadingScene)
            return;

        healthTanks -= damage;
    }

    public void HealPlayer(int healAmount)
    {
        if (isGameOver || isLoadingScene)
            return;

        healthTanks += healAmount;
    }

    public void ResetPlayerStats()
    {
        _healthTanks = startingHealthTanks;
        Debug.Log($"Player stats reset. Health Tanks: {_healthTanks}/{maxHealthTanks}");
    }

    public void LoadGameOverFromAnimation()
    {
        if (isLoadingScene)
            return;

        isGameOver = true;
        isLoadingScene = true;
        isPaused = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene(gameOverSceneName);
    }

    private void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        Debug.Log("Game Paused");
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        Debug.Log("Game Resumed");
    }
}