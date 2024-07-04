using UnityEngine;

namespace Character.CharacterScripts
{
    [CreateAssetMenu(menuName = "Create BotStats", fileName = "BotStats", order = 0)]
    public class BotStats : ScriptableObject
    {
        #region Character Move Speed Values
        [Header("Character Move Speed Values")]
        [Space]
        [Tooltip("The current speed of the character.")]
        [SerializeField] private float currentSpeed;
        [Tooltip("The maximum speed the character can reach.")]
        [SerializeField] private float maxSpeed;
        [Tooltip("The walking speed of the character.")]
        [SerializeField] private float walkSpeed;
        [Tooltip("The deceleration rate when the character stops moving.")]
        [SerializeField] private float moveDeceleration;
        [Tooltip("The time it takes to smoothly change speed.")]
        [SerializeField] private float smoothTime;

        public float CurrentSpeed
        {
            get => currentSpeed;
            set => currentSpeed = value;
        }
        public float MaxSpeed => maxSpeed;
        public float WalkSpeed => walkSpeed;
        public float MoveDeceleration => moveDeceleration;
        public float SmoothTime => smoothTime;
        #endregion

        #region Character Move Direction Values
        [Header("Character Move Direction Values")]
        [Space]
        [Tooltip("The current move direction of the character.")]
        [SerializeField] private Vector3 moveDirection;
        [Tooltip("The current direction value.")]
        [SerializeField] private int currentDirectionValue;
        [Tooltip("The last direction value.")]
        [SerializeField] private int lastDirectionValue;
        [Tooltip("The time spent moving in the current direction.")]
        [SerializeField] private float directionTime;
        [Tooltip("The target angle for rotation.")]
        [SerializeField] private float targetAngle;

        public float VelocityX { get; set; }
        public Vector3 MoveDirection
        {
            get => moveDirection;
            set => moveDirection = value;
        }
        public int CurrentDirectionValue
        {
            get => currentDirectionValue;
            set => currentDirectionValue = value;
        }
        public int LastDirectionValue
        {
            get => lastDirectionValue;
            set => lastDirectionValue = value;
        }
        public float DirectionTime
        {
            get => directionTime;
            set => directionTime = value;
        }
        public float TargetAngle
        {
            get => targetAngle;
            set => targetAngle = value;
        }
        #endregion

        #region Character Dash Values
        [Header("Character Dash Values")]
        [Space]
        [Tooltip("The force applied during a dash.")]
        [SerializeField] private int dashForce;
        [Tooltip("Whether the character is currently dashing.")]
        [SerializeField] private bool isDashing;
        [Tooltip("Whether the character has dashed.")]
        [SerializeField] private bool hasDashed;
        [Tooltip("Whether the character can dash.")]
        [SerializeField] private bool canDash;
        [Tooltip("The duration of the dash.")]
        [SerializeField] private float dashDuration;
        [Tooltip("The timer for the dash duration.")]
        [SerializeField] private float dashTimer;
        [Tooltip("The start time of the dash cooldown.")]
        [SerializeField] private float dashCooldownStart;
        [Tooltip("The cooldown time before the character can dash again.")]
        [SerializeField] private float dashCooldown;
        [Tooltip("The duration of the dash in air.")]
        [SerializeField] private float dashDurationAir;
        [Tooltip("The duration of the dash on ground.")]
        [SerializeField] private float dashDurationGround;

        public float DashDurationAir => dashDurationAir;
        public float DashDurationGround => dashDurationGround;
        public bool HasDashed
        {
            get => hasDashed;
            set => hasDashed = value;
        }
        public float DashCooldownStart
        {
            get => dashCooldownStart;
            set => dashCooldownStart = value;
        }
        public float DashTimer
        {
            get => dashTimer;
            set => dashTimer = value;
        }
        public float DashCooldown
        {
            get => dashCooldown;
            set => dashCooldown = value;
        }

        public float DashDuration
        {
            get => dashDuration;
            set => dashDuration = value;
        }
        public bool CanDash
        {
            get => canDash;
            set => canDash = value;
        }
        public int DashForce => dashForce;
        public bool IsDashing
        {
            get => isDashing;
            set => isDashing = value;
        }
        #endregion

        #region Character Move States
        [Header("Character Move States")]
        [Space]
        [Tooltip("Whether the character is rotating.")]
        [SerializeField] private bool isRotating;
        [Tooltip("Whether the character is crouching.")]
        [SerializeField] private bool isCrouching;
        [Tooltip("Whether the character has crouched.")]
        [SerializeField] private bool hasCrouched;
        [Tooltip("Whether the character has stopped.")]
        [SerializeField] private bool hasStopped;

        public bool IsRotating
        {
            get => isRotating;
            set => isRotating = value;
        }
        public bool IsCrouching
        {
            get => isCrouching;
            set => isCrouching = value;
        }
        public bool HasCrouched
        {
            get => hasCrouched;
            set => hasCrouched = value;
        }
        public bool HasStopped
        {
            get => hasStopped;
            set => hasStopped = value;
        }
        #endregion

        #region Character Jump/Air Values
        [Header("Character Jump/Air Values")]
        [Space]
        [Tooltip("The force applied during a jump.")]
        [SerializeField] private float jumpForce;
        [Tooltip("Whether the character is jumping.")]
        [SerializeField] private bool isJump;
        [Tooltip("Whether the character is falling.")]
        [SerializeField] private bool isFalling;
        [Tooltip("Whether the character is hasJumped.")]
        [SerializeField] private bool hasJumped;
        [Tooltip("Force of Minimum Jump")]
        [SerializeField] private float initialJumpForce;
        [Tooltip("Whether the character is gliding.")]
        [SerializeField] private bool isGliding;
        [Tooltip("Whether the character was glided.")]
        [SerializeField] private bool hasGlided;
        [Tooltip("Apply gravity in ground state.")]
        [SerializeField] private Vector2 groundG;
        [Tooltip("Apply gravity in air state.")]
        [SerializeField] private Vector2 airG;
        [Tooltip("Apply gravity in gliding state.")]
        [SerializeField] private Vector2 glidingG;

        public Vector2 GlidingG => glidingG;
        public Vector2 AirG => airG;
        public Vector2 GroundG => groundG;
        
        public bool HasGlided
        {
            get => hasGlided;
            set => hasGlided = value;
        }
        public bool IsGliding
        {
            get => isGliding;
            set => isGliding = value;
        }

        public bool IsFalling
        {
            get => isFalling;
            set => isFalling = value;
        }
        public bool IsJump
        {
            get => isJump;
            set => isJump = value;
        }
        public float JumpForce => jumpForce;

        public float InitialJumpForce => initialJumpForce;

        public bool HasJumped
        {
            get => hasJumped;
            set => hasJumped = value;
        }
        #endregion
    }
}