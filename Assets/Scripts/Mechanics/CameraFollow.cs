using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("X Bounds")]
    [SerializeField] private float minXPos;
    [SerializeField] private float maxXPos;

    [Header("Follow Target")]
    [SerializeField] private Transform target;

    [Header("Camera Feel")]
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private float yOffset = 2f; // headroom above the player

    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
            {
                Debug.LogError("CameraFollow: No target assigned and no GameObject with tag 'Player' exists.");
                return;
            }

            target = player.transform;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetCameraPos = transform.position;

        // Follow X, but keep it clamped between min/max.
        targetCameraPos.x = Mathf.Clamp(target.position.x, minXPos, maxXPos);

        // Follow Y with some headroom above the player.
        targetCameraPos.y = target.position.y + yOffset;

        // Keep camera Z unchanged.
        targetCameraPos.z = transform.position.z;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetCameraPos,
            followSpeed * Time.deltaTime
        );
    }
}