using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

namespace Character.CharacterScripts
{
    public class BotDetection : MonoBehaviour
    {
        [SerializeField] private Transform groundTransform;
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
        }


        private void IsGrounded()
        {
            var bounds = botData.BotComponents.MoveCollider.bounds;
            var bottom = bounds.center -
                         new Vector3(0, bounds.extents.y, 0);
            botData.BotDetectionStats.IsGrounded = Physics.CheckSphere(
                   bottom, 0.1f, botData.BotDetectionStats.Grounded | botData.BotDetectionStats.Platform);
        }

        public void IsNearOnGround()
        {
            var bounds = botData.BotComponents.MoveCollider.bounds;
            var bottom = bounds.center -
                         new Vector3(0, bounds.extents.y, 0);
            botData.BotDetectionStats.IsNearOnGround = Physics.CheckSphere(
                bottom, 1f, botData.BotDetectionStats.Grounded | botData.BotDetectionStats.Platform);
        }

        private void CheckWall()
        {
            if (botData.BotStats.IsInLedgeClimbing) return;
            if (botData.BotDetectionStats.IsGrounded) return;
            switch (botData.BotStats.CurrentDirectionValue)
            {
                case 1:
                    botData.BotDetectionStats.IsLedge = Physics.Raycast(LedgeDetectionTransform.position, Vector3.right,
                        0.5f, botData.BotDetectionStats.Grounded);
                    botData.BotDetectionStats.IsWall = Physics.Raycast(WallDetectionTransform.position, Vector3.right,
                        botData.BotDetectionStats.WallDetectionRadius, botData.BotDetectionStats.Wall);
                    break;
                case -1:
                    botData.BotDetectionStats.IsLedge = Physics.Raycast(LedgeDetectionTransform.position,
                        -Vector3.right,
                        0.5f, botData.BotDetectionStats.Grounded);
                    botData.BotDetectionStats.IsWall = Physics.Raycast(WallDetectionTransform.position, -Vector3.right,
                        botData.BotDetectionStats.WallDetectionRadius, botData.BotDetectionStats.Wall);
                    break;
            }
        }

        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.contacts.Length == 0) return;
            if (((1 << collision.gameObject.layer) & botData.BotDetectionStats.Platform) == 0) return;
            if (!collision.contacts.Any(contact => contact.normal.y > 0.9f)) return;
            transform.SetParent(collision.transform, true);
            botData.BotComponents.Rb.interpolation = RigidbodyInterpolation.None;
        }

        private void OnCollisionStay(Collision collision)
        {
          
                if (collision.contacts.Length == 0) return;
                if (((1 << collision.gameObject.layer) & botData.BotDetectionStats.Platform) == 0) return;
                foreach (var contact in collision.contacts)
                {
                    if (contact.normal.y > 0.9f) 
                    {
                        if (botData.BotStats.MoveDirection == Vector3.zero && transform.parent != collision.transform)
                        {
                            transform.SetParent(collision.transform);
                            botData.BotComponents.Rb.interpolation = RigidbodyInterpolation.None;

                        }
                        
                    }

                    if (botData.BotStats.MoveDirection == Vector3.zero || transform.parent == null) continue;
                    transform.SetParent(null);
                    botData.BotComponents.Rb.interpolation = RigidbodyInterpolation.Interpolate;

                }
         
        }

        private void OnCollisionExit(Collision collision)
        {
            if (((1 << collision.gameObject.layer) & botData.BotDetectionStats.Platform) == 0) return;
            if (transform.parent != collision.transform) return;
            transform.parent = null;
            botData.BotComponents.Rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }
}
