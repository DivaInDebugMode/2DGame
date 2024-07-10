using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotGroundedState : BotBaseState
    {
        private float crouchTimer;
        private const float CrouchDuration = 0.25f;

        private bool dashAnimatorReset;
        private static readonly int Velocity = Animator.StringToHash("Velocity");
        private static readonly int XDirection = Animator.StringToHash("XDirection");
        private static readonly int Crouch1 = Animator.StringToHash("Crouch");
        private static readonly int CrouchToStand = Animator.StringToHash("CrouchToStand");
        private static readonly int Dash = Animator.StringToHash("Dash");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int WallSlide = Animator.StringToHash("Wall Slide");

        private bool jumpTimerOn;
        private float jumpStartTimer;
        private float jumpTimerDuration;

        public override void EnterState()
        {
            botData.BotStats.DashDuration = botData.BotStats.DashDurationGround;
            Physics.gravity = botData.BotStats.GroundGForce;
            botAnimatorController.Animator.SetBool(WallSlide, false);
            jumpStartTimer = Time.time;
            jumpTimerOn = true;
        }

        public override void UpdateState()
        {
            HandleMovementSpeed();
            HandleMovementAnimation();
            SetCurrentDirectionValue();
            Crouch();
            HandleDashAnimation();
            DisableDashAnimation();
            HandleGroundedAnimation();
            HandleTimer();
        }

        private void HandleTimer()
        {
            if (jumpTimerOn)
            {
                jumpTimerDuration = Time.time - jumpStartTimer;
                if (jumpTimerDuration >= 0.06f)
                {
                    botData.BotStats.HasJumped = false;
                    jumpTimerOn = false;
                }
            }
        }

        public override void FixedUpdate()
        {
            botMovement.MoveHorizontally(botData.BotStats.CurrentSpeed);
            if (botData.BotStats.IsDashing && !botData.BotDetectionStats.IsNearOnGround)
            {
                botDash.Dash();
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
            if(botData.BotStats.IsDashing) return;
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
            if (!botData.BotStats.IsRotating && botInput.MoveDown.action.triggered && !botData.BotStats.IsCrouching
                && !botData.BotStats.IsDashing && crouchTimer <= 0)
            {
                botData.BotComponents.Rb.velocity = new Vector2(0, botData.BotComponents.Rb.velocity.y);
                botData.BotComponents.Coll.enabled = false;
                botData.BotComponents.CrouchCollider.enabled = true;
                botData.BotStats.CurrentSpeed = 0;
                botData.BotStats.DirectionTime = 0;
                botAnimatorController.Animator.SetTrigger(Crouch1);
                botData.BotStats.HasCrouched = false;
                botData.BotStats.IsCrouching = true;
                crouchTimer = CrouchDuration;
            }
            else if (!botInput.MoveDown.action.IsPressed() && botData.BotStats.IsCrouching && !botData.BotStats.HasCrouched && crouchTimer <= 0f)
            {
                botData.BotComponents.CrouchCollider.enabled = false;
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
            botAnimatorController.Animator.SetBool(Dash, true);
            botData.BotStats.HasDashed = false;
            dashAnimatorReset = false;
        }

        private void DisableDashAnimation()
        {
            if (!botData.BotStats.IsDashing && !dashAnimatorReset)
            {
                botAnimatorController.Animator.SetBool(Dash, false);
                dashAnimatorReset = true;
            }
        }

        private void HandleGroundedAnimation()
        {
            if (!botData.BotStats.IsDashing)
            {
                botAnimatorController.Animator.SetBool(Grounded, true);
            }
        }
        
        public override void ExitState()
        {
            botAnimatorController.Animator.SetBool(Grounded, false);
            botAnimatorController.Animator.SetBool(Dash, false);
        }


        public BotGroundedState(BotStateMachine currentContext, BotMovement botMovement, BotInput botInput, BotData botData, BotAnimatorController botAnimatorController, BotDash botDash) : base(currentContext, botMovement, botInput, botData, botAnimatorController, botDash)
        {
        }
    }
}