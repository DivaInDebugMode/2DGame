using System.Collections.Generic;
using Character.CharacterScriptable;

namespace Character.CharacterScripts
{
    public enum States
    {
        GroundedState,
        AirState
    }
    public class BotStateFactory
    {
        private readonly Dictionary<States, BotBaseState> container = new();

        public BotStateFactory(BotStateMachine currentContext,
            BotData botData, BotMovement botMovement, BotInput botInput, BotAnimatorController botAnimatorController,BotJump botJump)
        {
            container[States.GroundedState] =
                new BotGroundedState(currentContext,botMovement, botInput,botData, botAnimatorController,botJump);
            container[States.AirState] =
                new BotAirState(currentContext, botMovement, botInput, botData, botAnimatorController,botJump);
        }
        
        public BotBaseState Grounded()
        {
            return container[States.GroundedState];
        }

        public BotBaseState Air()
        {
            return container[States.AirState];
        }
    }
}