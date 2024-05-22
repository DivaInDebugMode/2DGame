
using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotGroundedState : BotBaseState
    {
        private bool isOnEdge;
        private bool isEdgeOff;
        public override void EnterState()
        {

        }

        public override void UpdateState()
        {
            MoveAnim();
            botData.BotDetection.CheckGroundLeftFoot();
            botData.BotDetection.CheckGroundRightFoot();
        }

        public override void FixedUpdate()
        {
            botMovement.MoveHorizontally(botData.BotStats.MoveSpeed);
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

        public override void ExitState()
        {
            
        }
        
        private void MoveAnim()
        {
            switch (botData.BotStats.MoveDirection.x)
            {
                case > 0:
                case < 0:
                    if (botData.BotStats.IsFallingEdge) return;
                    botData.BotStats.IsRunning = true;
                    if (!botData.BotStats.IsRotating)
                    {
                        botAnimatorController.ChangeAnimationState(botAnimatorController.botRunAnim, 0.1f);
                    }
                    else if (botData.BotStats.IsRotating && botData.BotStats.MoveSpeed > 5.5f)
                    {
                        botAnimatorController.ChangeAnimationState(
                            botData.BotStats.CurrentDirection == Directions.Left
                                ? botAnimatorController.botRunningTurnLeft
                                : botAnimatorController.botRunningTurnRight, 0.1f);
                        //botData.BotStats.MoveSpeed = 6.5f;

                    }else if (botData.BotStats.IsRotating && botData.BotStats.MoveSpeed == 0f)
                    {
                        botAnimatorController.ChangeAnimationState(
                            botData.BotStats.CurrentDirection == Directions.Left
                                ? botAnimatorController.idleTurnLeft
                                : botAnimatorController.idleTurnRight, 0.1f);
                    }
                    //botData.BotStats.MoveSpeed = 6.5f;
                    break;
                case 0:
                    botData.BotStats.MoveSpeed = 0f;
                    // if (botData.BotStats.IsRunning)
                    // {
                    //     if (!botData.BotStats.IsRotating && !botData.BotStats.IsFallingEdge)
                    //         botAnimatorController.ChangeAnimationState(botAnimatorController.botRunToStopAnim, 0.1f);
                    // }
                    
                    if (botData.BotStats.IsFallingEdge)
                    {
                        botAnimatorController.ChangeAnimationState(botAnimatorController.fallingEdge, 0.1f);
                    }
                    
                    else if (!botData.BotStats.IsRunning)
                    {
                        botAnimatorController.ChangeAnimationState(botAnimatorController.botIdleAnim, 0.1f);
                    }
                    break;
            }
        }


        public BotGroundedState(BotStateMachine currentContext, BotMovement botMovement, BotInput botInput, BotData botData, BotAnimatorController botAnimatorController) : base(currentContext, botMovement, botInput, botData, botAnimatorController)
        {
        }
    }
}