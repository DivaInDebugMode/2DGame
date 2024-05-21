using Cinemachine;
using Player.PlayerMovement;
using Save_Load;
using UnityEngine;

// Namespace for camera-related functionality
namespace Camera
{
    // Class responsible for making the camera follow a target
    public class CameraFollow : MonoBehaviour, IDataPersistence
    {
        // Reference to the target object to follow
        [SerializeField] private Transform target;
       // [SerializeField] private CharacterComponents characterComponents;
        [SerializeField] private float smoothSpeed = 0.125f;
        private Vector3 desiredPosition, currentPosition, targetPosition, smoothedPosition;
        
        private void Update()
        {
            if (target is null) return;

            // switch (characterComponents.Sr.flipX)
            // {
            //     case false:
            //         targetPosition = target.position;
            //         currentPosition = transform.position;
            //         desiredPosition = new Vector3(targetPosition.x + 0.5f, targetPosition.y + 1, currentPosition.z);
            //         break;
            //     default:
            //         targetPosition = target.position;
            //         currentPosition = transform.position;
            //         desiredPosition = new Vector3(targetPosition.x - 0.5f, targetPosition.y + 1, currentPosition.z);
            //         break;
            // }
            
            targetPosition = target.position;
            currentPosition = transform.position;
            desiredPosition = new Vector3(targetPosition.x + 0.5f, targetPosition.y + 1, currentPosition.z);
            smoothedPosition = Vector3.Lerp(currentPosition, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
        
        // Method to load camera data from a saved game state
        public void LoadData(GameData data)
        {
            // Set the camera's position based on the loaded data
            transform.position = data.cameraPos;
        }

        // Method to save camera data to a game state
        public void SaveData(ref GameData data)
        {
            // Save the camera's position to the game data
            data.cameraPos = transform.position;
        }
    }
}
