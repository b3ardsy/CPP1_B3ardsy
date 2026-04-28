using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private float minXPos;
    [SerializeField] private float maxXPos;

    [SerializeField] private Transform target;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Make code defensive against bad input!

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

    // Update is called once per frame
    void LateUpdate()
    {
        //early return - if we don't have a target we can't follow anything so we should exit the method.

        if (target == null) return;

        //Store our current position
        Vector3 currentPos = transform.position;

        //update the X position to be the same as the target's x position, but clamped between our min and max X values
        currentPos.x = Mathf.Clamp(target.position.x, minXPos, maxXPos);

        //apply the updated position to the camera
        //transform.position = currentPos;

        //Alt method below:
        transform.position = Vector3.MoveTowards(transform.position, currentPos, 10f * Time.deltaTime);
    }
}
