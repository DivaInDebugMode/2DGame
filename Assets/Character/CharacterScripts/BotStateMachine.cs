using Character.CharacterScriptable;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotStateMachine : MonoBehaviour
    {
        private BotStateFactory states;
        private BotBaseState currentState;
        [SerializeField] private BotData botData;
        [SerializeField] private BotInput botInput;
        [SerializeField] private BotMovement botMovement;
        [SerializeField] private BotAnimatorController botAnimatorController;
        [SerializeField] private BotJump botJump;
        public bool test { get; set; }
        private void Awake()
        {
            states = new BotStateFactory(this,botData,botMovement,botInput,botAnimatorController,botJump);
            currentState = states.Grounded();
            currentState.EnterState();
        }
        private void FixedUpdate()
        {
            currentState.FixedUpdate();
        }
        private void Update()
        {
            currentState.UpdateState();
            StateSwitcher();
          
         
        }
        
        private void StateSwitcher()
        {
            if(botData.BotDetectionStats.IsGrounded && !botData.BotStats.IsJump)
                CheckState(states.Grounded());
            if(!botData.BotDetectionStats.IsGrounded || botData.BotStats.IsJump)
                CheckState(states.Air());
        }
    
        private void CheckState(BotBaseState newState)
        {
            if (currentState == newState) return;
            currentState.ExitState();
            currentState = newState;
            currentState.EnterState();
        }
    }
}
