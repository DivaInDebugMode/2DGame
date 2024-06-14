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
        [SerializeField] private bool hasRotate;
        [SerializeField] private float moveDeceleration;
        [SerializeField] private Vector3 moveDirection;
        [SerializeField] private float smoothTime;
        [SerializeField] private bool isRunning;
        [SerializeField] private bool isRotating;
        [SerializeField] private Directions currentDirection;
        [SerializeField] private bool isFallingEdge;
        [SerializeField] private Vector3 collCentreGround;
        [SerializeField] private bool hasStopped;
        [SerializeField] private bool isCrouching;
        [SerializeField] private bool hasCrouched;

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
        public bool HasRotate
        {
            get => hasRotate;
            set => hasRotate = value;
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
        public Directions CurrentDirection
        {
            get => currentDirection;
            set => currentDirection = value;
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
