using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotDetection : MonoBehaviour
    {
        [SerializeField]
        private Transform groundTransform;
        [SerializeField] private Transform edgeFeetTransform;
        [SerializeField] private BotData botData;

        private void Update()
        {
            IsGrounded();
           
        }

        private void IsGrounded()
        {
            botData.BotDetectionStats.IsGrounded = botData.BotComponents.Rb.velocity.y <= 0.01f && Physics.CheckSphere(
                groundTransform.position, 0.2f, botData.BotDetectionStats.Grounded);
        }

        public void CheckGroundDistance()
        {
            var rayDirection = Vector3.down; 
            var rayOrigin = edgeFeetTransform.position; 
            var edgeHit = Physics.Raycast(rayOrigin, rayDirection,
                0.1f, botData.BotDetectionStats.FallingEdge);
            if (edgeHit)
            {
                botData.BotComponents.Rb.velocity= Vector3.zero; 
                botData.BotComponents.Coll.center = botData.BotStats.CollCentreEdge;
                botData.BotStats.IsRotating = false;
                botData.BotStats.IsRunning = false;
                botData.BotStats.IsFallingEdge = true;
               Debug.Log("0");
            }
            else
            {
                botData.BotStats.IsFallingEdge = false;
                if (botData.BotDetectionStats.IsGrounded) return;
                Debug.Log("1");
            }
        }
     
    }
}