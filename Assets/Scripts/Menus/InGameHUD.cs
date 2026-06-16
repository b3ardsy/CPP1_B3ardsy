using TMPro;
using UnityEngine;

public class InGameHUD : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text specialAmmoText;

    [Header("Player Stats")]
    [SerializeField] private PlayerStats playerStats;

    void Start()
    {
        if (playerStats == null)
        {
            playerStats = FindAnyObjectByType<PlayerStats>();
        }

        UpdateHUD();
    }

    void Update()
    {
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (playerStats == null)
            return;

        if (healthText != null)
        {
            healthText.text = "Health: " + playerStats.CurrentHealth + " / " + playerStats.MaxHealth;
        }

        if (specialAmmoText != null)
        {
            specialAmmoText.text = "Ammo: " + playerStats.CurrentSpecialAmmo + " / " + playerStats.MaxSpecialAmmo;
        }
    }
}