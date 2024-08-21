using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.CharacterScripts
{
    public class BotJump : MonoBehaviour
    {
        [SerializeField] private BotData botData;
        [SerializeField] private BotInput botInput;
       
        private bool isPressed;
        private float pressStartTime;
        private float jumpPressedTime;
        private bool isTap;
        private float dropTimer;
        private bool shouldDrop;

        private void OnEnable()
        {
            botInput.Jump.action.started += JumpActionPress;
            botInput.Jump.action.canceled += OnButtonCancel;
        }

        private void OnDisable()
        {
            botInput.Jump.action.started -= JumpActionPress;
            botInput.Jump.action.canceled -= OnButtonCancel;
        }

        private void JumpActionPress(InputAction.CallbackContext context)
        {
            if (botData.BotDetectionStats.IsGrounded | botData.BotDetectionStats.IsOnPlatform && !botData.BotStats.IsGroundDashing &&
                !botData.BotStats.IsCrouching && !botData.BotStats.HasJumped)
            {
                botData.BotStats.HasJumped = true;
                botData.BotStats.IsJump = true;
                pressStartTime = Time.time;
                isPressed = true;
                isTap = false;
                shouldDrop = false;
                dropTimer = 0f;
                botData.BotComponents.Rb.velocity = new Vector2(botData.BotComponents.Rb.velocity.x, botData.BotStats.JumpForce);
            }
        }

        private void Update()
        {
            if(botData.BotStats.IsAirDashing) return;

            if (isPressed)
            {
                jumpPressedTime = Time.time - pressStartTime;
                isTap = jumpPressedTime <= 0.1f;
            }
        }

        private void FixedUpdate()
        { 
            if(botData.BotStats.IsAirDashing) return;
            if (isTap && !shouldDrop)
            {
                dropTimer += Time.deltaTime;
                if (dropTimer >= 0.12f)
                {
                    botData.BotComponents.Rb.velocity = new Vector2(botData.BotComponents.Rb.velocity.x, botData.BotStats.InitialJumpForce);
                    shouldDrop = true;
                }
            }
        }

        private void OnButtonCancel(InputAction.CallbackContext context)
        {
            if (botData.BotDetectionStats.IsWall) return;

            isPressed = false;
            if (jumpPressedTime > 0.1f)
            {
                isTap = false;
                dropTimer = 0f;
                shouldDrop = false;
            }
        }
    }
}