using System.Collections;
using UnityEngine;

public class Tavern : MonoBehaviour
{
    [Header("Messages")]
    [TextArea]
    [SerializeField]
    private string tavernMessage =
        "Happy Hour 5 - 7!\nEntrance Fee: 1 Bounty Token";

    [TextArea]
    [SerializeField]
    private string completedMessage =
        "We're sorry. The Tavern is closed due to scope creep.\nThanks for playing!";

    [Header("Ending")]
    [SerializeField] private float endingDelay = 5f;

    private InGameHUD hud;
    private bool endingTriggered = false;

    void Start()
    {
        hud = FindAnyObjectByType<InGameHUD>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player == null)
            return;

        PlayerStats stats = collision.GetComponent<PlayerStats>();

        if (stats == null)
            return;

        if (hud == null)
            hud = FindAnyObjectByType<InGameHUD>();

        if (hud == null)
            return;

        if (stats.BountyTokens >= 1)
        {
            hud.ShowPersistentAlert(completedMessage);

            if (!endingTriggered)
            {
                endingTriggered = true;
                StartCoroutine(ReturnToTitleRoutine());
            }
        }
        else
        {
            hud.ShowPersistentAlert(tavernMessage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() == null)
            return;

        if (endingTriggered)
            return;

        if (hud != null)
            hud.ClearPersistentAlert();
    }

    private IEnumerator ReturnToTitleRoutine()
    {
        yield return new WaitForSecondsRealtime(endingDelay);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadTitle();
        }
    }
}