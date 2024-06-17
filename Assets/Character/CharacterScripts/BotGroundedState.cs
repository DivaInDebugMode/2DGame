using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotGroundedState : BotBaseState
    {
        private bool isOnEdge;
        private bool isEdgeOff;
        private static readonly int Velocity = Animator.StringToHash("Velocity");
        private static readonly int Rotate = Animator.StringToHash("Rotate");
        private static readonly int RunRotate = Animator.StringToHash("RunRotate");
        private static readonly int XDirection = Animator.StringToHash("XDirection");
        private static readonly int Crouch1 = Animator.StringToHash("Crouch");
        private static readonly int CrouchToStand = Animator.StringToHash("CrouchToStand");
        private static readonly int Dash = Animator.StringToHash("Dash");

        public override void EnterState()
        {

        }

        public override void UpdateState()
        {
            HandleRotation();
            HandleMovementSpeed();
            HandleMovementAnimation();
            SetCurrentDirectionValue();
            Crouch();
            HandleDashAnimation();
            botData.BotDetection.CheckGroundLeftFoot();
            botData.BotDetection.CheckGroundRightFoot();
        }

        public override void FixedUpdate()
        {
            botMovement.MoveHorizontally(botData.BotStats.CurrentSpeed);
            
            if (botData.BotDetection.IsLeftFootOnEdge && botData.BotDetection.IsRightFootOnGround && !isOnEdge)
            {
                botData.BotStats.IsRotating = false;
                botData.BotStats.IsRunning = false;
                botData.BotStats.IsFallingEdge = true;
                isOnEdge = true;
                isEdgeOff = false;
            }
            else if(!isEdgeOff && !botData.BotDetection.IsLeftFootOnEdge)
            {
                botData.BotStats.IsFallingEdge = false;
                switch (botData.BotStats.CurrentDirection)
                {
                    case Directions.Left:
                        var transform = botData.transform;
                        var position = transform.position;
                        position = new Vector3(position.x - 0.4f,position.y,position.z);
                        transform.position = position;
                        break;
                    case Directions.Right:
                        var transform1 = botData.transform;
                        var position1 = transform1.position;
                        position1 = new Vector3(position1.x + 0.4f,position1.y,position1.z);
                        transform1.position = position1;
                        break;
                }

                isEdgeOff = true;
                isOnEdge = false;
            }
        }


        private void HandleMovementAnimation()
        {
            if (botData.BotStats.IsRotating || botData.BotStats.IsDashing) return;
            botAnimatorController.Animator.SetFloat(XDirection,botData.BotStats.MoveDirection.x);
            botAnimatorController.Animator.SetFloat(Velocity,botData.BotStats.CurrentSpeed);
        }

        private void HandleRotation()
        {
            if (!botData.BotStats.IsRotating || botData.BotStats.HasRotate) return;
            switch (botData.BotStats.DirectionTime)
            {
                case >= 0.5f:
                    botAnimatorController.Animator.SetTrigger(RunRotate);
                    botData.BotStats.DirectionTime = 0f;
                    break;
                case < 0.5f:
                    botAnimatorController.Animator.SetTrigger(Rotate);
                    break;
            }

            botData.BotStats.HasRotate = true;
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
                        botData.BotStats.IsRunning = true;
                        botData.BotStats.CurrentSpeed = botData.BotStats.MaxSpeed;
                    }
                }
                else
                {
                    if (Math.Abs(botData.BotStats.CurrentSpeed - botData.BotStats.WalkSpeed) > 0.0001f)
                    {
                        botData.BotStats.IsRunning = false;
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
            if (botInput.MoveDown.action.triggered && !botData.BotStats.IsCrouching && !botData.BotStats.IsDashing)
            {
                botData.BotComponents.Rb.velocity = new Vector2(0, botData.BotComponents.Rb.velocity.y);
                botData.BotComponents.Coll.enabled = false;
                botData.BotComponents.BoxCollider.enabled = true;
                botData.BotStats.CurrentSpeed = 0;
                botData.BotStats.DirectionTime = 0;
                botAnimatorController.Animator.SetTrigger(Crouch1);
                botData.BotStats.HasCrouched = false;
                botData.BotStats.IsCrouching = true;
            }
            else if (!botInput.MoveDown.action.IsPressed() && botData.BotStats.IsCrouching && !botData.BotStats.HasCrouched)
            {
                botData.BotComponents.BoxCollider.enabled = false;
                botData.BotComponents.Coll.enabled = true;

                botAnimatorController.Animator.SetTrigger(CrouchToStand);
                botData.BotStats.HasCrouched = true;
            }
        }

        private void HandleDashAnimation()
        {
            if (botData.BotStats.IsDashing && botData.BotStats.HasDashed && botData.BotStats.MoveDirection.x != 0)
            {
                botAnimatorController.Animator.SetTrigger(Dash);
                botData.BotStats.HasDashed = false;
            }
                
        }
        public override void ExitState()
        {
            
        }

        public BotGroundedState(BotStateMachine currentContext, BotMovement botMovement, BotDash botDash, BotInput botInput, BotData botData, BotAnimatorController botAnimatorController) : base(currentContext, botMovement, botDash, botInput, botData, botAnimatorController)
        {
        }
    }
}