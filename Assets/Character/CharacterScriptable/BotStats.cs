using Character.CharacterScripts;
using UnityEngine;

namespace Character.CharacterScriptable
{
    [CreateAssetMenu(menuName = "Create BotStats", fileName = "BotStats", order = 0)]
    public class BotStats : ScriptableObject
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector3 moveDirection;
        [SerializeField] private float smoothTime;
        [SerializeField] private bool isRunning;
        [SerializeField] private bool isRotating;
        [SerializeField] private  Directions currentDirection;
        [SerializeField] private bool isFallingEdge;
        [SerializeField] private Vector3 collCentreGround;
        [SerializeField] private Vector3 collCentreEdge;
        


        public Vector3 CollCentreGround => collCentreGround;
        public Vector3 CollCentreEdge => collCentreEdge;


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

        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = value;
        }

      
        public Vector3 MoveDirection
        {
            get => moveDirection;
            set => moveDirection = value;
        }

        public float SmoothTime
        {
            get => smoothTime;
            set => smoothTime = value;
        }
    }
}
