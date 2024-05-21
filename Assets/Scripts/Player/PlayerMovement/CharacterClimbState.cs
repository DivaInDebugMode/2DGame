
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Player.PlayerMovement // Namespace for managing player movement
{
    // Class representing the character's climb state, inheriting from CharacterBaseState
    public class CharacterClimbState : CharacterBaseState
    {
        // Method called when entering the climb state
        public override void EnterState()
        {
            // Disables jump action, resets attacking flag, resets jump count, and sets gravity scale to 0
          
            detectionStats.IsClimbing = true;
            stats.IsAttacking = false;
            characterComponents.Rb.gravityScale = 0;
            stats.NumberOfJumps = 0;
           
          
        }

        // Method called to update the climb state
        public override void UpdateState()
        {
            // Moves character on the wall and ledge
            MoveOnWall();
            MoveOnLedge();
            if (characterInput.JumpAction.action.triggered)
            {
                stats.IsJump = true;
            }
            
        }

       
        // Method called during fixed update for physics calculations
        public override void FixedUpdate()
        {
            // Moves character vertically and horizontally based on move vector and speed
            characterMovement.MoveVertically(stats.MoveVector.y * stats.MoveSpeed);
            characterJump.Jump();
        }

        // Method called when exiting the climb state
        public override void ExitState()
        {
            // Resets gravity scale and enables jump action
            characterComponents.Rb.gravityScale = 5;
            if (characterInput.MoveLeft.action.ReadValue<float>() < 0.1f && characterComponents.Sr.flipX ||
                characterInput.MoveRight.action.ReadValue<float>() < 0.1f && !characterComponents.Sr.flipX)
            {
                detectionStats.WallCollisionRadius = 0;
            }
            if (!stats.IsJump) detectionStats.IsClimbing = false;
            ctx.StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(0.3f);
            detectionStats.WallCollisionRadius = 0.3f;
            
        }

        // Method to handle movement on the wall
        private void MoveOnWall()
        {
            // Skips if ledge is detected
            if (!detectionStats.IsLedgeDetected)
            {
                if (stats.VelocityY is > 0 or < 0)
                {
                    characterAnimation.ChangeAnimationState(characterAnimation.wallClimbAnim, 0.1f);
                }
                else if (stats.VelocityY == 0)
                {
                    if (stats.MoveVector.x == 0)
                    {
                        characterAnimation.ChangeAnimationState(characterAnimation.wallIdleAnim, 0.1f);
                    }
                    else
                    {
                        characterAnimation.ChangeAnimationState(characterAnimation.ledgeIdleAnim, 0.1f);
                    }
                }
            }
        }
        



        // Method to handle movement on the ledge
        private void MoveOnLedge()
        {
            // Changes animation to ledge idle if ledge is detected but no input for ledge climb
            if (detectionStats.IsLedgeDetected && !characterDetection.IsInputForLedgeClimb)
            {
                characterAnimation.ChangeAnimationState(characterAnimation.ledgeIdleAnim, 0.1f);
            }

            // Changes animation to ledge climb if ledge is detected and input for ledge climb
            if (detectionStats.IsLedgeDetected && characterDetection.IsInputForLedgeClimb)
            {
                characterAnimation.ChangeAnimationState(characterAnimation.ledgeClimbAnim, 0.1f);
            }
        }

        // Constructor initializing the base state with necessary components
        public CharacterClimbState(CharacterStateMachine currentContext, CharacterStateFactory characterStateFactory,
            CharacterStats characterStats, DetectionStats detectionStats, CharacterDetection characterDetection,
            CharacterComponents components, CharacterInput input, CharacterDash characterDash,
            CharacterMovement characterMovement, CharacterJump characterJump, CharacterAnimation characterAnimation) 
            : base(currentContext, characterStateFactory, characterStats, detectionStats, characterDetection,
                components, input, characterDash, characterMovement, characterJump, characterAnimation)
        {
        }
    }
}