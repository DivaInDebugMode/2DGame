using System.Collections.Generic;

// Namespace for managing player movement
namespace Player.PlayerMovement
{
    // Enum defining different character states
    public enum States
    {
        GroundedState,  // Character is on the ground
        Air,            // Character is airborne
        Climb           // Character is climbing
    }

    // Class responsible for creating character states
    public class CharacterStateFactory
    {
        // Dictionary to store character states
        private readonly Dictionary<States, CharacterBaseState> container = new();

        // Constructor for CharacterStateFactory
        public CharacterStateFactory(CharacterStateMachine currentContext, CharacterStats characterStats,
            DetectionStats detectionStats,
            CharacterDetection detection, CharacterComponents components, CharacterInput characterInput,
            CharacterDash dash, CharacterMovement movement, CharacterJump jump,CharacterAnimation animation)
        {
            // Initializing different character states and adding them to the container
            container[States.GroundedState] = new CharacterGroundedState(currentContext, this, characterStats,
                detectionStats, detection, components, characterInput, dash, movement, jump, animation);
            container[States.Air] = new CharacterAirState(currentContext, this, characterStats, detectionStats,
                detection, components, characterInput, dash, movement, jump, animation);
            container[States.Climb] = new CharacterClimbState(currentContext, this, characterStats, detectionStats,
                detection, components, characterInput, dash, movement, jump, animation);
        }
        
        // Method to get the grounded state
        public CharacterBaseState Grounded()
        {
            return container[States.GroundedState];
        }

        // Method to get the air state
        public CharacterBaseState Air()
        {
            return container[States.Air];
        }
        
        // Method to get the climb state
        public CharacterBaseState Climb()
        {
            return container[States.Climb];
        }      
    }
}
