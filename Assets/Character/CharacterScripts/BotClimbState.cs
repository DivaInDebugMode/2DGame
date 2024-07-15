using System;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace Character.CharacterScripts
{
    public class BotClimbState : BotBaseState
    {
        private static readonly int WallSlide = Animator.StringToHash("Wall Slide");
        private bool isOnLedge;

        private float ledgeClimbingStartTime;
        private float durationOfLedgeClimbing;
        
        public override void EnterState()
        {
            Physics.gravity = new Vector2(0.0f, -3.5f);
            botData.BotComponents.Rb.velocity = Vector3.zero;
            botAnimatorController.Animator.SetBool(WallSlide, true);
            botData.BotStats.IsWallJump = false;

            botData.BotStats.IsInLedgeClimbing = false;
        }

        public override void UpdateState()
        {
            RotateFromWall();
            JumpAction();
            LedgeAction();

            if (botData.BotStats.IsInLedgeClimbing)
            {
                durationOfLedgeClimbing = Time.time - ledgeClimbingStartTime;
                if (durationOfLedgeClimbing >= 1.3f)
                {
                    botData.BotStats.IsInLedgeClimbing = false;
                    botAnimatorController.Animator.applyRootMotion = false;
                    botData.BotComponents.MoveCollider.enabled = true;

                    botAnimatorController.Animator.SetBool("LedgeClimb", false);

                }
            }
        }

        public override void FixedUpdate()
        {
            JumpPhysicsFromWall();
            HandleLedgeGrab();
        }

        private void RotateFromWall()
        {
            if(botData.BotStats.IsInLedgeClimbing) return;
            
            if (Math.Abs(botData.BotStats.TargetAngle - 270f) < 0.0001f && botInput.MoveDown.action.triggered)
            {
                botData.BotDetectionStats.WallDetectionRadius = 0f;
                botData.BotStats.TargetAngle = 90f;
                botData.BotStats.CurrentDirectionValue = 1;
                botData.BotStats.LastDirectionValue = 1;
            }
            else if (Math.Abs(botData.BotStats.TargetAngle - 90f) < 0.0001f && botInput.MoveDown.action.triggered)
            {
                botData.BotDetectionStats.WallDetectionRadius = 0f;
                botData.BotStats.TargetAngle = 270f;
                botData.BotStats.CurrentDirectionValue = -1;
                botData.BotStats.LastDirectionValue = -1;
            }
        }

        private void LedgeAction()
        {
            if (isOnLedge && botInput.MoveUp.action.triggered && !botData.BotStats.IsInLedgeClimbing)
            {
                botData.BotStats.IsInLedgeClimbing = true;
                ledgeClimbingStartTime = Time.time;
                botData.BotComponents.MoveCollider.enabled = false;
                botAnimatorController.Animator.SetBool("LedgeClimb", true);
                botAnimatorController.Animator.applyRootMotion = true;
            } 
        }

        private void HandleLedgeGrab()
        {
            if (botData.BotDetectionStats.IsLedge && !isOnLedge)
            {
                isOnLedge = true; 
                botData.BotComponents.Rb.velocity = Vector3.zero;
                Physics.gravity = Vector3.zero;
            }
        }

        private void JumpAction()
        {
            if(botData.BotStats.IsInLedgeClimbing) return;

            if (botInput.Jump.action.triggered && !botData.BotStats.IsWallJump)
            {
                Physics.gravity = new Vector2(0.0f, -3.5f);
                botData.BotStats.IsWallJump = true;
                botData.BotStats.WallJumpDurationStart = true;
            }
        }
        private void JumpPhysicsFromWall()
        {
            if (botData.BotStats.IsWallJump)
            {
                switch (botData.BotStats.CurrentDirectionValue)
                {
                    case 1:
                        botData.BotComponents.Rb.velocity = new Vector2(-6, 9);
                        botData.BotStats.TargetAngle = 270f;
                        botData.BotStats.CurrentDirectionValue = -1;
                        botData.BotStats.LastDirectionValue = -1;
                        break;
                    case -1:
                        botData.BotComponents.Rb.velocity = new Vector2(6, 9);
                        botData.BotStats.TargetAngle = 90f;
                        botData.BotStats.CurrentDirectionValue = 1;
                        botData.BotStats.LastDirectionValue = 1;
                        break;
                }
                
            }
        }

        public override void ExitState()
        {
            botAnimatorController.Animator.SetBool("LedgeClimb", false);

            isOnLedge = false;
            botAnimatorController.Animator.SetBool(WallSlide, false);
            botData.BotDetectionStats.WallDetectionRadius = 0.5f;
        }

        public BotClimbState(BotStateMachine currentContext, BotMovement botMovement, BotInput botInput, BotData botData, BotAnimatorController botAnimatorController, BotDash botDash) : base(currentContext, botMovement, botInput, botData, botAnimatorController, botDash)
        {
        }
    }
}