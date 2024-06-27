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

        public Transform LedgeDetectionTransform
        {
            get => ledgeDetectionTransform;
        }


        public Transform WallDetectionTransform
        {
            get => wallDetectionTransform;
        }
        
        private void Update()
        {
            IsGrounded();
            CheckLedge();
        }

        private void IsGrounded()
        {
            botData.BotDetectionStats.IsGrounded = botData.BotComponents.Rb.velocity.y <= 0.01f && Physics.CheckSphere(
                groundTransform.position, 0.2f, botData.BotDetectionStats.Grounded);
        }

        private void CheckLedge()
        {
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

        private void OnDrawGizmos()
        {
            if (ledgeDetectionTransform != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(ledgeDetectionTransform.position, ledgeDetectionTransform.position + Vector3.right * (botData.BotStats.CurrentDirectionValue == 1 ? 1f : -1f));
            }

            if (wallDetectionTransform != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(wallDetectionTransform.position, wallDetectionTransform.position + Vector3.right * (botData.BotStats.CurrentDirectionValue switch
                {
                    1 => botData.BotDetectionStats.WallDetectionRadius,
                    _ => -botData.BotDetectionStats.WallDetectionRadius,
                }));
            }
        }

    }
}