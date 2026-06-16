using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameHUD : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text specialAmmoText;

    [Header("Special Arrow Images")]
    [SerializeField] private Image fireArrowImage;
    [SerializeField] private Image iceArrowImage;

    [Header("Player Stats")]
    [SerializeField] private PlayerStats playerStats;

    void Start()
    {
        if (playerStats == null)
            playerStats = FindAnyObjectByType<PlayerStats>();

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
            healthText.text = "Health: " + playerStats.CurrentHealth + " / " + playerStats.MaxHealth;

        if (specialAmmoText != null)
        {
            specialAmmoText.gameObject.SetActive(playerStats.HasAnySpecialArrow);

            if (playerStats.HasAnySpecialArrow)
            {
                specialAmmoText.text = playerStats.CurrentSpecialAmmo + " / " + playerStats.MaxSpecialAmmo;
            }
        }

        if (fireArrowImage != null)
            fireArrowImage.gameObject.SetActive(playerStats.EquippedSpecialArrow == SpecialArrowType.Fire);

        if (iceArrowImage != null)
            iceArrowImage.gameObject.SetActive(playerStats.EquippedSpecialArrow == SpecialArrowType.Ice);
    }
}