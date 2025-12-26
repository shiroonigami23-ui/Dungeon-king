using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Following")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10);
    public float smoothness = 5f;

    [Header("Clamping")]
    public Vector2 minPos;
    public Vector2 maxPos;

    private Vector3 shakeOffset;

    private void Start()
    {
        // Snap camera to player at the very start
        if (target != null)
        {
            Vector3 startPos = target.position + offset;
            transform.position = startPos;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // 1. Calculate the target position
        Vector3 targetPos = target.position + offset;

        // 2. Apply Clamping (Stay within boundaries)
        targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);

        // 3. Smoothly Lerp to that position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPos, smoothness * Time.deltaTime);
        
        // 4. Final Position = Smooth Path + Shake Vibrations
        transform.position = smoothedPosition + shakeOffset;
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            shakeOffset = new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        shakeOffset = Vector3.zero;
    }
}