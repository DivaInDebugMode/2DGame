using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotAirState : BotBaseState
    {
        [Header("Jump Variables")] private bool hasJumped;
        private bool cancelJumpAnimation;
        private static readonly int GroundJump = Animator.StringToHash("GroundJump");

        [Header("Wall Jump Variables")] private bool wasWallJumped;
        private float durationOfWallJump;
        private float startTimeOfWallJump;
        
        [Header("Falling Variables")] private bool inFalling;
        private bool cancelFallingAfterDash;
        private bool cancelFallingAfterGliding;
        private bool cancelFallingAfterLanding;
        private static readonly int Falling = Animator.StringToHash("Falling");

        [Header("Gliding Variables")] private bool inGliding;
        private bool hasGlided;
        private bool cancelGlidingAfterDash;
        private static readonly int Gliding = Animator.StringToHash("Gliding");

        [Header("AirDash Variables")] private bool cancelDashAnimation;
        private bool canDash;
        private bool canDashAnimation;
        private static readonly int AirDash = Animator.StringToHash("AirDash");


        public override void EnterState()
        {
            botData.BotDetectionStats.IsWall = false;
            Physics.gravity = botData.BotStats.FallingGForce;
            
            botData.BotStats.DashDuration = botData.BotStats.DashDurationAir;
            cancelJumpAnimation = false;
            cancelFallingAfterGliding = false;
            hasJumped = false;
            inFalling = false;
            inGliding = false;
            hasGlided = false;
            canDash = true;
            canDashAnimation = true;
            cancelFallingAfterDash = true;
            cancelFallingAfterLanding = false;

        }

        public override void UpdateState()
        {
            HandleMovementSpeed();
            
            HandleAirDash();
            
            HandleGliding();
            
            HandleJumpAnimation();
            
            HandleFallAnimation();
            
            HandleAirDashAnimation();
            
            HandleGlidingAnimation();
            
            HandleFallingGravity();

            HandleLanding();
            
            botData.BotDetection.IsNearOnGround();

            if (botData.BotStats.IsWallJump && botData.BotStats.WallJumpDurationStart)
            {
                startTimeOfWallJump = Time.time;
                botData.BotStats.WallJumpDurationStart = false;
            }

            if (botData.BotStats.IsWallJump)
            {
                durationOfWallJump = Time.time - startTimeOfWallJump;
                if (durationOfWallJump >= 0.4f)
                {
                    botData.BotStats.IsWallJump = false;
                }
            }
            
            if (botData.BotStats.IsInLedgeClimbing)
            {
                botData.BotStats.DurationOfLedgeClimbing = Time.time - botData.BotStats.LedgeClimbingStartTime;
                if (botData.BotStats.DurationOfLedgeClimbing >= 0.3f)
                {
                    botData.BotStats.IsInLedgeClimbing = false;
                    botData.BotComponents.MoveCollider.isTrigger = false;
                    botData.BotStats.LedgeClimbingStartTime = 0f;
                    botData.BotStats.DurationOfLedgeClimbing = 0f;
                    Debug.Log("gavedi");
                    
                }
            }
        }
        public override void FixedUpdate()
        {
            if (botData.BotStats.IsInLedgeClimbing) return;//aq
            if(botData.BotStats.IsWallJump) return;
            botMovement.MoveHorizontally(botData.BotStats.CurrentSpeed);
        }

        private void HandleMovementSpeed()
        {
            if (botData.BotStats.IsInLedgeClimbing) return; // aq
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

        private void HandleJumpAnimation()
        {
            if (botData.BotStats.IsJump && botData.BotComponents.Rb.velocity.y > 0f && !hasJumped)
            {
                hasJumped = true;
                botAnimatorController.Animator.SetBool(GroundJump, true);
            }
            else if(hasJumped && !cancelJumpAnimation && botData.BotComponents.Rb.velocity.y <= 0)
            {
                botData.BotDetectionStats.WallDetectionRadius = 0.5f;
                botData.BotStats.IsJump = false;
                cancelJumpAnimation = true;
                botAnimatorController.Animator.SetBool(GroundJump, false);
            }
        }
        
        private void HandleFallAnimation()
        {
            if (botData.BotStats.IsInLedgeClimbing) return;
            if (botData.BotComponents.Rb.velocity.y < 0 && !inFalling && !inGliding && !botData.BotStats.IsWallJump)
            {
                inFalling = true;
                
                cancelFallingAfterLanding = false;
                botAnimatorController.Animator.SetBool(Falling, true);
            }
            else if (!cancelFallingAfterDash && botData.BotStats.IsDashing || !cancelFallingAfterGliding && inGliding)
            {
                if (!cancelFallingAfterDash) cancelFallingAfterDash = true;
                if (!cancelFallingAfterGliding) cancelFallingAfterGliding = true;
                botAnimatorController.Animator.SetBool(Falling, false);
            }
        }
        
        private void HandleGliding()
        {
            if (botData.BotStats.IsInLedgeClimbing) return;
            if (botInput.Jump.action.IsPressed() && !botData.BotStats.IsDashing && 
                botData.BotComponents.Rb.velocity.y <= 0 && !botData.BotDetectionStats.IsNearOnGround && !botData.BotStats.IsWallJump)
            {
                inGliding = true;
                Physics.gravity = botData.BotStats.GlidingGForce;
            }
        }
        private void HandleGlidingAnimation()
        {
            
            if (inGliding && botInput.Jump.action.IsPressed() && !hasGlided && !botData.BotStats.IsDashing &&
                botData.BotComponents.Rb.velocity.y <= 0 && !botData.BotDetectionStats.IsNearOnGround && !botData.BotStats.IsWallJump)
            {
                hasGlided = true;
                botAnimatorController.Animator.SetBool(Gliding, true);
            }
        
            if (botData.BotDetectionStats.IsNearOnGround && !cancelFallingAfterLanding &&
                botData.BotComponents.Rb.velocity.y < 0f || !cancelGlidingAfterDash && botData.BotStats.IsDashing ||
                inGliding && !botInput.Jump.action.IsPressed())
            {
                if (!cancelGlidingAfterDash) cancelGlidingAfterDash = true;
                if (!cancelFallingAfterLanding) cancelFallingAfterLanding = true;
                if (inFalling && inGliding)
                {
                    inFalling = false;
                }
                inGliding = false;
                hasGlided = false;
                cancelFallingAfterGliding = false;
                botAnimatorController.Animator.SetBool(Gliding, false);
            }
        }
        
        private void HandleAirDash()
        {
            if (!botInput.Jump.action.IsPressed() && canDash && botData.BotStats.IsDashing &&
                !botData.BotStats.IsJump && !botData.BotDetectionStats.IsNearOnGround && !botData.BotStats.IsWallJump)
            {
                Physics.gravity = Vector3.zero;
                canDash = false;
                botData.BotStats.CanAirDash = false;
                botDash.Dash();
            }
        }

        private void HandleAirDashAnimation()
        {
            if (!botInput.Jump.action.IsPressed() && canDashAnimation && botData.BotStats.IsDashing &&
                botData.BotStats.HasDashed && !botData.BotStats.IsJump && !botData.BotDetectionStats.IsNearOnGround && !botData.BotStats.IsWallJump)
            {
                
                cancelFallingAfterDash = false;
                cancelDashAnimation = false;
                cancelGlidingAfterDash = false;
                cancelFallingAfterLanding = false;

                canDashAnimation = false;
                botData.BotStats.HasDashed = false;
                botAnimatorController.Animator.SetBool(AirDash, true);
            }
            else if (!botData.BotStats.IsDashing && !cancelDashAnimation)
            {
                if (inFalling)
                {
                    inFalling = false;
                }
                cancelDashAnimation = true;
                botAnimatorController.Animator.SetBool(AirDash, false);
            }
        }

        private void HandleFallingGravity()
        {
            if (!inGliding && !botData.BotStats.IsDashing && !botData.BotDetectionStats.IsNearOnGround)
            {
                Physics.gravity = botData.BotStats.FallingGForce;
            }
        }

        private void HandleLanding()
        {
            if (botData.BotDetectionStats.IsNearOnGround && botData.BotComponents.Rb.velocity.y <= 0)
            {
                Physics.gravity = botData.BotStats.FallingGForce;
            }
        }

        public override void ExitState()
        {
            botAnimatorController.Animator.SetBool(Gliding, false);
            botAnimatorController.Animator.SetBool(Falling, false);
            botAnimatorController.Animator.SetBool(GroundJump, false);
            
            botData.BotStats.IsJump = false;
            botData.BotDetectionStats.IsNearOnGround = false;

            //ss
            botData.BotStats.CanAirDash = true;

        }

        public BotAirState(BotStateMachine currentContext, BotMovement botMovement, BotInput botInput, BotData botData, BotAnimatorController botAnimatorController, BotDash botDash) : base(currentContext, botMovement, botInput, botData, botAnimatorController, botDash)
        {
        }
    }
}