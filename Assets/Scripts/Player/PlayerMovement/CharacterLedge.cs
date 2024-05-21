using System.Collections;
using UnityEngine;

namespace Player.PlayerMovement
{
    // This class handles ledge detection and related actions
    public class CharacterLedge : MonoBehaviour
    {
        [SerializeField] private CharacterComponents characterComponents; // Reference to character components
        [SerializeField] private CharacterDetection characterDetection; // Reference to character detection
        [SerializeField] private DetectionStats detectionStats; // Reference to detection stats
        [SerializeField] private CharacterStats stats; // Reference to character stats
        [SerializeField] private CharacterInput characterInput; // Reference to character input

        // Update is called once per frame
        private void Update()
        {
            // Check ledge conditions
            LedgeCondition();
        }

        // Check conditions related to ledge detection
        private void LedgeCondition()
        {
            // If head ray hit collider
            if (characterDetection.HeadRayHit.collider is not null)
            {
                detectionStats.IsLedgeDetected = true; // Ledge detected
                characterComponents.Rb.velocity = Vector2.zero; // Stop character movement
                StartCoroutine(StayOnLedge()); // Start coroutine to stay on ledge
            }
            // If head ray doesn't hit collider
            else if (characterDetection.HeadRayHit.collider is null)
            {
                detectionStats.IsLedgeDetected = false; // Ledge not detected
                characterDetection.HasMoveOnLedge = false; // Character doesn't have move on ledge
                characterDetection.IsInputForLedgeClimb = false; // No input for ledge climb
                stats.MoveSpeed = 5; // Reset movement speed
            }
        }

        // Coroutine to handle actions while staying on ledge
        private IEnumerator StayOnLedge()
        {
            if (!characterDetection.HasMoveOnLedge)
            {
                stats.MoveSpeed = 0; // Set movement speed to 0
                characterDetection.HasMoveOnLedge = true; // Character has move on ledge
            }
            yield return new WaitForSecondsRealtime(0.05f); // Wait for a short duration
            if (characterInput.MoveUp.action.ReadValue<float>() > 0)
            {
                characterDetection.IsInputForLedgeClimb = true; // Input detected for ledge climb
            }
            // If input detected to move away from ledge
            if (characterInput.MoveDown.action.ReadValue<float>() > 0 || !characterComponents.Sr.flipX &&
                characterInput.MoveLeft.action.ReadValue<float>() > 0 || characterComponents.Sr.flipX &&
                characterInput.MoveRight.action.ReadValue<float>() > 0)
            {
                stats.MoveSpeed = 5; // Set movement speed back to normal
            }
        }

        // Reset ledge-related input and movement speed
        private void ResetLedgeInput()
        {
            characterDetection.IsInputForLedgeClimb = false; // Reset input for ledge climb
            stats.MoveSpeed = 5; // Reset movement speed
        }

        // Process finishing actions after ledge climb
        public void LedgeFinishProcess()
        {
            // If character is facing left
            if (characterComponents.Sr.flipX)
            {
                characterComponents.LedgeFinishLeft.enabled = true; // Enable left ledge finish animation
                characterComponents.Sr.enabled = false; // Disable character sprite renderer temporarily
                StartCoroutine(SetFinishSr()); // Start coroutine to set finish sprite renderer
                var transform1 = transform; // Get transform reference
                var position = transform1.position; // Get current position
                position = new Vector2((position.x - 0.38f), (position.y + 1.25f)); // Adjust position for left ledge finish
                transform1.position = position; // Update character position
            }

            // If character is facing right
            if (characterComponents.Sr.flipX) return; // If character is not facing right, exit function
            {
                characterComponents.LedgeFinishRight.enabled = true; // Enable right ledge finish animation
                characterComponents.Sr.enabled = false; // Disable character sprite renderer temporarily
                StartCoroutine(SetFinishSr()); // Start coroutine to set finish sprite renderer
                var transform1 = transform; // Get transform reference
                var position = transform1.position; // Get current position
                position = new Vector2((position.x + 0.38f), (position.y + 1.25f)); // Adjust position for right ledge finish
                transform1.position = position; // Update character position
            }
        }

        // Coroutine to set finish sprite renderer and reset animations
        private IEnumerator SetFinishSr()
        {
            yield return new WaitForSecondsRealtime(0.15f); // Wait for a short duration
            characterComponents.Sr.enabled = true; // Enable character sprite renderer
            characterComponents.LedgeFinishRight.enabled = false; // Disable right ledge finish animation
            characterComponents.LedgeFinishLeft.enabled = false; // Disable left ledge finish animation
        }
    }
}