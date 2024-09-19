using System.Collections.Generic;

namespace Character.CharacterScripts
{
    public enum States
    {
        GroundedState,
        AirState,
        ClimbState,
    }
    public class BotStateFactory
    {
        private readonly Dictionary<States, BotBaseState> container = new();

        public BotStateFactory(BotStateMachine currentContext,
            BotData botData, BotMovement botMovement, BotInput botInput)
        {
            container[States.GroundedState] =
                new BotGroundedState(currentContext,botMovement, botInput,botData);
            container[States.AirState] =
                new BotAirState(currentContext, botMovement, botInput, botData);
            container[States.ClimbState] =
                new BotClimbState(currentContext, botMovement, botInput, botData);
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