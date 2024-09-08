using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotGroundedState : BotBaseState, IDash
    {
        [Header("Move Variables")] private bool inMove;
        private static readonly int Velocity = Animator.StringToHash("Velocity");
        private static readonly int XDirection = Animator.StringToHash("XDirection");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        
        [Header("Crouch Variables")]
        private float crouchTimer;
        private const float CrouchDuration = 0.25f;
        private static readonly int Crouch1 = Animator.StringToHash("Crouch");

        private static readonly int CrouchToStand = Animator.StringToHash("CrouchToStand");


        [Header("Dash Variables")]
        private bool dashAnimatorReset;
        private static readonly int Dash = Animator.StringToHash("Dash");


        [Header("Jump Timer Variables for enable jump action")]
        private bool jumpTimerOn;
        private float jumpStartTimer;
        private float jumpTimerDuration;

        public override void EnterState()
        {
            botData.BotDetectionStats.IsWall = false;
            Physics.gravity = botData.BotStats.GroundGForce;
            botData.BotComponents.Rb.velocity = Vector3.zero;
            inMove = false;
            jumpStartTimer = Time.time;
            jumpTimerOn = true;
            botData.BotStats.IsWallJump = false;
            botData.BotStats.CurrentSpeed = 0;
            botData.BotStats.GroundDashTimer = 0f;
            botData.BotStats.CanGroundDash = true;
            botData.BotStats.HasGroundDashed = false;
        }

        public override void UpdateState()
        {
            SetCurrentDirectionValue();
            HandleMovementSpeed();
            HandleMovementAnimation();
            HandleDashAnimation();
            HandleGroundedAnimation();
            Crouch();
            CrouchActionResetTimer();
            JumpActionResetTimer();
            HandleDashTimer();
            HandleDashAction();
        }
        
        public override void FixedUpdate()
        {
            if (!botData.BotStats.IsGroundDashing)
            {
                botMovement.MoveHorizontally(botData.BotStats.CurrentSpeed);
            }
            
            StartDash();

            LedgeSlide();
        }
        private void HandleMovementAnimation()
        {
            if (botData.BotStats.IsGroundDashing) return;
            botAnimatorController.Animator.SetFloat(XDirection,botData.BotStats.CurrentDirectionValue);
            botAnimatorController.Animator.SetFloat(Velocity,botData.BotStats.CurrentSpeed);
        }
        
        private void HandleMovementSpeed()
        {
            if(botData.BotStats.IsCrouching || botData.BotStats.IsGroundDashing) return;
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
            if(botData.BotStats.IsGroundDashing) return;
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
            if (!botData.BotStats.IsRotating && !botData.BotStats.IsJump &&
                botInput.MoveDown.action.triggered && !botData.BotStats.IsCrouching
                && !botData.BotStats.IsGroundDashing && crouchTimer <= 0)
            {
                botData.BotComponents.Rb.velocity = Vector3.zero;
                botAnimatorController.Animator.SetTrigger(Crouch1);
                botData.BotStats.HasCrouched = false;
                botData.BotStats.IsCrouching = true;
                crouchTimer = CrouchDuration;
            }
            else if (!botInput.MoveDown.action.IsPressed() && botData.BotStats.IsCrouching &&
                     !botData.BotStats.HasCrouched && crouchTimer <= 0f)
            {
                botAnimatorController.Animator.SetTrigger(CrouchToStand);
                botData.BotStats.IsCrouching = false;
                botData.BotStats.HasCrouched = true;
                crouchTimer = CrouchDuration;
            }
        }

        private void CrouchActionResetTimer()
        {
            if (!(crouchTimer > 0f)) return;
            crouchTimer -= Time.deltaTime;
            if (crouchTimer <= 0f) crouchTimer = 0;
        }
        private void JumpActionResetTimer()
        {
            if (jumpTimerOn)
            {
                jumpTimerDuration = Time.time - jumpStartTimer;
                if (jumpTimerDuration >= 0.1f)
                {
                    botData.BotStats.HasJumped = false;
                    jumpTimerOn = false;
                }
            }
        }
        private void HandleDashAnimation()
        {
            if (botData.BotStats.IsGroundDashing && dashAnimatorReset)
            {
                botAnimatorController.Animator.SetBool(Dash, true);
                dashAnimatorReset = false;
            }
            else if (!botData.BotStats.IsGroundDashing && !dashAnimatorReset)
            {
                botAnimatorController.Animator.SetBool(Dash, false);
                dashAnimatorReset = true;
            }
        }

        private void HandleDashAction()
        {
            if (botData.BotStats.IsCrouching) return;
            if (botData.BotStats.IsRotating) return;
            if (botInput.Dash.action.triggered && botData.BotStats.CanGroundDash)
            {
                botData.BotStats.IsGroundDashing = true;
                botData.BotStats.GroundDashCooldownStart = Time.time;
                botData.BotStats.CanGroundDash = false;
            }
        }

        private void HandleDashTimer()
        {
            if (botData.BotStats.IsGroundDashing)
            {
                botData.BotStats.GroundDashDuration = Time.time - botData.BotStats.GroundDashCooldownStart;
                if (botData.BotStats.GroundDashDuration >= botData.BotStats.DashLengthTimeInGround) EndDash();
            }

            if (!botData.BotStats.CanGroundDash)
            {
                botData.BotStats.GroundDashTimer = Time.time - botData.BotStats.GroundDashCooldownStart;
                if (botData.BotStats.GroundDashTimer >= botData.BotStats.GroundDashCooldown)
                {
                    botData.BotStats.CanGroundDash = true;
                    botData.BotStats.HasGroundDashed = false;
                    botData.BotStats.GroundDashTimer = 0;
                }
            }
        }

        public void StartDash()
        {
            if (botData.BotStats.IsGroundDashing && !botData.BotStats.HasGroundDashed)
            {
                var velocity = botData.BotComponents.Rb.velocity;
                velocity = botData.BotStats.CurrentDirectionValue switch
                {
                    1 => new Vector2(botData.BotStats.DashForce, 0),
                    -1 => new Vector2(-botData.BotStats.DashForce, 0),
                    _ => velocity
                };
                botData.BotComponents.Rb.velocity = velocity;
                botData.BotStats.HasGroundDashed = true;
            }
        }

        public void EndDash()
        {
            botData.BotStats.IsGroundDashing = false;

           // botData.BotStats.DirectionTime = 0;
            botData.BotComponents.Rb.velocity = Vector3.zero;
        }


        private void HandleGroundedAnimation()
        {
            if (!botData.BotStats.IsGroundDashing && !inMove)
            {
                inMove = true;
                botAnimatorController.Animator.SetBool(Grounded, true);
            }
            else if (botData.BotStats.IsGroundDashing && inMove)
            {
                inMove = false;
                botAnimatorController.Animator.SetBool(Grounded, false);
            }
        }

        private void LedgeSlide()
        {
            if (!botData.BotDetectionStats.IsClimbingGround && botData.BotDetectionStats.IsGroundFrontFoot)
            {
                switch (botData.BotStats.CurrentDirectionValue)
                {
                    case 1:
                        if (botData.BotComponents.Rb.velocity == Vector3.zero)
                        {
                            botData.BotDetectionStats.IsOnEdge = true;
                            botData.BotComponents.Rb.velocity = new Vector2(-1.5f, -1.5f);
                            Debug.Log("haha");
                        }
                        break;
                    case -1:
                        if (botData.BotComponents.Rb.velocity == Vector3.zero)
                        {
                            botData.BotDetectionStats.IsOnEdge = true;
                            botData.BotComponents.Rb.velocity = new Vector2(1.5f, -1.5f);
                            Debug.Log("haha");
                        }
                        break;
                }
            }
        }
        
        public override void ExitState()
        {
            botData.BotStats.GroundDashTimer = 0;
            botData.BotStats.HasGroundDashed = false;
            botData.BotStats.IsGroundDashing = false;
            botAnimatorController.Animator.SetBool(Grounded, false);
            botAnimatorController.Animator.SetBool(Dash, false);
            botData.BotDetectionStats.IsOnEdge = false;
        }


        public BotGroundedState(BotStateMachine currentContext, BotMovement botMovement, BotInput botInput,
            BotData botData, BotAnimatorController botAnimatorController) : base(currentContext,
            botMovement, botInput, botData, botAnimatorController)
        {
        
        }
       
    }
}