
namespace Player.PlayerMovement // Namespace for managing player movement
{
    // Abstract class serving as a base for character states
    public abstract class CharacterBaseState
    {
        // Protected fields for accessing within derived classes
        protected CharacterStateMachine ctx; // Current state machine context
        protected CharacterStateFactory factory; // Factory for creating character states
        protected readonly CharacterStats stats; // Character's statistics
        protected readonly DetectionStats detectionStats; // Detection statistics
        protected readonly CharacterDetection characterDetection; // Character detection component
        protected readonly CharacterComponents characterComponents; // Character components
        protected readonly CharacterInput characterInput; // Character input component
        protected readonly CharacterDash characterDash; // Character dash component
        protected readonly CharacterMovement characterMovement; // Character movement component
        protected readonly CharacterJump characterJump; // Character jump component
        protected readonly CharacterAnimation characterAnimation; // Character animation component

        // Constructor initializing protected fields
        protected CharacterBaseState(CharacterStateMachine currentContext, CharacterStateFactory characterStateFactory,
            CharacterStats characterStats, DetectionStats detectionStats, CharacterDetection characterDetection,
            CharacterComponents components, CharacterInput input, CharacterDash characterDash,
            CharacterMovement characterMovement, CharacterJump characterJump, CharacterAnimation characterAnimation)
        {
            ctx = currentContext; // Assigning current state machine context
            factory = characterStateFactory; // Assigning character state factory
            stats = characterStats; // Assigning character statistics
            this.detectionStats = detectionStats; // Assigning detection statistics
            characterComponents = components; // Assigning character components
            this.characterDetection = characterDetection; // Assigning character detection component
            characterInput = input; // Assigning character input component
            this.characterDash = characterDash; // Assigning character dash component
            this.characterMovement = characterMovement; // Assigning character movement component
            this.characterJump = characterJump; // Assigning character jump component
            this.characterAnimation = characterAnimation; // Assigning character animation component
        }

        // Abstract methods for entering, updating, fixing, and exiting states
        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void FixedUpdate();
        public abstract void ExitState();
    }
}