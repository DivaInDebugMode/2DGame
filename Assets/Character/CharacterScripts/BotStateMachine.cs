using Character.CharacterScriptable;
using Save_Load;
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
        [SerializeField] private BotDash botDash;
        private void Awake()
        {
            states = new BotStateFactory(this,botData,botMovement,botInput,botAnimatorController,botDash);
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
            if (botData.BotDetectionStats.IsGrounded && !botData.BotStats.IsJump)
            {
                CheckState(states.Grounded());
            }
            if (!botData.BotDetectionStats.IsGrounded && !botData.BotDetectionStats.IsWall || botData.BotStats.IsJump || botData.BotStats.IsInLedgeClimbing)
            {
                CheckState(states.Air());
            }

            if (botData.BotDetectionStats.IsWall && !botData.BotDetectionStats.IsGrounded &&
                botData.BotDetectionStats.WallDetectionRadius > 0f && !botData.BotStats.IsJump && !botData.BotStats.IsInLedgeClimbing)
            {
                CheckState(states.Climb());
            }
        }
    
        private void CheckState(BotBaseState newState)
        {
            if (currentState == newState) return;
            currentState.ExitState();
            currentState = newState;
            currentState.EnterState();
        }
        
        public void LoadData(GameData data)
        {
            transform.position = data.playerPos;
        }

        // Saves character data to a save file
        public void SaveData(ref GameData data)
        {
            data.playerPos = transform.position;
        }
    }
}
