using System;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace Character.CharacterScripts
{
    public class BotClimbState : BotBaseState
    {
        private static readonly int WallSlide = Animator.StringToHash("Wall Slide");
        
        public override void EnterState()
        {
            Physics.gravity = new Vector2(0.0f, -3.5f);
            botData.BotComponents.Rb.velocity = Vector3.zero;
            botAnimatorController.Animator.SetBool(WallSlide, true);
            botData.BotStats.IsWallJump = false;
        }

        public override void UpdateState()
        {
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

            if (botInput.Jump.action.triggered && !botData.BotStats.IsWallJump)
            {
                botData.BotStats.IsWallJump = true;
                botData.BotStats.WallJumpDurationStart = true;
            }
        }

        public override void FixedUpdate()
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
            botAnimatorController.Animator.SetBool(WallSlide, false);
            botData.BotDetectionStats.WallDetectionRadius = 0.5f;
        }

        public BotClimbState(BotStateMachine currentContext, BotMovement botMovement, BotInput botInput, BotData botData, BotAnimatorController botAnimatorController, BotDash botDash) : base(currentContext, botMovement, botInput, botData, botAnimatorController, botDash)
        {
        }
    }
}