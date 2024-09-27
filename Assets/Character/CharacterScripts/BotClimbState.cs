using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotClimbState : BotBaseState
    {
        private static readonly int WallSlide = Animator.StringToHash("Wall Slide");
        private bool isOnLedge;
        
        public override void EnterState()
        {
            Physics.gravity = botData.BotStats.WallGForce;
            botData.Rb.velocity = Vector3.zero;
            botData.Animator.SetBool(WallSlide, true);
            botData.BotStats.IsWallJump = false;
            botData.BotStats.IsInLedgeClimbing = false;
            botData.BotStats.HasStopped = true;
        }

        public override void UpdateState()
        {
            RotateFromWall();
            LedgeAction();
        }

        public override void FixedUpdate()
        {
            HandleLedgeGrab();
        }

        private void RotateFromWall()
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
        }

        private void LedgeAction()
        {
            switch ( botData.BotStats.CurrentDirectionValue)
            {
                case 1:
                    if (isOnLedge && botInput.MoveUp.action.IsPressed() | botInput.MoveRight.action.IsPressed()  && !botData.BotStats.IsInLedgeClimbing)
                    {
                        botData.BotStats.LedgeClimbingStartTime = Time.time;
                        botData.Rb.velocity = 
                            new Vector2(-botData.BotStats.LedgeJumpForce.x, botData.BotStats.LedgeJumpForce.y); 
                        Physics.gravity = botData.BotStats.FallingGForce;
                        botData.BotDetectionStats.IsLedge = false;
                        botData.BotStats.IsInLedgeClimbing = true;
                    } 
                    break;
                case -1:
                    if (isOnLedge && botInput.MoveUp.action.IsPressed() | botInput.MoveLeft.action.IsPressed()  && !botData.BotStats.IsInLedgeClimbing)
                    {
                        botData.BotStats.LedgeClimbingStartTime = Time.time;
                        botData.Rb.velocity = botData.BotStats.LedgeJumpForce; 
                        Physics.gravity = botData.BotStats.FallingGForce;
                        botData.BotDetectionStats.IsLedge = false;
                        botData.BotStats.IsInLedgeClimbing = true;
                    } 
                    break;
            }
        }
        
        private void HandleLedgeGrab()
        {
            if (botData.BotDetectionStats.IsLedge && !isOnLedge)
            {
                isOnLedge = true; 
                botData.Rb.velocity = Vector3.zero;
                Physics.gravity = Vector3.zero;
            }
        }

        public override void ExitState()
        { 
            isOnLedge = false;
            botData.Animator.SetBool(WallSlide, false);
        }

        public BotClimbState(BotStateMachine currentContext, BotMovement botMovement, BotInput botInput,
            BotData botData) : base(currentContext, botMovement, botInput,
            botData)
        {
        }
    }
}