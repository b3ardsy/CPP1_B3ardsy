using UnityEngine;

public class Ice : PickUp
{
    public override void OnPickup(GameObject player)
    {
        player.GetComponent<PlayerController>().PickUpIceArrow();
    }
}