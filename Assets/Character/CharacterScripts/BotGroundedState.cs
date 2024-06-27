using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotGroundedState : BotBaseState
    {
        private bool isOnEdge;
        private bool isEdgeOff;
        private float crouchTimer;
        private const float CrouchDuration = 0.3f;
        private float resetJumpTimer;
        private bool resetJumpTimerActive;
        private float resetDashTimer;
        private bool resetDashTimerActive;
        private static readonly int Velocity = Animator.StringToHash("Velocity");
        private static readonly int XDirection = Animator.StringToHash("XDirection");
        private static readonly int Crouch1 = Animator.StringToHash("Crouch");
        private static readonly int CrouchToStand = Animator.StringToHash("CrouchToStand");
        private static readonly int Dash = Animator.StringToHash("Dash");
        private static readonly int Landing = Animator.StringToHash("Landing");
        private static readonly int Falling = Animator.StringToHash("Falling");

        public override void EnterState()
        {
            botData.BotComponents.Rb.velocity = Vector3.zero;
            botAnimatorController.Animator.SetBool("Wall Slide", false);
            Physics.gravity = new Vector2(0, -9.81f);
            botData.BotStats.IsJump = false;
            botData.BotStats.CanDash = false;
            botData.BotStats.DashDuration = 0.7f;
            ctx.test = false;
            resetJumpTimer = 0.2f;
            resetJumpTimerActive = true;
            resetDashTimer = 0.09f;
            resetDashTimerActive = true;
        }

        public override void UpdateState()
        {
            HandleTimers();
            HandleMovementSpeed();
            HandleMovementAnimation();
            SetCurrentDirectionValue();
            Crouch();
            HandleDashAnimation();
            HandleLanding();
        }

        private void HandleTimers()
        {
            if (resetJumpTimerActive)
            {
                resetJumpTimer -= Time.deltaTime;
                if (resetJumpTimer <= 0f)
                {
                    botData.BotStats.NumberOfJump = 0;
                    resetJumpTimerActive = false;
                }
            }

            if (resetDashTimerActive)
            {
                resetDashTimer -= Time.deltaTime;
                if (resetDashTimer <= 0f)
                {
                    botData.BotStats.CanDash = true;
                    resetDashTimerActive = false;
                }
            }
            
        }
        public override void FixedUpdate()
        {
            botMovement.MoveHorizontally(botData.BotStats.CurrentSpeed);
            if (botData.BotStats.HasJumped)
            {
                botData.BotComponents.Rb.velocity =
                    new Vector2(botData.BotComponents.Rb.velocity.x, Mathf.Max(botData.BotStats.JumpForce, botData.BotStats.InitialJumpForce));
                botData.BotStats.HasJumped = false;
            }
        }


        private void HandleMovementAnimation()
        {
            if (botData.BotStats.IsDashing) return;
            botAnimatorController.Animator.SetFloat(XDirection,botData.BotStats.CurrentDirectionValue);
            botAnimatorController.Animator.SetFloat(Velocity,botData.BotStats.CurrentSpeed);
        }
        
        private void HandleMovementSpeed()
        {
            if(botData.BotStats.IsCrouching || botData.BotStats.IsDashing) return;
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

        private void SetCurrentDirectionValue()
        {
            if(botData.BotStats.IsCrouching || botData.BotStats.IsDashing) return;
            switch (botData.BotStats.CurrentDirectionValue)
            {
                case 1 when botInput.Run.action.IsPressed() && botData.BotStats.MoveDirection.x != 0:
                case -1 when botInput.Run.action.IsPressed() && botData.BotStats.MoveDirection.x != 0:
                    botData.BotStats.DirectionTime += Time.deltaTime;
                    break;
            }
        }

        private void Crouch()
        {
            if (botInput.MoveDown.action.triggered && !botData.BotStats.IsCrouching && !botData.BotStats.IsDashing && !botData.BotStats.IsJump && crouchTimer <= 0)
            {
                botData.BotComponents.Rb.velocity = new Vector2(0, botData.BotComponents.Rb.velocity.y);
                botData.BotComponents.Coll.enabled = false;
                botData.BotComponents.BoxCollider.enabled = true;
                botData.BotStats.CurrentSpeed = 0;
                botData.BotStats.DirectionTime = 0;
                botAnimatorController.Animator.SetTrigger(Crouch1);
                botData.BotStats.HasCrouched = false;
                botData.BotStats.IsCrouching = true;
                crouchTimer = CrouchDuration;
            }
            else if (!botInput.MoveDown.action.IsPressed() && botData.BotStats.IsCrouching && !botData.BotStats.HasCrouched && crouchTimer <= 0f)
            {
                botData.BotComponents.BoxCollider.enabled = false;
                botData.BotComponents.Coll.enabled = true;
                botAnimatorController.Animator.SetTrigger(CrouchToStand);
                botData.BotStats.IsCrouching = false;
                botData.BotStats.HasCrouched = true;
                crouchTimer = CrouchDuration;
            }

            if (crouchTimer > 0f)
            {
                crouchTimer -= Time.deltaTime;
                if (crouchTimer <= 0f)
                {
                    crouchTimer = 0;
                }
            }
        }

        private void HandleDashAnimation()
        {
            if (!botData.BotStats.IsDashing || !botData.BotStats.HasDashed) return;
            botAnimatorController.Animator.SetTrigger(Dash);
            botData.BotStats.HasDashed = false;
        }

        private void HandleLanding()
        {
            if (botData.BotStats.IsFalling && !botData.BotStats.IsDashing)
            {
                botAnimatorController.Animator.SetTrigger(Landing);
                botAnimatorController.Animator.SetBool(Falling, false);
                botData.BotStats.IsFalling = false;
            }
        }
        
        public override void ExitState()
        {
            botAnimatorController.Animator.ResetTrigger(Landing);
        }
        
        public BotGroundedState(BotStateMachine currentContext, BotMovement botMovement, BotInput botInput, BotData botData, BotAnimatorController botAnimatorController, BotJump botJump) : base(currentContext, botMovement, botInput, botData, botAnimatorController, botJump)
        {
        }
    }
}