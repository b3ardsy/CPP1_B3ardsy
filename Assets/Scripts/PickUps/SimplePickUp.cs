using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SimplePickUp : MonoBehaviour
{
    public enum PickupType
    {
        Health = 0,
        Ammo = 1,
        JumpBoost = 2,
        Roll = 3,
        FireArrow = 4,
        IceArrow = 5,
        BountyToken = 6,
    }

//    [SerializeField] private PickupType type;

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            PlayerController controller = collision.GetComponent<PlayerController>();

//            switch (type)
//            {
//                case PickupType.Health:
//                    controller.lives++;
//                    break;

//                case PickupType.JumpBoost:
//                    break; 
//            }
//            Destroy(gameObject);
//        }
//    }
}
