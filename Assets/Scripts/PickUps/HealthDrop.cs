using UnityEngine;

public class HealthDrop : PickUp
{
    [SerializeField] private int healAmount = 1;

    public override void OnPickup(GameObject player)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.HealPlayer(healAmount);
        }
    }
}