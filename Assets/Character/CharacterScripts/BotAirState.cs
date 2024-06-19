using Character.CharacterScriptable;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotAirState : BotBaseState
    {
        private static readonly int Jump = Animator.StringToHash("Jump");


        public override void EnterState()
        {
           // botData.BotComponents.Coll.center = botData.BotStats.CollCentreGround;
           Debug.Log("eneter");
            
            
        }

        public override void UpdateState()
        {
            botJump.Jump();
            if (botInput.Jump.action.inProgress && !ctx.test)
            {
                botAnimatorController.Animator.SetTrigger(Jump);
                ctx.test = true;
                Debug.Log("update");
            }
        }

        public override void FixedUpdate()
        {
         
        }

        public override void ExitState()
        {
           
        }


        public BotAirState(BotStateMachine currentContext, BotMovement botMovement, BotInput botInput, BotData botData, BotAnimatorController botAnimatorController, BotJump botJump) : base(currentContext, botMovement, botInput, botData, botAnimatorController, botJump)
        {
        }
    }
}