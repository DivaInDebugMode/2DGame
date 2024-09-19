using Character.CharacterScriptable;

namespace Character.CharacterScripts
{
    public abstract class BotBaseState
    {
        protected BotStateMachine ctx;
        protected readonly BotMovement botMovement;
        protected BotInput botInput;
        protected readonly BotData botData;
        
        protected BotBaseState(BotStateMachine currentContext,BotMovement botMovement, BotInput botInput,
           BotData botData)
        {
            ctx = currentContext;
            this.botMovement = botMovement;
            this.botInput = botInput;
            this.botData = botData;
        }

        // Abstract methods for entering, updating, fixing, and exiting states
        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void FixedUpdate();
        public abstract void ExitState();
    }
}