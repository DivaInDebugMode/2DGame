using Character.CharacterScripts;
using UnityEngine;

namespace Character.CharacterScriptable
{
    [CreateAssetMenu(menuName = "Create BotStats", fileName = "BotStats", order = 0)]
    public class BotStats : ScriptableObject
    {
        [SerializeField] private float currentSpeed;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float walkSpeed;
        [SerializeField] private int currentDirectionValue;
        [SerializeField] private float directionTime;
        [SerializeField] private float moveDeceleration;
        [SerializeField] private Vector3 moveDirection;
        [SerializeField] private float smoothTime;
        [SerializeField] private bool isRunning;
        [SerializeField] private bool isRotating;
        [SerializeField] private bool isFallingEdge;
        [SerializeField] private Vector3 collCentreGround;
        [SerializeField] private bool hasStopped;
        [SerializeField] private bool isCrouching;
        [SerializeField] private bool hasCrouched;
        [SerializeField] private bool isDashing;
        [SerializeField] private int dashForce;
        [SerializeField] private bool canDash;
        [SerializeField] private float dashDuration;
        [SerializeField] private float dashCooldown;
        [SerializeField] private float dashTimer;
        [SerializeField] private float dashCooldownStart;
        [SerializeField] private bool hasDashed;
        [SerializeField] private float targetAngle;
        [SerializeField] private int lastDirectionValue;
        [SerializeField] private float jumpForce;
        [SerializeField] private int numberOfJump;
        [SerializeField] private int maxJump;
        [SerializeField] private bool endedJumPEarly;
        [SerializeField] private float jumpEarlyGravityModifier;
        [SerializeField] private bool isJump;



        public int NumberOfJump
        {
            get => numberOfJump;
            set => numberOfJump = value;
        }

        public bool IsJump
        {
            get => isJump;
            set => isJump = value;
        }
        public int MaxJump => maxJump;

        public bool EndedJumpEarly
        {
            get => endedJumPEarly;
            set => endedJumPEarly = value;
        }

        public float JumpEarlyGravityModifier
        {
            get => jumpEarlyGravityModifier;
            set => jumpEarlyGravityModifier = value;
        }
        public float JumpForce
        {
            get => jumpForce;
            set => jumpForce = value;
        }

        public int LastDirectionValue
        {
            get => lastDirectionValue;
            set => lastDirectionValue = value;
        }
        public float TargetAngle
        {
            get => targetAngle;
            set => targetAngle = value; 
        }
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

        public int DashForce
        {
            get => dashForce;
            set => dashForce = value;
        }

        public bool IsDashing
        {
            get => isDashing;
            set => isDashing = value;
        }

        public int CurrentDirectionValue
        {
            get => currentDirectionValue;
            set => currentDirectionValue = value;
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

        public float DirectionTime
        {
            get => directionTime;
            set => directionTime = value;
        }
        
        public bool HasStopped
        {
            get => hasStopped;
            set => hasStopped = value;
        }
        
        public float MaxSpeed
        {
            get => maxSpeed;
            set => maxSpeed = value;
        }

        public float MoveDeceleration => moveDeceleration;

        public Vector3 CollCentreGround => collCentreGround;


        public bool IsFallingEdge
        {
            get => isFallingEdge;
            set => isFallingEdge = value;
        }
        public bool IsRotating
        {
            get => isRotating;
            set => isRotating = value;
        }
        public bool IsRunning
        {
            get => isRunning;
            set => isRunning = value;
        }

        public float VelocityX { get; set; }

        public float CurrentSpeed
        {
            get => currentSpeed;
            set => currentSpeed = value;
        }

      
        public Vector3 MoveDirection
        {
            get => moveDirection;
            set => moveDirection = value;
        }

        public float SmoothTime => smoothTime;

        public float WalkSpeed => walkSpeed;
    }
}