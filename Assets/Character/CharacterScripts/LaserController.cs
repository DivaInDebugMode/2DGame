using UnityEngine;

namespace Character.CharacterScripts
{
    public class LaserController : MonoBehaviour
    {
        public enum LaserType
        {
            Rotating,
            Moving,
            MovingAndRotating
        }

        [Header("Laser Type")] [SerializeField]
        private LaserType laserType = LaserType.Rotating; // Choose the laser type

        [Header("Rotation Settings")] [SerializeField]
        private Vector3 rotationAxis = Vector3.up; // Axis to rotate around

        [SerializeField] private float rotationSpeed = 100f; // Rotation speed
        [SerializeField] private float rotationAngle = 360f; // Max angle to rotate (e.g. 45, 90, 360 degrees)

        private float currentRotation = 0f; // Track the current rotation
        private bool rotatingForward = true; // Control direction of rotation

        [Header("Movement Settings")] [SerializeField]
        private Vector3 moveDirection = Vector3.right; // Direction to move

        [SerializeField] private float moveDistance = 5f; // Distance to move from the starting position
        [SerializeField] private float moveSpeed = 5f; // Speed to move

        private Vector3 startPosition;
        private Vector3 targetPosition;
        private bool movingForward = true;

        void Start()
        {
            // Store the initial position of the laser
            startPosition = transform.position;

            // Set the initial target position for movement
            targetPosition = startPosition + moveDirection * moveDistance;
        }

        void Update()
        {
            // Determine laser behavior based on the selected type
            switch (laserType)
            {
                case LaserType.Rotating:
                    RotateLaserWithAngleLimit();
                    break;

                case LaserType.Moving:
                    MoveLaserBetweenRelativePoints();
                    break;

                case LaserType.MovingAndRotating:
                    MoveLaserBetweenRelativePoints();
                    RotateLaserWithAngleLimit();
                    break;
            }
        }

        // Rotates the laser with an angle limit
        private void RotateLaserWithAngleLimit()
        {
            // Calculate the rotation for this frame
            float rotationStep = rotationSpeed * Time.deltaTime;

            // Check if we are rotating forward or backward
            if (rotatingForward)
            {
                // Rotate the laser forward
                transform.Rotate(rotationAxis * rotationStep);
                currentRotation += rotationStep;

                // Check if we've reached the max rotation angle
                if (currentRotation >= rotationAngle)
                {
                    rotatingForward = false; // Reverse direction
                }
            }
            else
            {
                // Rotate the laser backward
                transform.Rotate(-rotationAxis * rotationStep);
                currentRotation -= rotationStep;

                // Check if we've reached the starting angle (0 degrees)
                if (currentRotation <= 0)
                {
                    rotatingForward = true; // Reverse direction
                }
            }
        }

        // Moves the laser between the starting position and the relative target position
        private void MoveLaserBetweenRelativePoints()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                targetPosition = movingForward
                    ? startPosition - moveDirection * moveDistance
                    : startPosition + moveDirection * moveDistance;
                movingForward = !movingForward;
            }
        }
    }
}