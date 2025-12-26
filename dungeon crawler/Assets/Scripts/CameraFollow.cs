using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Follow target
    public Transform target;
    public Vector3 offset;
    public float smoothness;

    // Clamp stuff
    public Vector2 minPos;
    public Vector2 maxPos;

    // Follow()
    Vector3 TargetPos;

    private void Start()
    {
        TargetPos = new Vector3(target.position.x, target.position.y, target.position.z) + offset;
        posToPlayer();
    }

    private void LateUpdate()
    {
        if (transform.position != target.position)
        {
            TargetPos = new Vector3(target.position.x, target.position.y, target.position.z) + offset;

            TargetPos.x = Mathf.Clamp(TargetPos.x, minPos.x, maxPos.x);
            TargetPos.y = Mathf.Clamp(TargetPos.y, minPos.y, maxPos.y);

            transform.position = Vector3.Lerp(transform.position, TargetPos, smoothness * Time.deltaTime);
        }
    }

    public void posToPlayer()
    {
        transform.position = TargetPos;
    }
}
