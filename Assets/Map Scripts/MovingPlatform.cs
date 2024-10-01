using System;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector3 moveDirection = Vector3.right; // Direction of movement
    [SerializeField] private float moveDistance;               // Distance to move
    [SerializeField] private float moveSpeed;                  // Speed of movement

     [Header("Stop Durations")]
    [SerializeField] private float stopDurationAtStart;             // Stop duration at the start
    [SerializeField] private float stopDurationAtEnd;               // Stop duration at the end

    private bool movingForward = true;   // Track direction of movement
    private bool isStopped = false;      // Flag to check if the platform is stopped
    private float stopTimer;             // Timer for stop durations
    private float distanceMoved = 0f;    // Track how far the platform has moved
    private Vector3 initialPosition;     // Store the initial position

    private void Start()
    {
        initialPosition = transform.position; // Store the starting position
    }

    private void FixedUpdate()
    {
        if (isStopped)
        {
            stopTimer -= Time.fixedDeltaTime; // Use fixedDeltaTime for physics consistency
            if (stopTimer <= 0)
            {
                isStopped = false;  // Resume movement after stop duration
            }
            return;
        }

        // Calculate movement distance for this frame
        float moveStep = moveSpeed * Time.fixedDeltaTime; // Use fixedDeltaTime

        if (movingForward)
        {
            distanceMoved += moveStep;
            // Use Lerp for smooth transition between start and end positions
            transform.position = Vector3.Lerp(initialPosition, initialPosition + moveDirection.normalized * moveDistance, distanceMoved / moveDistance);

            if (distanceMoved >= moveDistance)
            {
                movingForward = false;
                distanceMoved = moveDistance;
                isStopped = true;
                stopTimer = stopDurationAtEnd;  // Set stop duration at the end
            }
        }
        else
        {
            distanceMoved -= moveStep;
            // Lerp back to initial position
            transform.position = Vector3.Lerp(initialPosition + moveDirection.normalized * moveDistance, initialPosition, 1 - (distanceMoved / moveDistance));

            if (distanceMoved <= 0)
            {
                movingForward = true;
                distanceMoved = 0;
                isStopped = true;
                stopTimer = stopDurationAtStart;  // Set stop duration at the start
            }
        }
    }
}