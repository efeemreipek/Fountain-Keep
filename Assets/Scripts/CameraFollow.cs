using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 baseOffset = new Vector3(0f, 10f, -5f);
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private float shiftAmount = 3f;

    private Vector3 targetOffset;
    private Vector3 lastPosition;

    void Start()
    {
        targetOffset = baseOffset;
        lastPosition = target.position;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate player movement direction
            Vector3 movementDirection = target.position - lastPosition;
            lastPosition = target.position;

            // Adjust camera offset based on movement direction
            if (movementDirection != Vector3.zero)
            {
                Vector3 normalizedDirection = movementDirection.normalized;
                targetOffset = baseOffset + new Vector3(normalizedDirection.x * shiftAmount, 0, normalizedDirection.z * shiftAmount);
            }
            else
            {
                // Reset to base offset when player is stationary
                targetOffset = baseOffset;
            }

            // Desired position for the camera
            Vector3 desiredPosition = target.position + targetOffset;

            // Smoothly interpolate between the current position and the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update the camera's position without rotation
            transform.position = smoothedPosition;
        }
    }
}
