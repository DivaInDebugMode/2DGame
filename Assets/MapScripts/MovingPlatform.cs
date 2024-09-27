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
    [SerializeField] private float stopDurationAtStart;        // Stop duration at the start
    [SerializeField] private float stopDurationAtEnd;          // Stop duration at the end

    private bool movingForward = true;   // Track direction of movement
    private bool isStopped = false;      // Flag to check if the platform is stopped
    private float stopTimer;             // Timer for stop durations
    private float distanceMoved = 0f;    // Track how far the platform has moved

    private void FixedUpdate()
    {
        if (isStopped)
        {
            stopTimer -= Time.deltaTime;
            if (stopTimer <= 0)
            {
                isStopped = false;  // Resume movement after stop duration
            }
            return;
        }

        // Calculate movement distance for this frame
        float moveStep = moveSpeed * Time.deltaTime;

        // Check if the platform is moving forward or backward
        if (movingForward)
        {
            // Move the platform forward
            transform.Translate(moveDirection.normalized * moveStep);
            distanceMoved += moveStep;

            // Check if it has reached the maximum distance
            if (distanceMoved >= moveDistance)
            {
                movingForward = false;  // Reverse direction
                distanceMoved = moveDistance;  // Clamp to max distance
                isStopped = true;
                stopTimer = stopDurationAtEnd;  // Set stop timer for the end
            }
        }
        else
        {
            // Move the platform backward
            transform.Translate(-moveDirection.normalized * moveStep);
            distanceMoved -= moveStep;

            // Check if it has reached the starting position
            if (distanceMoved <= 0)
            {
                movingForward = true;  // Reverse direction
                distanceMoved = 0;  // Clamp to start position
                isStopped = true;
                stopTimer = stopDurationAtStart;  // Set stop timer for the start
            }
        }
    }
}