using UnityEngine;

// Namespace for managing player movement
namespace Player.PlayerMovement
{
    // ScriptableObject class for defining detection statistics
    [CreateAssetMenu(menuName = "Create CharacterDetection", fileName = "CharacterDetection")]
    public class DetectionStats : ScriptableObject
    {
        // Serialized fields for layer masks and detection parameters
        [Header("Layer Mask")] 
        [SerializeField] private LayerMask grounded;           // Layer mask for detecting ground
        [SerializeField] private LayerMask wallMask;           // Layer mask for detecting walls
        [SerializeField] private LayerMask platform;           // Layer mask for detecting platforms

        [Header("Float")] 
        [SerializeField] private float wallCollisionRadius;    // Radius for detecting wall collisions
        [SerializeField] private float ledgeLength;            // Length of ledge

        [Header("Bool")] 
        [SerializeField] private bool isLedgeDetected;         // Flag indicating if a ledge is detected
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isOnPlatform;
        [SerializeField] private bool isWall;
        [SerializeField] private bool isClimbing;

        // Properties to access serialized fields
        public LayerMask Grounded => grounded;                 // Property for accessing ground layer mask
        public LayerMask WallMask => wallMask;                 // Property for accessing wall layer mask

        public bool IsGrounded
        {
            get => isGrounded;
            set => isGrounded = value;
        }

        public bool IsClimbing
        {
            get => isClimbing;
            set => isClimbing = value;
        }

        public bool IsWall
        {
            get => isWall;
            set => isWall = value;
        }
        
        public bool IsOnPlatform
        {
            get => isOnPlatform;
            set => isOnPlatform = value;
        }
        
        public LayerMask Platform => platform;                 // Property for accessing platform layer mask

        public float WallCollisionRadius
        {
            get => wallCollisionRadius;
            set => wallCollisionRadius = value;

        } // Property for accessing wall collision radius
        public bool IsLedgeDetected                           // Property for accessing and setting ledge detection flag
        {
            get => isLedgeDetected;
            set => isLedgeDetected = value;
        }
        public float LedgeLength => ledgeLength;               // Property for accessing ledge length
       
    }
}
