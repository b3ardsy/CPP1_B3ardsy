using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameHUD : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text specialAmmoText;
    [SerializeField] private TMP_Text alertText;

    [Header("Alert Settings")]
    [SerializeField] private float alertDuration = 3f;

    [Header("Special Arrow Images")]
    [SerializeField] private Image fireArrowImage;
    [SerializeField] private Image iceArrowImage;

    [Header("Player Stats")]
    [SerializeField] private PlayerStats playerStats;

    private Coroutine alertCoroutine;

    void Start()
    {
        if (playerStats == null)
            playerStats = FindAnyObjectByType<PlayerStats>();

        if (playerStats != null)
            playerStats.OnAlertRequested += ShowAlert;

        if (alertText != null)
            alertText.gameObject.SetActive(false);

        UpdateHUD();
    }

    void OnDestroy()
    {
        if (playerStats != null)
            playerStats.OnAlertRequested -= ShowAlert;
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
                specialAmmoText.text = playerStats.CurrentSpecialAmmo + " / " + playerStats.MaxSpecialAmmo;
        }

        if (fireArrowImage != null)
            fireArrowImage.gameObject.SetActive(playerStats.EquippedSpecialArrow == SpecialArrowType.Fire);

        if (iceArrowImage != null)
            iceArrowImage.gameObject.SetActive(playerStats.EquippedSpecialArrow == SpecialArrowType.Ice);
    }

    private void ShowAlert(string message)
    {
        if (alertText == null)
            return;

        if (alertCoroutine != null)
            StopCoroutine(alertCoroutine);

        alertCoroutine = StartCoroutine(AlertRoutine(message));
    }

    private IEnumerator AlertRoutine(string message)
    {
        alertText.text = message;
        alertText.gameObject.SetActive(true);

        yield return new WaitForSeconds(alertDuration);

        alertText.gameObject.SetActive(false);
        alertCoroutine = null;
    }
}