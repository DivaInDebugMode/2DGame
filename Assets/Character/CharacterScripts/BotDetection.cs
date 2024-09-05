
using System;
using System.Linq;
using Character.CharacterScriptable;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotDetection : MonoBehaviour
    {
        [SerializeField] private Transform groundTransform;
        [SerializeField] private Transform groundLedgeTransform;
        [SerializeField] private BotData botData;
        [SerializeField] private Transform ledgeDetectionTransform;
        [SerializeField] private Transform wallDetectionTransform;
        private Transform LedgeDetectionTransform => ledgeDetectionTransform;
        private Transform WallDetectionTransform => wallDetectionTransform;
        public Transform GroundTransform => groundTransform;

        private void Update()
        {
            IsGrounded();
            CheckWall();
            CheckIce();
            PanchuriKidezeWithFeet();
        }
        private void IsGrounded()
        {
            var bounds = botData.BotComponents.MoveCollider.bounds;
            var bottom = bounds.center -
                         new Vector3(0, bounds.extents.y, 0);
            botData.BotDetectionStats.IsGrounded = Physics.CheckSphere(
                bottom, 0.35f, botData.BotDetectionStats.Grounded | botData.BotDetectionStats.Platform);
        }
        
        public void IsNearOnGround()
        {
            var bounds = botData.BotComponents.MoveCollider.bounds;
            var bottom = bounds.center -
                         new Vector3(0, bounds.extents.y, 0);
            botData.BotDetectionStats.IsNearOnGround = Physics.CheckSphere(
                bottom, 1f, botData.BotDetectionStats.Grounded | botData.BotDetectionStats.Platform | botData.BotDetectionStats.Ice );
        }

        private void CheckWall()
        {
            if (botData.BotStats.IsInLedgeClimbing) return;
            if (botData.BotDetectionStats.IsGrounded) return;
            switch (botData.BotStats.CurrentDirectionValue)
            {
                case 1:
                    botData.BotDetectionStats.IsLedge = Physics.Raycast(LedgeDetectionTransform.position, Vector3.right,
                        0.5f, botData.BotDetectionStats.Grounded  | botData.BotDetectionStats.Platform);
                    botData.BotDetectionStats.IsWall = Physics.Raycast(WallDetectionTransform.position, Vector3.right,
                        botData.BotDetectionStats.WallDetectionRadius, botData.BotDetectionStats.Wall);
                    break;
                case -1:
                    botData.BotDetectionStats.IsLedge = Physics.Raycast(LedgeDetectionTransform.position,
                        -Vector3.right,
                        0.5f, botData.BotDetectionStats.Grounded | botData.BotDetectionStats.Platform);
                    botData.BotDetectionStats.IsWall = Physics.Raycast(WallDetectionTransform.position, -Vector3.right,
                        botData.BotDetectionStats.WallDetectionRadius, botData.BotDetectionStats.Wall);
                    break;
            }
        }

        private void PanchuriKidezeWithFeet()
        {
            botData.BotDetectionStats.IsOnEdgeWithSecondFoot = Physics.Raycast(groundTransform.position,
                Vector3.down, 0.2f, botData.BotDetectionStats.Grounded | botData.BotDetectionStats.Platform | botData.BotDetectionStats.Edge);
            
            botData.BotDetectionStats.IsClimbingGround = Physics.Raycast(groundLedgeTransform.position,
                Vector3.down, 1f, botData.BotDetectionStats.Grounded | botData.BotDetectionStats.Platform | botData.BotDetectionStats.Edge);
        }

        private void CheckIce()
        {
            var bounds = botData.BotComponents.MoveCollider.bounds;
            var bottom = bounds.center - new Vector3(0, bounds.extents.y, 0);
            var size = Mathf.Max(bounds.extents.x, bounds.extents.z); 
            botData.BotDetectionStats.IsOnIce = Physics.CheckSphere(
                bottom, size, botData.BotDetectionStats.Ice);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & botData.BotDetectionStats.HurricaneBounce.value) != 0)
            {
                botData.BotStats.IsHurricaneBounce = true;
                botData.BotStats.CanAirDash = true;

            }else if (((1 << other.gameObject.layer) & botData.BotDetectionStats.MegaJumpBounce.value) != 0)
            {
                botData.BotStats.IsMegaBounce = true;
                botData.BotStats.CanAirDash = true;
            }
            
            
        }
        
        private void OnDrawGizmos()
        {
            if (botData == null || botData.BotComponents.MoveCollider == null)
                return;

            // Draw bounds of the collider
            var bounds = botData.BotComponents.MoveCollider.bounds;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(bounds.center, bounds.size);

            // Calculate bottom position of the bounds
            var bottom = bounds.center - new Vector3(0, bounds.extents.y, 0);

            // Draw sphere at the bottom of the collider
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(bottom, 0.35f);
        }
    }
}
