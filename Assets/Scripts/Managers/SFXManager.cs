using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip playerDeath;
    [SerializeField] private AudioClip playerHit;
    [SerializeField] private AudioClip enemyFireball;
    [SerializeField] private AudioClip basicArrow;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPlayerDeath()
    {
        audioSource.PlayOneShot(playerDeath);
    }

    public void PlayPlayerHit()
    {
        audioSource.PlayOneShot(playerHit);
    }

    public void PlayEnemyFireball()
    {
        audioSource.PlayOneShot(enemyFireball);
    }

    public void PlayBasicArrow()
    {
        audioSource.PlayOneShot(basicArrow);
    }
}