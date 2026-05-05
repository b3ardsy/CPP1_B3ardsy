using UnityEngine;

public class Fire : PickUp
{
    public override void OnPickup(GameObject player)
    {
        player.GetComponent<PlayerController>().PickUpFireArrow();
    }
}