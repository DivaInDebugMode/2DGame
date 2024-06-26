using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotAirState : BotBaseState
    {
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int AirDash = Animator.StringToHash("AirDash");
        private static readonly int Falling = Animator.StringToHash("Falling");
        private static readonly int Gliding = Animator.StringToHash("Gliding");

        public override void EnterState()
        {
            botData.BotStats.IsFalling = true;
            botData.BotStats.DashDuration = 0.5f;
            HandleFallingGravity();
        }

        public override void UpdateState()
        {
            HandleAirAnimation();
            HandleMovementSpeed();
            HandleAirDashAnimation();
            HandleGliding();
            HandleFallingGravity();
        }

        public override void FixedUpdate()
        {
            botMovement.MoveHorizontally(botData.BotStats.CurrentSpeed);
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

        public override void ExitState()
        {
            botAnimatorController.Animator.SetBool(Gliding, false);
            botData.BotStats.IsGliding = false;
            botData.BotStats.HasGlided = false;
        }

        private void HandleGliding()
        {
            if (botInput.Jump.action.triggered && !botData.BotStats.IsDashing)
            {
                botData.BotStats.IsGliding = true;
            }
            else if (botData.BotStats.IsGliding && botInput.Jump.action.IsPressed() && botData.BotComponents.Rb.velocity.y < 0 && !botData.BotStats.HasGlided)
            {
                botAnimatorController.Animator.SetBool(Gliding, true);
                Physics.gravity = new Vector2(0.0f, -3f);
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
                Physics.gravity = new Vector2(0.0f, -35f);
            }
        }
        private void HandleAirDashAnimation()
        {
            if (!botData.BotStats.IsDashing || !botData.BotStats.HasDashed) return;
            Physics.gravity = Vector3.zero;
            botAnimatorController.Animator.SetTrigger(AirDash);
            botData.BotStats.HasDashed = false;
        }

        private void HandleAirAnimation()
        {
            if (botInput.Jump.action.inProgress && botData.BotStats.IsJump && !ctx.test)
            {
                botAnimatorController.Animator.SetTrigger(Jump);
                ctx.test = true;
            }

            if (!botData.BotStats.IsJump && botData.BotStats.IsFalling && !botData.BotStats.IsDashing && !botData.BotStats.IsGliding)
            {
                botAnimatorController.Animator.SetBool(Falling, true);
            }
        }


        public BotAirState(BotStateMachine currentContext, BotMovement botMovement, BotInput botInput, BotData botData, BotAnimatorController botAnimatorController, BotJump botJump) : base(currentContext, botMovement, botInput, botData, botAnimatorController, botJump)
        {
        }
    }
}