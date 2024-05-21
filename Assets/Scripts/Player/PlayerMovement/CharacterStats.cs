using UnityEngine;
using UnityEngine.Serialization;

namespace Player.PlayerMovement
{
    // This class defines the stats and properties of the player character
    [CreateAssetMenu(menuName = "Create CharacterStats", fileName = "CharacterStats")]
    public class CharacterStats : ScriptableObject
    {
        // Character Jump Parameters
        [Header("Character Jump")] 
        [SerializeField] private float jumpForce; // Force applied when jumping
        [SerializeField] private float megaJumpForce; // Force applied when jumping
        [SerializeField] private int maxJump; // Maximum number of jumps allowed
        [SerializeField] private int numberOfJumps; // Current number of jumps
        [SerializeField] private bool isJump; // Flag indicating whether the character is currently jumping
        [SerializeField] private float gravity; // Gravity affecting the character
        [SerializeField] private float jumpEndEarlyGravityModifier; // Gravity modifier for ending jump early
        public bool EndedJumpEarly { get; set; } // Flag indicating whether the jump ended early

        // Character Movement Parameters
        [FormerlySerializedAs("moveSpeed")] // For backward compatibility
        [Header("Character Movement")]
        [SerializeField] private float dashDuration; // Duration of dash
        [SerializeField] private float dashTimer; // Timer for dash
        [SerializeField] private bool isFalling; // Flag indicating whether the character is falling
        [SerializeField] private Vector2 moveVector; // Movement vector
        [SerializeField] private bool isDashing; // Flag indicating whether the character is dashing
        [SerializeField] private float dashCooldownTimer; // Timer for dash cooldown
        [SerializeField] private float smoothTime; // Smooth time for movement
        [SerializeField] private float dashSpeed; // Speed of dash
        [SerializeField] private float moveSpeed; // Speed of movement

        // Character Attack Parameters
        [Header("Character Attack")]
        [SerializeField] private bool isAttacking; // Flag indicating whether the character is attacking
        [SerializeField] private int attackCounter; // Counter for attacks
        [SerializeField] private int airAttackCounter; // Counter for air attacks

        // Properties for velocity
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }

        // Property for movement vector
        public Vector2 MoveVector
        {
            get => moveVector;
            set => moveVector = value;
        }

        // Property for dash speed
        public float DashSpeed
        {
            get => dashSpeed;
            set => dashSpeed = value;
        }

        // Property for smooth time
        public float SmoothTime
        {
            get => smoothTime;
            set => smoothTime = value;
        }

        // Property for checking if the character is dashing
        public bool IsDashing { get=> isDashing; set => isDashing = value; }

        // Property for dash cooldown timer
        public float DashCooldownTimer { get => dashCooldownTimer; set => dashCooldownTimer = value; }

        // Property for jump force
        public float JumpForce { get => jumpForce; set => jumpForce = value; }
        public float MegaJumpForce { get => megaJumpForce; set => megaJumpForce = value; }


        // Property for maximum number of jumps
        public int MaxJump { get => maxJump; set => maxJump = value; }

        // Property for current number of jumps
        public int NumberOfJumps { get => numberOfJumps; set => numberOfJumps = value; }

        // Readonly property for jump end early gravity modifier
        public float JumpEndEarlyGravityModifier => jumpEndEarlyGravityModifier;

        // Readonly property for gravity
        public float Gravity => gravity;

        // Property for checking if the character is jumping
        public bool IsJump
        {
            get => isJump;
            set => isJump = value;
        }

        // Property for movement speed
        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

        // Readonly property for dash duration
        public float DashDuration => dashDuration;

        // Property for dash timer
        public float DashTimer { get => dashTimer; set => dashTimer = value; }

        // Property for checking if the character is falling
        public bool IsFalling { get => isFalling; set => isFalling = value; }

        // Property for checking if the character is attacking
        public bool IsAttacking
        {
            get => isAttacking;
            set => isAttacking = value;
        }

        // Property for attack counter
        public int AttackCounter
        {
            get => attackCounter;
            set => attackCounter = value;
        }

        // Property for air attack counter
        public int AirAttackCounter
        {
            get => airAttackCounter;
            set => airAttackCounter = value;
        }
    }
}
