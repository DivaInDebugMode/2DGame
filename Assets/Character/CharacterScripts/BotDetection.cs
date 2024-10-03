using System;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotDetection : MonoBehaviour
    {
        [SerializeField] private Transform groundTransform;
        [SerializeField] private BotData botData;
        [SerializeField] private Transform ledgeDetectionTransform;
        [SerializeField] private Transform wallDetectionTransform;
        [SerializeField] private GameObject glideObj;
        public GameObject GlideObj => glideObj;
        private Transform LedgeDetectionTransform => ledgeDetectionTransform;
        private Transform WallDetectionTransform => wallDetectionTransform;
        public Transform GroundTransform => groundTransform;

        private void Update()
        {
            IsGrounded();
            IsWallOrIsLedge();
        }
        private void IsGrounded()
        {
            var bounds = botData.MoveCollider.bounds;
            var bottom = bounds.center -
                         new Vector3(0, bounds.extents.y, 0);
            botData.BotDetectionStats.IsGrounded = Physics.CheckSphere(
                bottom, 0.34f, botData.BotDetectionStats.Grounded | 
                               botData.BotDetectionStats.HorizontalPlatform | botData.BotDetectionStats.VerticalPlatform);
        }
        
        public void IsNearOnGround()
        {
            var bounds = botData.MoveCollider.bounds;
            var bottom = bounds.center -
                         new Vector3(0, bounds.extents.y, 0);
            botData.BotDetectionStats.IsNearOnGround = Physics.CheckSphere(
                bottom, 0.7f, botData.BotDetectionStats.Grounded | 
                            botData.BotDetectionStats.HorizontalPlatform | botData.BotDetectionStats.VerticalPlatform);
        }
        
        private void IsWallOrIsLedge()
        {
            if (botData.BotStats.IsInLedgeClimbing) return;
            if (botData.BotDetectionStats.IsGrounded) return;
            switch (botData.BotStats.CurrentDirectionValue)
            {
                case 1:
                    botData.BotDetectionStats.IsLedge = Physics.Raycast(LedgeDetectionTransform.position, Vector3.right,
                        0.5f, botData.BotDetectionStats.Grounded  | botData.BotDetectionStats.HorizontalPlatform);
                    
                    botData.BotDetectionStats.IsWall = Physics.Raycast(WallDetectionTransform.position, Vector3.right,
                        botData.BotDetectionStats.WallDetectionRadius, botData.BotDetectionStats.Wall);
                    break;
                case -1:
                    botData.BotDetectionStats.IsLedge = Physics.Raycast(LedgeDetectionTransform.position,
                        -Vector3.right,
                        0.5f, botData.BotDetectionStats.Grounded | botData.BotDetectionStats.HorizontalPlatform);
                    
                    botData.BotDetectionStats.IsWall = Physics.Raycast(WallDetectionTransform.position, -Vector3.right,
                        botData.BotDetectionStats.WallDetectionRadius, botData.BotDetectionStats.Wall);
                    break;
            }
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

        private void OnCollisionEnter(Collision collision)
        {
            if (((1 << collision.gameObject.layer) & botData.BotDetectionStats.HorizontalPlatform.value) != 0)
            {
                transform.parent = collision.transform;
            }
            
            if (((1 << collision.gameObject.layer) & botData.BotDetectionStats.VerticalPlatform.value) != 0)
            {
                transform.parent = collision.transform;
            }
        }

        private void OnCollisionStay(Collision collisionInfo)
        {
            if (((1 << collisionInfo.gameObject.layer) & botData.BotDetectionStats.HorizontalPlatform.value) != 0)
            {
                if (botData.BotStats.MoveDirection.x != 0 || botData.BotStats.IsJump)
                {
                    transform.parent = null;
                }
                else if(botData.BotStats.MoveDirection.x == 0 || !botData.BotStats.IsJump)
                {
                    transform.parent = collisionInfo.transform;
                }
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (((1 << other.gameObject.layer) & botData.BotDetectionStats.HorizontalPlatform.value) != 0)
            {
                transform.parent = null;
            }
            
            if (((1 << other.gameObject.layer) & botData.BotDetectionStats.VerticalPlatform.value) != 0)
            {
                transform.parent = null;
            }
        }
    }
}
