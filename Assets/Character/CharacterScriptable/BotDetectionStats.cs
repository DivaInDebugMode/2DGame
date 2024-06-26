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
        [Tooltip("Layer mask to detect the edge the bot can fall off.")]
        [SerializeField] private LayerMask edge;

        public LayerMask Grounded => grounded;
        public LayerMask FallingEdge => edge;
        #endregion

        #region Grounded State
        [Header("Grounded State")]
        [Tooltip("Indicates whether the bot is currently grounded.")]
        [SerializeField] private bool isGrounded;

        public bool IsGrounded
        {
            get => isGrounded;
            set => isGrounded = value;
        }
        #endregion
    }
}
