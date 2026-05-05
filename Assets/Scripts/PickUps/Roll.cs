using UnityEngine;

public class Roll : PickUp
{
    public override void OnPickup(GameObject player)
    {
        player.GetComponent<PlayerController>().PickUpRoll();
    }
}