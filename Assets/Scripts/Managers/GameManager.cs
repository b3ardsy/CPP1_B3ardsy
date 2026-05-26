using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

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

    public int healthTanks
    {
        get { return _healthTanks; }
        set
        {
            if (isGameOver || isLoadingScene)
                return;

            if (value <= 0)
            {
                _healthTanks = 0;
                GameOver();
                return;
            }

            if (value > maxHealthTanks)
                _healthTanks = maxHealthTanks;
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

        ResetPlayerStats();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        }
    }

    public void StartGame()
    {
        isGameOver = false;
        isLoadingScene = false;

        ResetPlayerStats();
        SceneManager.LoadScene(gameSceneName);
    }

    public void LoadTitle()
    {
        isGameOver = false;
        isLoadingScene = false;

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

    private void GameOver()
    {
        if (isGameOver || isLoadingScene)
            return;

        isGameOver = true;
        isLoadingScene = true;

        Debug.Log("Game Over");
        StartCoroutine(LoadGameOverScene());
    }

    private IEnumerator LoadGameOverScene()
    {
        yield return null;

        SceneManager.LoadScene(gameOverSceneName);

        isLoadingScene = false;
    }
}