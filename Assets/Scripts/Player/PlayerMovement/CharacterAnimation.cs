using UnityEngine;

// Namespace for managing player movement animations
namespace Player.PlayerMovement
{
    // Handles character animations
    public class CharacterAnimation : MonoBehaviour // Defining a class named CharacterAnimation that inherits from MonoBehaviour
    {
        // References to other components
        [SerializeField] private CharacterComponents characterComponents; // Reference to character components
        [SerializeField] private CharacterInput characterInput; // Reference to character input
        [SerializeField] private CharacterStats characterStats;

        // Animator and animation states
        [Header("Character Sprite Animations")] [SerializeField]
        private Animator anim;
        public string idleAnim = "Player_Idle";
        public string runAnim = "Player_Run";
        public string jumpAnim = "Player_Jump";
        public string doubleJumpAnim = "Player_DoubleJump";
        public string dashAnim = "Player_Dash";
        public string rollAnim = "Player_Roll";
        public string kickAnim = "Player_Kick";
        public string kick01Anim = "Player_Kick01";
        public string punch01Anim = "Player_Punch01";
        public string landAnim = "Player_Land";
        public string ledgeClimbAnim = "Player_LedgeClimb";
        public string ledgeIdleAnim = "Player_LedgeHang";
        public string climbTurnAnim = "Player_ClimbTurn";
        public string wallIdleAnim = "Player_WallIdle";
        public string wallClimbAnim = "Player_WallClimb";
        public string groundAttack01 = "Player_Attack01";
        public string groundUpAttack = "Player_Attack02";
        public string groundAttack03 = "Player_Attack03";
        public string groundAttack04 = "Player_Attack04";

        private string currentAnimation; // Currently playing animation

        // Changes the character's animation state with a transition
        public void ChangeAnimationState(string newStateAnim, float transitionTime)
        {
            // Skip if the new animation state is already playing
            if (currentAnimation == newStateAnim) return;

            // Crossfade to the new animation state with the specified transition time
            anim.CrossFade(newStateAnim, transitionTime);

            // Update the current animation state
            currentAnimation = newStateAnim;
        }

        // Updates the character's sprite direction based on input
        public void UpdateSpriteDirection()
        {
            if(characterStats.IsDashing || characterStats.IsAttacking) return;
            // Flip the sprite horizontally if moving left
            if (characterInput.MoveLeft.action.ReadValue<float>() > 0.001f)
                characterComponents.Sr.flipX = true;

            // Flip the sprite horizontally if moving right
            if (characterInput.MoveRight.action.ReadValue<float>() > 0.001f)
                characterComponents.Sr.flipX = false;
        }
    }
}