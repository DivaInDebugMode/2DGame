using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Character.CharacterScriptable
{
    [CreateAssetMenu(menuName = "Create BotDetection", fileName = "BotDetectionStats", order = 0)]
    public class BotDetectionStats : ScriptableObject
    {
        #region Layer Masks
        [Header("Layer Masks")]
        [Tooltip("Layer mask to detect if the bot is grounded.")]
        [SerializeField] private LayerMask grounded;
        [Tooltip("Layer mask to detect the ledge.")]
        [SerializeField] private LayerMask wall;

        [SerializeField] private LayerMask rope;
        [SerializeField] private LayerMask ropeTail;
        [SerializeField] private LayerMask platform;
        
        [SerializeField] private bool isLedge;
        [SerializeField] private bool isWall;
        [SerializeField] private float wallDetectionRadius;
        [SerializeField] private bool isOnPlatform;

        public float WallDetectionRadius
        {
            get => wallDetectionRadius;
            set => wallDetectionRadius = value;
        }

        public bool IsOnPlatform
        {
            get => isOnPlatform;
            set => isOnPlatform = value;
        }
        public bool IsWall
        {
            get => isWall;
            set => isWall = value;
        }

        public bool IsLedge
        {
            get => isLedge;
            set => isLedge = value;
        }

        public LayerMask Grounded => grounded;
        public LayerMask Wall => wall;
        public LayerMask Rope => rope;
        public LayerMask RopeTail => ropeTail;
        public LayerMask Platform => platform;
        #endregion

        #region Grounded State
        [Header("Grounded State")]
        [Tooltip("Indicates whether the bot is currently grounded.")]
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isNearOnGround;
        [SerializeField] private bool isDistanceForDash;
        [SerializeField] private bool isGroundForClimb;
        [SerializeField] private LayerMask hurricaneBounce;
        [SerializeField] private LayerMask megaJumpBounce;
        [SerializeField] private bool isOnEdge;
        [SerializeField] private bool isOnEdgeWithSecondFoot;
        [SerializeField] private bool isClimbingGround;
        
        public bool IsClimbingGround
        {
            get => isClimbingGround;
            set => isClimbingGround = value;
        }

        public bool IsOnEdgeWithSecondFoot
        {
            get => isOnEdgeWithSecondFoot;
            set => isOnEdgeWithSecondFoot = value;
        }

        public bool IsOnEdge
        {
            get => isOnEdge;
            set => isOnEdge = value;
        }

        public bool IsDistanceForDash
        {
            get => isDistanceForDash;
            set => isDistanceForDash = value;
        }
        public bool IsNearOnGround
        {
            get => isNearOnGround;
            set => isNearOnGround = value;
        }

        public bool IsGrounded
        {
            get => isGrounded;
            set => isGrounded = value;
        }
        
        
        #endregion

        [Header("Grounded State")] 
        [SerializeField] private bool isOnIce;
        [SerializeField] private LayerMask ice;
        [SerializeField] private LayerMask edge;

        public LayerMask Edge
        {
            get => edge;
        }
        
        
        public bool IsOnIce
        {
            get => isOnIce;
            set => isOnIce = value;
        }

        public LayerMask Ice
        {
            get => ice;
            set => ice = value;
        }

        public LayerMask HurricaneBounce
        {
            get => hurricaneBounce;
        }

        public LayerMask MegaJumpBounce
        {
            get => megaJumpBounce;
        }
        
    }
}
