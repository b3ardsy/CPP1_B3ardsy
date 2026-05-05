using UnityEngine;

public class SpecialAmmo : PickUp
{
    [SerializeField] private int ammoToAdd = 1;

    public override void OnPickup(GameObject player)
    {
        player.GetComponent<PlayerController>().specialAmmo += ammoToAdd;
    }
}