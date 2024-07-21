using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotDetection : MonoBehaviour
    {
        [SerializeField]
        private Transform groundTransform;
        [SerializeField] private BotData botData;
        [SerializeField] private Transform ledgeDetectionTransform;
        [SerializeField] private Transform wallDetectionTransform;
        [SerializeField] private Transform ropeDetectionTransform;
        private RaycastHit ropeTrailHit;

        public RaycastHit RopeTrailHit
        {
            get => ropeTrailHit;
            set => ropeTrailHit = value;
        }

        private Transform LedgeDetectionTransform => ledgeDetectionTransform;
        private Transform WallDetectionTransform => wallDetectionTransform;

        public Transform GroundTransform => groundTransform;


        private void Update()
        {
            IsGrounded();
            CheckWall();
            CheckRope();
        }

        private void IsGrounded()
        {
            botData.BotDetectionStats.IsGrounded = botData.BotComponents.Rb.velocity.y <= 0.01f && Physics.CheckSphere(
                groundTransform.position, 0.2f, botData.BotDetectionStats.Grounded);
        }

        public void IsNearOnGround()
        {
            botData.BotDetectionStats.IsNearOnGround = botData.BotComponents.Rb.velocity.y <= 0.01f && !botData.BotStats.IsGroundDashing && Physics.CheckSphere(
                groundTransform.position, 1f, botData.BotDetectionStats.Grounded);
        }
        
        private void CheckWall()
        {
            if (botData.BotStats.IsInLedgeClimbing) return;
            if(botData.BotDetectionStats.IsGrounded) return;
            switch (botData.BotStats.CurrentDirectionValue)
            {
                case 1:
                    botData.BotDetectionStats.IsLedge = Physics.Raycast(LedgeDetectionTransform.position, Vector3.right,
                        0.5f, botData.BotDetectionStats.Grounded);
                    botData.BotDetectionStats.IsWall = Physics.Raycast(WallDetectionTransform.position, Vector3.right,
                        botData.BotDetectionStats.WallDetectionRadius, botData.BotDetectionStats.Wall);
                    break;
                case -1:
                    botData.BotDetectionStats.IsLedge = Physics.Raycast(LedgeDetectionTransform.position, -Vector3.right,
                        0.5f, botData.BotDetectionStats.Grounded);
                    botData.BotDetectionStats.IsWall = Physics.Raycast(WallDetectionTransform.position, -Vector3.right,
                        botData.BotDetectionStats.WallDetectionRadius, botData.BotDetectionStats.Wall);
                    break;
            }
        }

        private void CheckRope()
        {
            if(botData.BotDetectionStats.IsGrounded) return;
            switch (botData.BotStats.CurrentDirectionValue)
            {
                case 1:
                    botData.BotDetectionStats.IsRope = Physics.Raycast(ropeDetectionTransform.position, Vector3.right,
                        0.5f, botData.BotDetectionStats.Rope);
                    botData.BotDetectionStats.IsRopeTail = Physics.Raycast(ropeDetectionTransform.position, Vector3.right, out ropeTrailHit,
                        0.5f, botData.BotDetectionStats.RopeTail);
                    break;
                case -1:
                    botData.BotDetectionStats.IsRope = Physics.Raycast(ropeDetectionTransform.position, -Vector3.right,
                        0.5f, botData.BotDetectionStats.Rope);
                    botData.BotDetectionStats.IsRopeTail = Physics.Raycast(ropeDetectionTransform.position, -Vector3.right, out ropeTrailHit,
                        0.5f, botData.BotDetectionStats.RopeTail);
                    break;
                
            }
        }
    }
}