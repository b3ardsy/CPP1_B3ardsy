using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("Player SFX")]
    [SerializeField] private AudioClip playerDeath;
    [SerializeField] private AudioClip playerHit;

    [Header("Enemy SFX")]
    [SerializeField] private AudioClip enemyFireball;

    [Header("Arrow SFX")]
    [SerializeField] private AudioClip basicArrow;
    [SerializeField] private AudioClip fireArrow;
    [SerializeField] private AudioClip iceArrow;

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
        PlayClip(playerDeath);
    }

    public void PlayPlayerHit()
    {
        PlayClip(playerHit);
    }

    public void PlayEnemyFireball()
    {
        PlayClip(enemyFireball);
    }

    public void PlayBasicArrow()
    {
        PlayClip(basicArrow);
    }

    public void PlayFireArrow()
    {
        PlayClip(fireArrow);
    }

    public void PlayIceArrow()
    {
        PlayClip(iceArrow);
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip == null || audioSource == null)
            return;

        audioSource.PlayOneShot(clip);
    }
}