using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Player.PlayerMovement
{
    public class CharacterJump : MonoBehaviour
        {
            [SerializeField] private CharacterStats characterStats; // Reference to character's stats
            [SerializeField] private CharacterComponents characterComponents; // Reference to character's components
            [SerializeField] private CharacterDetection characterDetection; // Reference to character's detection
            [SerializeField] private CharacterInput input; // Reference to character's input
            [SerializeField] private DetectionStats detectionStats;
            private const float Tolerance = 0.0001f; // Adjust as needed
            
            private bool isPressed;
            private float pressStartTime;
            private bool isLongJump;
            private float jumpPressedTime;
            private void OnEnable()
            {
                // Subscribe to jump action when enabled
                input.JumpAction.action.started += JumpActionPress;
                input.JumpAction.action.canceled += OnButtonCancel;
            }

            private void OnDisable()
            {
                // Unsubscribe from jump action when disabled
                input.JumpAction.action.started -= JumpActionPress;
                input.JumpAction.action.canceled -= OnButtonCancel;

            }
            
            // Method triggered when jump action is performed
            private void JumpActionPress(InputAction.CallbackContext context)
            {
                // Check if the character can jump
                if (characterStats.NumberOfJumps < characterStats.MaxJump && !characterStats.IsDashing && !characterStats.IsAttacking)
                {
                    pressStartTime = Time.time;
                    characterStats.NumberOfJumps++;
                    characterStats.IsJump = true;
                    characterStats.EndedJumpEarly = false;
                }
            }

            private void FixedUpdate()
            {
                if (!isPressed) return;
                jumpPressedTime = Time.time - pressStartTime;
                if (jumpPressedTime <= 0.15f) characterComponents.Rb.velocity = 
                    new Vector2(characterComponents.Rb.velocity.x, characterStats.JumpForce);
            }


            private void OnButtonCancel(InputAction.CallbackContext context)
            {
                isPressed = false;
            }
            
            // Method to execute the jumpS
            public void Jump()
            {
                // Check if the character can jump
                if (!characterStats.IsJump) return;

                switch (detectionStats.IsClimbing)
                {
                    case true:
                    {
                        if ((Math.Abs(characterDetection.RayDirection.x - (-1)) < Tolerance &&
                             Math.Abs(input.MoveRight.action.ReadValue<float>() - 1f) < Tolerance) ||
                            (Math.Abs(characterDetection.RayDirection.x - 1) < Tolerance &&
                             Math.Abs(input.MoveLeft.action.ReadValue<float>() - 1f) < Tolerance))
                        {
                            characterComponents.Rb.velocity =
                                new Vector2(characterStats.MoveVector.x, characterStats.JumpForce);

                        }
                        else
                        {
                            characterComponents.Rb.AddForce(new Vector2(0, -5), ForceMode2D.Impulse);
                            characterComponents.Sr.flipX = !characterComponents.Sr.flipX;
                            detectionStats.IsClimbing = false;
                            characterStats.NumberOfJumps++;
                        }

                        detectionStats.IsClimbing = false;
                        break;
                    }
                    default:
                        isPressed = true;
                        break;
                }
                
                // Reset jump flag
                characterStats.IsJump = false;
            }
            
            // Method to apply gravity
            public void ApplyGravity()
            {
                // If the jump ended early, adjust gravity
                    if (characterStats.EndedJumpEarly && characterComponents.Rb.velocity.y > 0)
                    {
                        characterComponents.Rb.gravityScale *= characterStats.JumpEndEarlyGravityModifier;
                        //Debug.Log(gravity);
                    }
                
                    // If character is grounded or moving upwards, exit
                    if (detectionStats.IsGrounded || !(characterComponents.Rb.gravityScale <= 0f)) return;
                
                     var newVelocityY = Mathf.MoveTowards(characterComponents.Rb.velocity.y, 5,
                         characterComponents.Rb.gravityScale);
                    characterComponents.Rb.velocity = new Vector2(characterComponents.Rb.velocity.x, newVelocityY);
            }


            // Method to handle early jump end
            public void HandleJumpEnd()
            {
                // If character is not grounded, not jumping, and moving upwards, set jump ended early flag
                if (!detectionStats.IsGrounded && !characterStats.IsJump && characterComponents.Rb.velocity.y > 0)
                {
                    characterStats.EndedJumpEarly = true;
                }
            }
        }
    }
