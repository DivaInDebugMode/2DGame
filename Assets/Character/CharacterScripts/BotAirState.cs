using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotAirState : BotBaseState
    {
        private static readonly int AirDash = Animator.StringToHash("AirDash");
        private static readonly int Falling = Animator.StringToHash("Falling");
        private static readonly int Gliding = Animator.StringToHash("Gliding");
        private static readonly int Landing = Animator.StringToHash("Landing");

        private bool landed;

        public override void EnterState()
        {
            botData.BotDetectionStats.WallDetectionRadius = 0.5f;
            landed = false;
            botData.BotStats.IsFalling = true;
            Debug.Log("enter");
            botData.BotStats.DashDuration = botData.BotStats.DashDurationAir;
            HandleFallingGravity();
        }

        public override void UpdateState()
        {
            HandleAirAnimation();
            HandleMovementSpeed();
            HandleAirDashAnimation();
            HandleGliding();
            HandleFallingGravity();
            HandleLanding();
            botData.BotDetection.IsNearOnGround();
        }

        public override void FixedUpdate()
        {
            botMovement.MoveHorizontally(botData.BotStats.CurrentSpeed);
            if (botData.BotStats.IsDashing && !botData.BotDetectionStats.IsNearOnGround)
            {
                botDash.Dash();
            }
        }

        private void HandleMovementSpeed()
        {
            if(botData.BotStats.IsDashing) return;
            if (botData.BotStats.MoveDirection.x != 0)
            {
                
                if (botInput.Run.action.IsPressed())
                {
                    if (Math.Abs(botData.BotStats.CurrentSpeed - botData.BotStats.MaxSpeed) > 0.0001f)
                    {
                        botData.BotStats.CurrentSpeed = botData.BotStats.MaxSpeed;
                    }
                }
                else
                {
                    if (Math.Abs(botData.BotStats.CurrentSpeed - botData.BotStats.WalkSpeed) > 0.0001f)
                    {
                        botData.BotStats.DirectionTime = 0;
                        botData.BotStats.CurrentSpeed = botData.BotStats.WalkSpeed;
                    }
                }
            }
            if (botData.BotStats.MoveDirection.x == 0 && botData.BotStats.CurrentSpeed >= 0f)
            {
                botData.BotStats.CurrentSpeed -= Time.deltaTime * botData.BotStats.MoveDeceleration * 2f;
            }
        }

        private void HandleGliding()
        {
            if (botInput.Jump.action.triggered && !botData.BotStats.IsDashing)
            {
                botData.BotStats.IsGliding = true;
            }
            else if (botData.BotStats.IsGliding && botInput.Jump.action.IsPressed() && botData.BotComponents.Rb.velocity.y < 0 && !botData.BotStats.HasGlided)
            {
                botAnimatorController.Animator.SetBool(Falling, false);
                botAnimatorController.Animator.SetBool(Gliding, true);
                Physics.gravity = botData.BotStats.GlidingG;
                botData.BotStats.HasGlided = true;
            }
            else if (!botInput.Jump.action.IsPressed())
            {
                botAnimatorController.Animator.SetBool(Gliding, false);
                botData.BotStats.IsGliding = false;
                botData.BotStats.HasGlided = false;
                botAnimatorController.Animator.SetBool(Falling, true);
            }
        }

        private void HandleFallingGravity()
        {
            if (!botData.BotStats.IsDashing && !botData.BotStats.IsGliding)
            {
                Physics.gravity = botData.BotStats.AirG;
            }
        }

        private void HandleAirDashAnimation()
        {
            if (botData.BotStats.IsDashing && botData.BotStats.HasDashed && !botData.BotDetectionStats.IsNearOnGround 
                && !botData.BotDetectionStats.IsGrounded)
            {
                Physics.gravity = Vector3.zero;
                botAnimatorController.Animator.SetTrigger(AirDash);
                botData.BotStats.HasDashed = false;
            }
        }

        private void HandleAirAnimation()
        {
            if (botData.BotStats.IsFalling && !botData.BotStats.IsDashing && !botData.BotStats.IsGliding)
            {
                botAnimatorController.Animator.SetBool(Falling, true);
            }
        }

        private void HandleLanding()
        {
            if (botData.BotDetectionStats.IsNearOnGround && !landed)
            {
                landed = true;
                botAnimatorController.Animator.SetBool(Falling, false);
                botAnimatorController.Animator.ResetTrigger(AirDash);
                botAnimatorController.Animator.SetBool(Landing, true);
                botData.BotStats.IsFalling = false;
                botData.BotStats.CanDash = false;
            }
            
            if (!botData.BotDetectionStats.IsNearOnGround)
            {
                botAnimatorController.Animator.SetBool(Landing, false);
            }
        }

        public override void ExitState()
        {
            botAnimatorController.Animator.SetBool(Gliding, false);
            botAnimatorController.Animator.SetBool(Landing, false);
            botAnimatorController.Animator.SetBool(Falling, false);
            botData.BotStats.IsGliding = false;
            botData.BotStats.HasGlided = false;
            botData.BotStats.DashTimer = 0;
            botData.BotStats.CanDash = true;
            botData.BotDetectionStats.IsNearOnGround = false;
        }

        public BotAirState(BotStateMachine currentContext, BotMovement botMovement, BotInput botInput, BotData botData, BotAnimatorController botAnimatorController, BotDash botDash) : base(currentContext, botMovement, botInput, botData, botAnimatorController, botDash)
        {
        }
    }
}