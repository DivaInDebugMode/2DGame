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

        [SerializeField] private bool isRope;
        [SerializeField] private bool isRopeTail;
        [SerializeField] private bool isNearOnGround;
        [SerializeField] private bool isDistanceForDash;

        [SerializeField] private bool isGroundForClimb;

        public bool IsRopeTail
        {
            get => isRopeTail;
            set => isRopeTail = value;
        }
        public bool IsRope
        {
            get => isRope;
            set => isRope = value;
        }
        public bool IsDistanceForDash
        {
            get => isDistanceForDash;
            set => isDistanceForDash = value;
        }
        public bool IsGroundForClimb
        {
            get => isGroundForClimb;
            set => isGroundForClimb = value;
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
        [SerializeField] private LayerMask water;
        
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
        public LayerMask Water
        {
            get => water;
            set => water = value;
        }
        
    }
}
