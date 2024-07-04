using System.Collections.Generic;
using Character.CharacterScriptable;

namespace Character.CharacterScripts
{
    public enum States
    {
        GroundedState,
        AirState,
        ClimbState
    }
    public class BotStateFactory
    {
        private readonly Dictionary<States, BotBaseState> container = new();

        public BotStateFactory(BotStateMachine currentContext,
            BotData botData, BotMovement botMovement, BotInput botInput, BotAnimatorController botAnimatorController,BotDash botDash)
        {
            container[States.GroundedState] =
                new BotGroundedState(currentContext,botMovement, botInput,botData, botAnimatorController,botDash);
            container[States.AirState] =
                new BotAirState(currentContext, botMovement, botInput, botData, botAnimatorController,botDash);
            container[States.ClimbState] =
                new BotClimbState(currentContext, botMovement, botInput, botData, botAnimatorController,botDash);
        }
        
        public BotBaseState Grounded()
        {
            return container[States.GroundedState];
        }

        public BotBaseState Air()
        {
            return container[States.AirState];
        }

        public BotBaseState Climb()
        {
            return container[States.ClimbState];
        }
    }
}