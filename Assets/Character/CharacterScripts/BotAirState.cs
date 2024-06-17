namespace Character.CharacterScripts
{
    public class BotAirState : BotBaseState
    {
        

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

        public BotAirState(BotStateMachine currentContext, BotMovement botMovement, BotDash botDash, BotInput botInput, BotData botData, BotAnimatorController botAnimatorController) : base(currentContext, botMovement, botDash, botInput, botData, botAnimatorController)
        {
        }
    }
}