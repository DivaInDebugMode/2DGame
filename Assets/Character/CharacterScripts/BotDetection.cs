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

        private Transform LedgeDetectionTransform => ledgeDetectionTransform;
        private Transform WallDetectionTransform => wallDetectionTransform;


        private void Update()
        {
            IsGrounded();
            CheckWall();
        }

        private void IsGrounded()
        {
            botData.BotDetectionStats.IsGrounded = botData.BotComponents.Rb.velocity.y <= 0.01f && Physics.CheckSphere(
                groundTransform.position, 0.2f, botData.BotDetectionStats.Grounded);
        }
        

        public void IsNearOnGround()
        {
            botData.BotDetectionStats.IsNearOnGround = botData.BotComponents.Rb.velocity.y <= 0.01f && !botData.BotStats.IsDashing && Physics.CheckSphere(
                groundTransform.position, 0.7f, botData.BotDetectionStats.Grounded);
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
    }
}