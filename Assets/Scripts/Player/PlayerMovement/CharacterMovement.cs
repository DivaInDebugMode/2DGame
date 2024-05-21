using UnityEngine;

// Namespace for managing player movement
namespace Player.PlayerMovement
{
    // Class responsible for character movement
    public class CharacterMovement : MonoBehaviour
    {
        // Serialized fields for character components and input
        [SerializeField] private CharacterComponents characterComponents;
        [SerializeField] private CharacterInput characterInput;
        [SerializeField] private CharacterStats characterStats;
        [SerializeField] private DetectionStats detectionStats;

        // Called every frame
        private void Update()
        {
            // Handling character input
            HandleInput();
        }

        // Method to handle character input
        private void HandleInput()
        {
            // If character is against a wall, adjust movement vector accordingly
            if (detectionStats.IsWall)
            {
                characterStats.MoveVector = new Vector2(
                    characterInput.MoveRight.action.ReadValue<float>() - characterInput.MoveLeft.action.ReadValue<float>(),
                    characterInput.MoveUp.action.ReadValue<float>() - characterInput.MoveDown.action.ReadValue<float>()
                ).normalized;
            }
            else if(!characterStats.IsDashing)
            {
                // If not against a wall, only horizontal movement is allowed
                characterStats.MoveVector = new Vector2(
                    characterInput.MoveRight.action.ReadValue<float>() - characterInput.MoveLeft.action.ReadValue<float>(),
                    0
                ).normalized;
            }
        }

        // Method to move character vertically
        public void MoveVertically(float verticalSpeed)
        {
            characterStats.VelocityY = Mathf.MoveTowards(characterComponents.Rb.velocity.y, verticalSpeed,
                2);
            characterComponents.Rb.velocity = new Vector2(characterComponents.Rb.velocity.x, characterStats.VelocityY);
        }
        
        // Method to move character horizontally
        public void MoveHorizontally(float horizontalSpeed)
        {
            if (!characterStats.IsDashing)
            {
                // Move character horizontally with smoothing
                characterStats.VelocityX = Mathf.MoveTowards(characterComponents.Rb.velocity.x,
                    horizontalSpeed, characterStats.SmoothTime * Time.fixedTime);
                characterComponents.Rb.velocity =
                    new Vector2(characterStats.VelocityX, characterComponents.Rb.velocity.y);
            }
        }
    }
}
