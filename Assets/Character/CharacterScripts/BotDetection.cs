
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
        [SerializeField] private Transform groundDetectionFrontFoot;
        [SerializeField] private GameObject glideObj;

        public GameObject GlideObj
        {
            get => glideObj;
            set => glideObj = value;
        }
        private Transform LedgeDetectionTransform => ledgeDetectionTransform;
        private Transform WallDetectionTransform => wallDetectionTransform;
        public Transform GroundTransform => groundTransform;

        private void Update()
        {
            IsGrounded();
            CheckWall();
            CheckIce();
            KickFromEdge();
        }
        private void IsGrounded()
        {
            var bounds = botData.BotComponents.MoveCollider.bounds;
            var bottom = bounds.center -
                         new Vector3(0, bounds.extents.y, 0);
            botData.BotDetectionStats.IsGrounded = Physics.CheckSphere(
                bottom, 0.32f, botData.BotDetectionStats.Grounded | botData.BotDetectionStats.Platform);
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

        private void KickFromEdge()
        {
            botData.BotDetectionStats.IsOnEdgeWithSecondFoot = Physics.Raycast(groundTransform.position,
                Vector3.down, 0.2f, botData.BotDetectionStats.Grounded | botData.BotDetectionStats.Platform | botData.BotDetectionStats.Edge);
    
            botData.BotDetectionStats.IsClimbingGround = Physics.Raycast(groundLedgeTransform.position,
                Vector3.down, 1.2f, botData.BotDetectionStats.Grounded | botData.BotDetectionStats.Platform | botData.BotDetectionStats.Edge);
    
            botData.BotDetectionStats.IsGroundFrontFoot = Physics.Raycast(groundDetectionFrontFoot.position,
                Vector3.down, 1.2f, botData.BotDetectionStats.Grounded | botData.BotDetectionStats.Platform | botData.BotDetectionStats.Edge);
        }

        private void OnDrawGizmos()
        {
            // Raycast for edge detection with the second foot
            Gizmos.color = Color.red; // Color for the first raycast
            Gizmos.DrawLine(groundTransform.position, groundTransform.position + Vector3.down * 0.2f);
    
            // Raycast for climbing ground detection
            Gizmos.color = Color.green; // Color for the second raycast
            Gizmos.DrawLine(groundLedgeTransform.position, groundLedgeTransform.position + Vector3.down * 1.2f);
    
            // Raycast for ground detection on the front foot
            Gizmos.color = Color.blue; // Color for the third raycast
            Gizmos.DrawLine(groundDetectionFrontFoot.position, groundDetectionFrontFoot.position + Vector3.down * 1.2f);
            
            if (botData == null || botData.BotComponents.MoveCollider == null) return;
    
            var bounds = botData.BotComponents.MoveCollider.bounds;
            var bottom = bounds.center - new Vector3(0, bounds.extents.y, 0);
    
            // Draw Gizmo for IsGrounded (small sphere)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(bottom, 0.315f);
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
                botData.BotStats.StopGlide = true;

            }else if (((1 << other.gameObject.layer) & botData.BotDetectionStats.MegaJumpBounce.value) != 0)
            {
                botData.BotStats.IsMegaBounce = true;
                botData.BotStats.CanAirDash = true;
                botData.BotStats.StopGlide = true;
                botData.BotStats.IsGroundDashing = false;
            }
        }

    }
}
