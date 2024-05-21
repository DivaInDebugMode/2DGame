using Save_Load;
using UnityEngine;

// Namespace and class declaration for managing character states and movements
namespace Player.PlayerMovement
{
    // Class responsible for managing the character's state machine and data persistence
    public class CharacterStateMachine : MonoBehaviour, IDataPersistence
    {
        // Private variables for managing states and character components
        private CharacterStateFactory states;
        private CharacterBaseState currentState;
        [SerializeField] private CharacterStats characterStats;
        [SerializeField] private DetectionStats detectionStats;
        [SerializeField] private CharacterDetection characterDetection;
        [SerializeField] private CharacterComponents characterComponents;
        [SerializeField] private CharacterInput characterInput;
        [SerializeField] private CharacterDash characterDash;
        [SerializeField] private CharacterMovement characterMovement;
        [SerializeField] private CharacterJump characterJump;
        [SerializeField] private CharacterAnimation characterAnimation;

        //testtesttest
        // Called when the script instance is being loaded
        private void Awake()
        {
            // Initializing the character state factory and setting initial state
            states = new CharacterStateFactory(this, characterStats, detectionStats, characterDetection,
                characterComponents, characterInput, characterDash, characterMovement, characterJump, characterAnimation);
            currentState = states.Grounded();
            currentState.EnterState();
        }
        
        // Called every fixed framerate frame
        private void FixedUpdate()
        {
            currentState.FixedUpdate();
        }
        
        // Called every frame
        private void Update()
        {
            currentState.UpdateState();
            StateSwitcher();
            if (currentState != states.Climb()) characterAnimation.UpdateSpriteDirection();
        }
        
        
        
        // Switches between different character states based on conditions
        private void StateSwitcher()
        {
            if (detectionStats.IsGrounded && !characterStats.IsJump ||detectionStats.IsOnPlatform && !characterStats.IsJump)
            {
                CheckState(states.Grounded());
            }

            if (characterStats.IsJump || !detectionStats.IsGrounded &&!detectionStats.IsOnPlatform && !detectionStats.IsLedgeDetected &&
                !detectionStats.IsWall)
            {
                CheckState(states.Air());
            }

            if (detectionStats.IsWall && !detectionStats.IsGrounded && !detectionStats.IsOnPlatform && detectionStats.WallCollisionRadius > 0)
            {
                CheckState(states.Climb());
            }
        }

       

        // Checks and transitions to a new state if necessary
        private void CheckState(CharacterBaseState newState)
        {
            if (currentState == newState) return;
            currentState.ExitState();
            currentState = newState;
            currentState.EnterState();
        }

        // Loads character data from a save file
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
