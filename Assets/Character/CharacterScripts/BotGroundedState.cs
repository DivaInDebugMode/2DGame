using System;
using System.Collections;
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


        //[Header("Jump Timer Variables for enable jump action")]
        // private bool jumpTimerOn;
        // private float jumpStartTimer;
        // private float jumpTimerDuration;
        private static readonly int GroundAttack1 = Animator.StringToHash("GroundAttack1");
        private static readonly int GroundAttack2 = Animator.StringToHash("GroundAttack2");

        public override void EnterState()
        {
            botData.Animator.SetBool(Grounded, true);
            botData.BotDetectionStats.IsWall = false;
            Physics.gravity = botData.BotStats.GroundGForce;
            botData.Rb.velocity = new Vector3(0, botData.Rb.velocity.y);
            inMove = false;
           // jumpStartTimer = Time.time;
           // jumpTimerOn = true;
            botData.BotStats.IsWallJump = false;
            botData.BotStats.GroundDashTimer = 0f;
            botData.BotStats.CanGroundDash = true;
            botData.BotStats.HasGroundDashed = false;
            
            
            botData.BotStats.HasJumped = false; //es davamate imitoro jump reseti davakomentare
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
          //  JumpActionResetTimer();
            HandleDashTimer();
            HandleDashAction();
           //AttackAnimation();

        }
        
        public override void FixedUpdate()
        {
            if (!botData.BotStats.IsGroundDashing)
            {
                botMovement.MoveHorizontally(botData.BotStats.CurrentSpeed);
            }
            
            StartDash();
        }
        private void HandleMovementAnimation()
        {
            if (botData.BotStats.IsGroundDashing) return;
            botData.Animator.SetFloat(XDirection,botData.BotStats.CurrentDirectionValue);
            botData.Animator.SetFloat(Velocity,botData.BotStats.CurrentSpeed);
        }
        
        private void HandleMovementSpeed()
        {
            if(botData.BotStats.IsCrouching || botData.BotStats.IsGroundDashing || botData.BotStats.IsAttacking) return;
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
                botData.BotStats.CurrentSpeed = 0;
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
                botData.Rb.velocity = Vector3.zero;
                botData.Animator.SetTrigger(Crouch1);
                botData.BotStats.HasCrouched = false;
                botData.BotStats.IsCrouching = true;
                crouchTimer = CrouchDuration;
            }
            else if (!botInput.MoveDown.action.IsPressed() && botData.BotStats.IsCrouching &&
                     !botData.BotStats.HasCrouched && crouchTimer <= 0f)
            {
                botData.Animator.SetTrigger(CrouchToStand);
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
        // private void JumpActionResetTimer()
        // {
        //     if (jumpTimerOn)
        //     {
        //         jumpTimerDuration = Time.time - jumpStartTimer;
        //         if (jumpTimerDuration >= 0.0f)
        //         {
        //             botData.BotStats.HasJumped = false;
        //             jumpTimerOn = false;
        //         }
        //     }
        // }
        private void HandleDashAnimation()
        {
            if (botData.BotStats.IsGroundDashing && dashAnimatorReset)
            {
                botData.Animator.SetBool(Dash, true);
                
                dashAnimatorReset = false;
            }
            else if (!botData.BotStats.IsGroundDashing && !dashAnimatorReset)
            {
                botData.Animator.SetBool(Dash, false);
                
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
                if (botData.BotStats.GroundDashDuration >= botData.BotStats.DashLengthTimeInGround)
                {
                    EndDash();
                }
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
                var velocity = botData.Rb.velocity;
                velocity = botData.BotStats.CurrentDirectionValue switch
                {
                    1 => new Vector2(botData.BotStats.DashForce, 0),
                    -1 => new Vector2(-botData.BotStats.DashForce, 0),
                    _ => velocity
                };
                botData.Rb.velocity = velocity;
                botData.BotStats.HasGroundDashed = true;
            }
        }

        public void EndDash()
        {
            botData.BotStats.IsGroundDashing = false;

            //botData.BotStats.DirectionTime = 0;
            if (botData.BotStats.MoveDirection.x == 0)
            {
                botData.Rb.velocity = Vector3.zero;
            }
            else
            {
                botData.Rb.velocity = new Vector2(botData.Rb.velocity.x, 0);
            }
        }


        private void HandleGroundedAnimation()
        {
            if (!botData.BotStats.IsGroundDashing && !inMove)
            {
                inMove = true;
                botData.Animator.SetBool(Grounded, true);
            }
            else if (botData.BotStats.IsGroundDashing && inMove)
            {
                inMove = false;
                botData.Animator.SetBool(Grounded, false);
            }
        }

        // private void AttackAnimation()
        // {
        //     if (botInput.Attack.action.triggered && !botData.BotStats.IsAttacking && !botData.BotStats.IsGroundDashing)
        //     {
        //         botData.BotStats.IsAttacking = true;
        //         botData.BotStats.CurrentSpeed = 0f;
        //
        //         botData.Animator.SetBool(GroundAttack1, botData.BotStats.IsAttacking);
        //         ctx.StartCoroutine(FinishAttack1());
        //
        //     }
        // }
        //
        // private bool attack1Finished;
        // private IEnumerator FinishAttack1()
        // {
        //     yield return new WaitForSecondsRealtime(0.55f);
        //     
        //     botData.BotStats.IsAttacking = false;
        //     botData.Animator.SetBool(GroundAttack1, botData.BotStats.IsAttacking);
        // }
        
        public override void ExitState()
        {
            botData.BotStats.GroundDashTimer = 0;
            botData.BotStats.HasGroundDashed = false;
            botData.BotStats.IsGroundDashing = false;
            botData.Animator.SetBool(Grounded, false);
            botData.Animator.SetBool(Dash, false);
        }
        
        public BotGroundedState(BotStateMachine currentContext, BotMovement botMovement, BotInput botInput,
            BotData botData) : base(currentContext,
            botMovement, botInput, botData)
        {
        
        }
       
    }
}