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

        [SerializeField] private LayerMask horizontalPlatform;
        [SerializeField] private LayerMask verticalPlatform;
        
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
        public LayerMask HorizontalPlatform => horizontalPlatform;
        public LayerMask VerticalPlatform => verticalPlatform;
        #endregion

        #region Grounded State
        [Header("Grounded State")]
        [Tooltip("Indicates whether the bot is currently grounded.")]
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isNearOnGround;
        [SerializeField] private bool isDistanceForDash;
        [SerializeField] private LayerMask hurricaneBounce;
        [SerializeField] private LayerMask megaJumpBounce;
        
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
        
        public LayerMask HurricaneBounce => hurricaneBounce;
        public LayerMask MegaJumpBounce => megaJumpBounce;
        
        #endregion
    }
}
