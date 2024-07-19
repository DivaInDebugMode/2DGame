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
        [SerializeField] private bool isGroundDashing;
        [Tooltip("Whether the character has dashed.")]
        [SerializeField] private bool hasGroundDashed;
        [Tooltip("Whether the character can dash.")]
        [SerializeField] private bool canGroundDash;
        [Tooltip("The duration of the dash.")]
        [SerializeField] private float groundDashDuration;
        [Tooltip("The timer for the dash duration.")]
        [SerializeField] private float groundDashTimer;
        [Tooltip("The start time of the dash cooldown.")]
        [SerializeField] private float groundDashCooldownStart;
        [Tooltip("The cooldown time before the character can dash again.")]
        [SerializeField] private float groundDashCooldown;
        [Tooltip("The duration of the dash on ground.")]
        [SerializeField] private float dashLengthTimeInGround;

        [Tooltip("Air Dash Variables")]
        [SerializeField] private bool isAirDashing;
        [Tooltip("Whether the character has dashed.")]
        [SerializeField] private bool hasAirDashed;
        [Tooltip("Whether the character can dash.")]
        [SerializeField] private bool canAirDash;
        [Tooltip("The duration of the dash.")]
        [SerializeField] private float airDashDuration;
        [Tooltip("The start time of the dash cooldown.")]
        [SerializeField] private float airDashCooldownStart;
        [Tooltip("The duration of the dash in air.")]
        [SerializeField] private float dashLengthTimeInAir;

        public float DashLengthTimeInAir => dashLengthTimeInAir;

        public bool HasAirDashed
        {
            get => hasAirDashed;
            set => hasAirDashed = value;
        }

        public float AirDashDuration
        {
            get => airDashDuration;
            set => airDashDuration = value;
        }

        public float AirDashCooldownStart
        {
            get => airDashCooldownStart;
            set => airDashCooldownStart = value;
        }
        public bool CanAirDash
        {
            get => canAirDash;
            set => canAirDash = value;
        }

        public bool IsAirDashing
        {
            get => isAirDashing;
            set => isAirDashing = value;
        }
        
        
        
        public float DashLengthTimeInGround => dashLengthTimeInGround;
        public bool HasGroundDashed
        {
            get => hasGroundDashed;
            set => hasGroundDashed = value;
        }
        public float GroundDashCooldownStart
        {
            get => groundDashCooldownStart;
            set => groundDashCooldownStart = value;
        }
        public float GroundDashTimer
        {
            get => groundDashTimer;
            set => groundDashTimer = value;
        }
        public float GroundDashCooldown
        {
            get => groundDashCooldown;
            set => groundDashCooldown = value;
        }

        public float GroundDashDuration
        {
            get => groundDashDuration;
            set => groundDashDuration = value;
        }
        public bool CanGroundDash
        {
            get => canGroundDash;
            set => canGroundDash = value;
        }

        public bool IsGroundDashing
        {
            get => isGroundDashing;
            set => isGroundDashing = value;
        }

        public int DashForce => dashForce;

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
        [Tooltip("Whether the character is wall jumping.")]
        [SerializeField] private bool isWallJump;
        [Tooltip("When player is in wall jump action")]
        [SerializeField] private bool wallJumpDurationStart;
        [Tooltip("Whether the character is hasJumped.")]
        [SerializeField] private bool hasJumped;
        [Tooltip("Whether the character is ledge climbing.")]
        [SerializeField] private bool isInLedgeClimbing;
        [Tooltip("Force of Minimum Jump")]
        [SerializeField] private float initialJumpForce;
        [Tooltip("Whether the character is gliding.")]
        [SerializeField] private bool isGliding;
        [Tooltip("Apply gravity in ground state.")]
        [SerializeField] private Vector2 groundGForce;
        [Tooltip("Apply gravity in air state.")]
        [SerializeField] private Vector2 fallingGForce;
        [Tooltip("Apply gravity in gliding state.")]
        [SerializeField] private Vector2 glidingGForce;
        [Tooltip("Apply gravity in wall state.")]
        [SerializeField] private Vector2 wallGForce;

        [SerializeField] private Vector2 ledgeJumpForce;
        [SerializeField] private float ledgeClimbingStartTime;
        [SerializeField] private float durationOfLedgeClimbing;

        public Vector2 LedgeJumpForce => ledgeJumpForce;

        public float LedgeClimbingStartTime
        {
            get => ledgeClimbingStartTime;
            set => ledgeClimbingStartTime = value;
        }

        public float DurationOfLedgeClimbing
        {
            get => durationOfLedgeClimbing;
            set => durationOfLedgeClimbing = value;
        }
        public Vector2 GlidingGForce => glidingGForce;
        public Vector2 FallingGForce => fallingGForce;
        public Vector2 GroundGForce => groundGForce;
        public Vector2 WallGForce => wallGForce;

        public bool IsInLedgeClimbing
        {
            get => isInLedgeClimbing;
            set => isInLedgeClimbing = value;
        }
        public bool WallJumpDurationStart
        {
            get => wallJumpDurationStart;
            set => wallJumpDurationStart = value;
        }
        public bool IsWallJump
        {
            get => isWallJump;
            set => isWallJump = value;
        }
        public bool IsGliding
        {
            get => isGliding;
            set => isGliding = value;
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