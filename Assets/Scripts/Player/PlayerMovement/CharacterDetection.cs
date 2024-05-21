using UnityEngine;

namespace Player.PlayerMovement // Namespace for managing player movement
{
    // Class responsible for character detection, inheriting from MonoBehaviour
    public class CharacterDetection : MonoBehaviour
    {
        [Header("Transform")] // Header for organization in Unity Inspector
        [SerializeField]
        private Transform groundTransform; // Reference to ground detection transform

        [SerializeField] private Transform headTransformForLedge; // Reference to head transform for ledge detection
        [SerializeField] private Transform bodyTransformForWall; // Reference to body transform for wall detection

        public Vector2 RayDirection { get; set; }

        [field: Header("Ray")] // Header for organization in Unity Inspector
        public RaycastHit2D HeadRayHit { get; private set; } // RaycastHit2D for head ray hit detection

        private RaycastHit2D checkRayHit; // Temporary RaycastHit2D for ray hit detection

        public RaycastHit2D GroundRayHit { get; private set; }

        // Properties for move on ledge and input for ledge climb flags
        public bool HasMoveOnLedge { get; set; }
        public bool IsInputForLedgeClimb { get; set; }
        private bool isLedgeParent;
        private bool isPlatformParent;
        private bool isDefaultParent;
        [Header("Class")] // Header for organization in Unity Inspector
        [SerializeField]
        private DetectionStats detectionStats; // Reference to detection statistics

        [SerializeField] private CharacterComponents characterComponents; // Reference to character components
        [SerializeField] private CharacterStats stats;


        // Method called on every frame update
        private void FixedUpdate()
        {
            IsWall();
            IsOnPlatform();
            IsGrounded(); // Checks if character is on a platform
            
        }
        private void Update()
        {
            CheckForLedgeWithRay(); // Checks for ledge presence with ray
            CheckGroundDistance();
        }

        private void IsGrounded()
        {
            // Checks if character's vertical velocity is close to zero and if there's an overlap circle at ground position
            detectionStats.IsGrounded =  characterComponents.Rb.velocity.y <= 0.01f && Physics2D.OverlapCircle(
                groundTransform.position, 0.2f, detectionStats.Grounded);
        }
       
        private void IsWall()
        {
            // Checks for wall presence with raycast based on character flip status
          detectionStats.IsWall =   CheckForWallWithRay() && characterComponents.Sr.flipX == false ||
                   CheckForWallWithRay() && characterComponents.Sr.flipX;
        
        }
        
        // Method to perform wall detection with raycast
        private bool CheckForWallWithRay()
        {
            // Defines ray direction based on character flip status and casts ray to detect wall
           RayDirection = bodyTransformForWall.right * (characterComponents.Sr.flipX ? -1 : 1);
            return Physics2D.Raycast(bodyTransformForWall.position, RayDirection,
                detectionStats.WallCollisionRadius, detectionStats.WallMask);
            
        }

        public void CheckGroundDistance()
        {
            Vector2 rayDirection = -headTransformForLedge.up;
            GroundRayHit = Physics2D.Raycast(groundTransform.position, rayDirection, 9999,detectionStats.Grounded);

        }

        private void CheckForLedgeWithRay()
        {
            Vector2 rayDirection = headTransformForLedge.right * (characterComponents.Sr.flipX ? -1 : 1);
            HeadRayHit = Physics2D.Raycast(headTransformForLedge.position, rayDirection,
                detectionStats.LedgeLength, detectionStats.Grounded | detectionStats.Platform);

            if (HeadRayHit.collider is null ||
                (((1 << HeadRayHit.collider.gameObject.layer) & detectionStats.Platform) == 0) ||
                (detectionStats.IsLedgeDetected || detectionStats.IsGrounded || isLedgeParent)) return;
            characterComponents.Rb.transform.SetParent(HeadRayHit.transform);
            isLedgeParent = true;
        }
        
      
        private void IsOnPlatform()
        {
            detectionStats.IsOnPlatform  = characterComponents.Rb.velocity.y <= 0.01f && Physics2D.OverlapCircle(
                groundTransform.position, 0.2f, detectionStats.Platform);
            if (stats.MoveVector.x == 0 && detectionStats.IsOnPlatform && !stats.IsDashing)
            {
                var platformCollider = Physics2D.OverlapCircle(groundTransform.position, 0.2f,
                    detectionStats.Platform);
                if (isPlatformParent) return;
                characterComponents.Rb.transform.SetParent(platformCollider.transform);
                isPlatformParent = true;
            }
            else
            {
                if ((characterComponents.Rb.velocity.x == 0 && characterComponents.Rb.velocity.y == 0 &&
                     (detectionStats.IsOnPlatform || detectionStats.IsLedgeDetected || isDefaultParent)) ||
                    characterComponents.Rb.transform.parent is null) return;
                characterComponents.Rb.transform.SetParent(null);
                isDefaultParent = true;
                isLedgeParent = false;
                isPlatformParent = false;
            }
        }
    }
}
