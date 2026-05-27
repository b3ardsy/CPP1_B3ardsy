using UnityEngine;

public class AmmoDrop : PickUp
{
    [SerializeField] private int ammoAmount = 1;

    public override void OnPickup(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerController.specialAmmo += ammoAmount;
        }
    }
}