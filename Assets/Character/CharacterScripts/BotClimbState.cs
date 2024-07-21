using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotClimbState : BotBaseState
    {
        private static readonly int WallSlide = Animator.StringToHash("Wall Slide");
        private bool isOnLedge;
        private HingeJoint test;
        
        public override void EnterState()
        {
            if (botData.BotDetectionStats.IsWall)
            {
                Physics.gravity = botData.BotStats.WallGForce;
                botData.BotComponents.Rb.velocity = Vector3.zero;
            
                botAnimatorController.Animator.SetBool(WallSlide, true);
                botData.BotStats.IsWallJump = false;
                botData.BotStats.IsInLedgeClimbing = false;
            }else if (botData.BotDetectionStats.IsRope)
            {
                botData.BotComponents.Rb.velocity = Vector3.zero;
                Physics.gravity = new Vector2(0, -5f);
            }
           
        }

        public override void UpdateState()
        {
            RotateFromWall();
            JumpAction();
            LedgeAction();
        }

        public override void FixedUpdate()
        {
            JumpPhysicsFromWall();
            HandleLedgeGrab();
            if (botData.BotDetectionStats.IsRopeTail)
            {
                botData.BotComponents.Rb.velocity = Vector3.zero;
                if (ctx.transform.GetComponent<HingeJoint>() == null)
                {
                    test = ctx.transform.AddComponent<HingeJoint>();
                    test.connectedBody = botData.BotDetection.RopeTrailHit.rigidbody;
                    botData.BotDetection.RopeTrailHit.rigidbody.velocity = new Vector2(8, 6);
                    Debug.Log("hit");
                }
                
                botData.BotStats.VelocityX = Mathf.MoveTowards(botData.BotComponents.Rb.velocity.x,
                    botData.BotStats.MoveDirection.x * 3, botData.BotStats.SmoothTime * Time.fixedTime);
                
                
                botData.BotDetection.RopeTrailHit.rigidbody.AddForce(
                    new Vector2(botData.BotStats.VelocityX, botData.BotDetection.RopeTrailHit.rigidbody.velocity.y),
                    ForceMode.Acceleration);

                Physics.gravity = new Vector2(0, 0f);
            }
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
            if (isOnLedge && botInput.MoveUp.action.triggered && !botData.BotStats.IsInLedgeClimbing)
            {
                botData.BotStats.LedgeClimbingStartTime = Time.time;
                botData.BotComponents.MoveCollider.isTrigger = true;
                botData.BotComponents.Rb.velocity = botData.BotStats.LedgeJumpForce; 
                Physics.gravity = botData.BotStats.FallingGForce;
                botData.BotDetectionStats.IsLedge = false;
                botData.BotStats.IsInLedgeClimbing = true;
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
            if (botInput.Jump.action.triggered && !botData.BotStats.IsWallJump)
            {
                Physics.gravity = botData.BotStats.WallGForce;
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
            isOnLedge = false;
            botAnimatorController.Animator.SetBool(WallSlide, false);

        }

        public BotClimbState(BotStateMachine currentContext, BotMovement botMovement, BotInput botInput,
            BotData botData, BotAnimatorController botAnimatorController) : base(currentContext, botMovement, botInput,
            botData, botAnimatorController)
        {
        }
    }
}