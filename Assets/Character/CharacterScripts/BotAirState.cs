using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotAirState : BotBaseState, IDash
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
        private static readonly int AirDash = Animator.StringToHash("AirDash");
       // private static readonly int NearGround = Animator.StringToHash("NearGround");

        public override void EnterState()
        {
            botData.BotDetectionStats.IsWall = false;
            Physics.gravity = botData.BotStats.FallingGForce;
            
            cancelJumpAnimation = false;
            cancelFallingAfterGliding = false;
            hasJumped = false;
            inFalling = false;
            botData.BotStats.IsGliding = false; //ss
            hasGlided = false;
            botData.BotStats.CanAirDashAnimation = true;
            cancelFallingAfterDash = true;
            cancelFallingAfterLanding = false;

            botData.BotStats.HasAirDashed = false;
            //ss
            botData.BotStats.CanAirDash = true;
        }

        public override void UpdateState()
        {
            HandleMovementSpeed();
            HandleGliding();
            HandleJumpAnimation();
            HandleFallAnimation();
            HandleAirDashAnimation();
            HandleGlidingAnimation();
            HandleFallingGravity();
            HandleLanding();
            WallJumpTimerHandler();
            HandleLedgeJumpTimer();
            botData.BotDetection.IsNearOnGround();
            IsInDashDistanceRange();
            HandleDashAction();
            HandleDashTimer();


        }
        public override void FixedUpdate()
        {
            StartDash();
            if (botData.BotStats.IsInLedgeClimbing) return;
            if(botData.BotStats.IsWallJump) return;
            if(botData.BotStats.IsAirDashing) return;
            botMovement.MoveHorizontally(botData.BotStats.CurrentSpeed);
        }

        private void HandleMovementSpeed()
        {
            if (botData.BotStats.IsInLedgeClimbing) return; 
            if(botData.BotStats.IsGroundDashing) return;
            if (botData.BotStats.MoveDirection.x != 0)
            {
                
                if (botInput.Run.action.IsPressed())
                {
                    if (Math.Abs(botData.BotStats.CurrentSpeed - botData.BotStats.MaxSpeed) > 0.0001f)
                    {
                        botData.BotStats.CurrentSpeed = botData.BotStats.WalkSpeed;
                    }
                    //wasashlelia
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

        private void WallJumpTimerHandler()
        {
            if (botData.BotStats.IsWallJump && botData.BotStats.WallJumpDurationStart)
            {
                startTimeOfWallJump = Time.time;
                botData.BotStats.WallJumpDurationStart = false;
            }
            else if (!botData.BotStats.IsWallJump)
            {
                return;
            }

            durationOfWallJump = Time.time - startTimeOfWallJump;
            if (durationOfWallJump >= 0.2f)
            {
                botData.BotStats.IsWallJump = false;
            }
        }
        
        private void HandleJumpAnimation()
        {
            if (botData.BotStats.IsJump && botData.BotComponents.Rb.velocity.y > 0f && !hasJumped  || botData.BotComponents.Rb.velocity.y > 0f )
            {
                hasJumped = true;
                botAnimatorController.Animator.SetBool(GroundJump, true);
            }
            else if(hasJumped && !cancelJumpAnimation && botData.BotComponents.Rb.velocity.y <= 0 || botData.BotDetectionStats.IsWall)
            {
                botData.BotDetectionStats.WallDetectionRadius = 0.3f;
                botData.BotStats.IsJump = false;
                cancelJumpAnimation = true;
                botAnimatorController.Animator.SetBool(GroundJump, false);
            }
        }
        
        private void HandleFallAnimation()
        {
            if (botData.BotStats.IsInLedgeClimbing) return;
            if(botData.BotDetectionStats.IsNearOnGround) return;
            if (botData.BotComponents.Rb.velocity.y < 0 && !inFalling && !botData.BotStats.IsGliding && !botData.BotStats.IsWallJump)
            {
                inFalling = true;
                
                cancelFallingAfterLanding = false;
                botAnimatorController.Animator.SetBool(Falling, true);
            }
            else if (!cancelFallingAfterDash && botData.BotStats.IsAirDashing || !cancelFallingAfterGliding && botData.BotStats.IsGliding)
            {
                if (!cancelFallingAfterDash) cancelFallingAfterDash = true;
                if (!cancelFallingAfterGliding) cancelFallingAfterGliding = true;
                botAnimatorController.Animator.SetBool(Falling, false);
            }
        }
        
        private void HandleGliding()
        {
            if (botData.BotStats.IsInLedgeClimbing) return;
            if (botInput.Jump.action.triggered && !botData.BotStats.IsAirDashing &&
                !botData.BotDetectionStats.IsNearOnGround && !botData.BotStats.IsWallJump)
            {
                botData.BotStats.StopGlide = false;
                botData.BotStats.IsGliding = true;
                Physics.gravity = botData.BotStats.GlidingGForce;
            }else if (botData.BotComponents.Rb.velocity.y > 0 && botData.BotStats.IsGliding || botData.BotStats.IsAirDashing)
            {
                botData.BotStats.IsGliding = false;
            }
        }
        

        private void HandleGlidingAnimation()
        {
            if (botData.BotStats.IsGliding && botInput.Jump.action.IsPressed() && !hasGlided && !botData.BotStats.IsAirDashing &&
                 !botData.BotDetectionStats.IsNearOnGround && !botData.BotStats.IsWallJump)
            {
                hasGlided = true;
                botData.BotComponents.Rb.velocity = Vector3.zero;
                botAnimatorController.Animator.SetBool(Gliding, true);
                botData.BotDetection.GlideObj.SetActive(true);
            }
        
            if (botData.BotDetectionStats.IsNearOnGround && !cancelFallingAfterLanding &&
                botData.BotComponents.Rb.velocity.y < 0f || !cancelGlidingAfterDash && botData.BotStats.IsAirDashing ||
                botData.BotStats.IsGliding && !botInput.Jump.action.IsPressed() || botData.BotStats.StopGlide)
            {
                if (!cancelGlidingAfterDash) cancelGlidingAfterDash = true;
                if (!cancelFallingAfterLanding) cancelFallingAfterLanding = true;
                if (inFalling && botData.BotStats.IsGliding)
                {
                    inFalling = false;
                }
                botData.BotStats.IsGliding = false;
                hasGlided = false;
                cancelFallingAfterGliding = false;
                botAnimatorController.Animator.SetBool(Gliding, false);
                botData.BotDetection.GlideObj.SetActive(false);
            }
        }
        
        private void HandleAirDashAnimation()
        {
            if (botData.BotStats.CanAirDashAnimation && botData.BotStats.IsAirDashing && !botData.BotStats.IsWallJump)
            {
                cancelFallingAfterDash = false;
                cancelDashAnimation = false;
                cancelGlidingAfterDash = false;
                cancelFallingAfterLanding = false;
                botData.BotStats.CanAirDashAnimation = false;
                botAnimatorController.Animator.SetBool(AirDash, true);
              
            }
            else if (!botData.BotStats.IsAirDashing && !cancelDashAnimation)
            {
                if (inFalling)
                {
                    inFalling = false;
                }
                cancelDashAnimation = true;
                botAnimatorController.Animator.SetBool(AirDash, false);
            }
        }
        
        private void HandleDashAction()
        {
            if (botData.BotStats.IsRotating) return;
            if(botData.BotStats.IsWallJump) return;
            if(botData.BotDetectionStats.IsWall) return;
            if(botData.BotDetectionStats.IsDistanceForDash) return;
            if (!botInput.Dash.action.triggered || !botData.BotStats.CanAirDash) return;
            botData.BotStats.IsAirDashing = true;
            botData.BotStats.AirDashCooldownStart = Time.time;
            Physics.gravity = Vector3.zero;
            botData.BotStats.CanAirDash = false;
        }

        public void StartDash()
        {
            if (botData.BotStats.IsAirDashing && !botData.BotStats.HasAirDashed)
            {
                var velocity = botData.BotComponents.Rb.velocity;
                velocity = botData.BotStats.CurrentDirectionValue switch
                {
                    1 => new Vector2(botData.BotStats.DashForce, 0),
                    -1 => new Vector2(-botData.BotStats.DashForce, 0),
                    _ => velocity
                };
                botData.BotComponents.Rb.velocity = velocity;
                botData.BotStats.HasAirDashed = true;
            }
        }

        private void HandleDashTimer()
        {
            if (botData.BotStats.IsAirDashing)
            {
                botData.BotStats.AirDashDuration = Time.time - botData.BotStats.AirDashCooldownStart;
                if (botData.BotStats.AirDashDuration >= botData.BotStats.DashLengthTimeInAir
                    || botData.BotStats.IsHurricaneBounce || botData.BotStats.IsMegaBounce)
                {
                    EndDash();
                }
            }
        }

        public void EndDash()
        {
            botData.BotStats.IsAirDashing = false;
            botData.BotStats.HasAirDashed = false;
            Physics.gravity = botData.BotStats.FallingGForce;
            botData.BotComponents.Rb.velocity = Vector3.zero;
        }

        private void HandleFallingGravity()
        {
            if (!botData.BotStats.IsGliding && !botData.BotStats.IsAirDashing && !botData.BotDetectionStats.IsNearOnGround)
            {
                Physics.gravity = botData.BotStats.FallingGForce;
            }
        }
        
        private void HandleLanding()
        {
            if (botData.BotDetectionStats.IsNearOnGround && botData.BotComponents.Rb.velocity.y <= 0 && !botData.BotStats.IsAirDashing)
            {
                Physics.gravity = botData.BotStats.FallingGForce;
            }
        }
        
        private void IsInDashDistanceRange()
        {
            botData.BotDetectionStats.IsDistanceForDash = Physics.CheckSphere(
                botData.BotDetection.GroundTransform.position, 0.5f, botData.BotDetectionStats.Grounded);
            // Vector3 bottom = botData.BotComponents.MoveCollider.bounds.center - new Vector3(0, botData.BotComponents.MoveCollider.bounds.extents.y, 0);
            //
            // // შემოწმება, რომ ბოტი მიწაზე ან პლატფორმაზე დგას
            // botData.BotDetectionStats.IsDistanceForDash = Physics.CheckSphere(
            //     bottom, 0.2f, botData.BotDetectionStats.Grounded | botData.BotDetectionStats.Platform);
        }

        private void HandleLedgeJumpTimer()
        {
            if (botData.BotStats.IsInLedgeClimbing)
            {
                botData.BotStats.DurationOfLedgeClimbing = Time.time - botData.BotStats.LedgeClimbingStartTime;
                if (botData.BotStats.DurationOfLedgeClimbing >= 0.2f)
                {
                    botData.BotStats.IsInLedgeClimbing = false;
                    //botData.BotComponents.MoveCollider.isTrigger = false;
                    botData.BotStats.LedgeClimbingStartTime = 0f;
                    botData.BotStats.DurationOfLedgeClimbing = 0f;
                }
            }
        }
        public override void ExitState()
        {
            botAnimatorController.Animator.SetBool(Gliding, false);
            botAnimatorController.Animator.SetBool(Falling, false);
            botAnimatorController.Animator.SetBool(GroundJump, false);
            botAnimatorController.Animator.SetBool(AirDash, false);
            
            botData.BotStats.IsJump = false;
            botData.BotDetectionStats.IsNearOnGround = false;

            botData.BotStats.IsInLedgeClimbing = false;
            botData.BotDetectionStats.IsDistanceForDash = false;
            
            botData.BotStats.IsAirDashing = false;
            // botAnimatorController.Animator.SetBool(NearGround,false);
            
            botData.BotDetection.GlideObj.SetActive(false);

        }

        public BotAirState(BotStateMachine currentContext, BotMovement botMovement, BotInput botInput, BotData botData,
            BotAnimatorController botAnimatorController) : base(currentContext, botMovement, botInput,
            botData, botAnimatorController)
        {
        }
    }
}