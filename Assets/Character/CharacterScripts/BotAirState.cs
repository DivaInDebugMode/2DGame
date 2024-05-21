namespace Character.CharacterScripts
{
    public class BotAirState : BotBaseState
    {
        public BotAirState(BotStateMachine currentContext, BotMovement botMovement, BotInput botInput, BotData botData, BotAnimatorController botAnimatorController) : base(currentContext, botMovement, botInput, botData, botAnimatorController)
        {
        }

        public override void EnterState()
        {
            botData.BotComponents.Coll.center = botData.BotStats.CollCentreGround;
        }

        public override void UpdateState()
        {
           
        }

        public override void FixedUpdate()
        {
          
        }

        public override void ExitState()
        {
           
        }
    }
}