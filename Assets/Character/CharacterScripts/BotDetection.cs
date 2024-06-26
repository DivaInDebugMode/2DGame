using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotDetection : MonoBehaviour
    {
        [SerializeField]
        private Transform groundTransform;
        [SerializeField] private Transform leftFeetTransform;
        [SerializeField] private Transform rightFeetTransform;
        [SerializeField] private bool isRightFootOnGround;
        [SerializeField] private bool isLeftFootOnEdge;
        private readonly Vector3 feetDirection = Vector3.down;
        [SerializeField] private BotData botData;

        public bool IsRightFootOnGround
        {
            get => isRightFootOnGround;
        }
        
        public bool IsLeftFootOnEdge
        {
            get => isLeftFootOnEdge;
        }

        private void Update()
        {
            IsGrounded();
        }

        private void IsGrounded()
        {
            botData.BotDetectionStats.IsGrounded = botData.BotComponents.Rb.velocity.y <= 0.01f && Physics.CheckSphere(
                groundTransform.position, 0.2f, botData.BotDetectionStats.Grounded);
        }

        public void CheckGroundRightFoot()
        {
             isRightFootOnGround = Physics.Raycast(rightFeetTransform.position, feetDirection,
                0.1f, botData.BotDetectionStats.Grounded | botData.BotDetectionStats.FallingEdge);
        }
        
        public void CheckGroundLeftFoot()
        {
            isLeftFootOnEdge = Physics.Raycast(leftFeetTransform.position, feetDirection,
                0.1f, botData.BotDetectionStats.FallingEdge);
        }
     
    }
}