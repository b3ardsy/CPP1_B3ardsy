using UnityEngine;

public class BountyToken : PickUp
{
    [SerializeField] private int tokensToAdd = 1;

    public override void OnPickup(GameObject player)
    {
        player.GetComponent<PlayerController>().bountyTokens += tokensToAdd;
    }
}