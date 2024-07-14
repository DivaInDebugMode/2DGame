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
        
        [SerializeField] private bool isLedge;
        [SerializeField] private bool isWall;
        [SerializeField] private float wallDetectionRadius;

        public float WallDetectionRadius
        {
            get => wallDetectionRadius;
            set => wallDetectionRadius = value;
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
        #endregion

        #region Grounded State
        [Header("Grounded State")]
        [Tooltip("Indicates whether the bot is currently grounded.")]
        [SerializeField] private bool isGrounded;

        [SerializeField] private bool isNearOnGround;

        [SerializeField] private bool isGroundForClimb;

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
    }
}
