
namespace Character.CharacterScripts
{
    public class BotGroundedState : BotBaseState
    {
        public override void EnterState()
        {
           

        }

        public override void UpdateState()
        {
            MoveAnim();
            botData.BotDetection.CheckGroundDistance();
            
            
        }

        public override void FixedUpdate()
        {
            botMovement.MoveHorizontally(botData.BotStats.MoveSpeed);
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
                    switch ( botData.BotStats.IsRotating)
                    {
                        case false:
                            botAnimatorController.ChangeAnimationState(botAnimatorController.botRunAnim, 0f);
                            break;
                        case true:
                            botAnimatorController.ChangeAnimationState(
                                botData.BotStats.CurrentDirection == Directions.Left
                                    ? botAnimatorController.botRunningTurnLeft
                                    : botAnimatorController.botRunningTurnRight, 0f);
                            break;
                    }
                    break;
                case 0:
                    if (botData.BotStats.IsRunning)
                    {
                        if (!botData.BotStats.IsRotating && !botData.BotStats.IsFallingEdge)
                            botAnimatorController.ChangeAnimationState(botAnimatorController.botRunToStopAnim, 0.1f);

                    }
                    
                    else if (botData.BotStats.IsFallingEdge)
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